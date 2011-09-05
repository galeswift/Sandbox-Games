using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content;
using Xen.Ex.Graphics.Content;
using System.Reflection;
using System.ComponentModel;

namespace Xen.Ex.ModelImporter
{

	[ContentTypeWriter]
	class ModelDataWriter : ContentTypeWriter<ModelData>
	{
		protected override void Write(ContentWriter output, ModelData value)
		{
			Func<BinaryWriter, object, bool> writeVertexData = WriteVB;
			typeof(ModelData).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(value, new object[] { output, writeVertexData });
		}
		
		private static bool WriteVB(BinaryWriter writer, object vertexData)
		{
			(writer as ContentWriter).WriteObject((VertexBufferContent)vertexData);
			return true;
		}

		public override string GetRuntimeReader(TargetPlatform targetPlatform)
		{
			return (string)typeof(ModelData).GetProperty("RuntimeReaderType", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, new object[0]);
		}
	}

	[ContentProcessor(DisplayName = "Model - Xen")]
	public class ModelImporter : ContentProcessor<NodeContent, ModelData>
	{
		public override ModelData Process(NodeContent input, ContentProcessorContext context)
		{
			List<MeshData> meshes = new List<MeshData>();
			SkeletonData skeleton = null;
			List<AnimationData> animations = new List<AnimationData>();

			List<SkeletonData> skeletons = new List<SkeletonData>();


			string rootPath = input.Identity.SourceFilename;

			Dictionary<string, object> processedContent = new Dictionary<string, object>();


			if (this.RotationX != 0 || this.RotationY != 0 || this.RotationZ != 0 || this.Scale != 1)
			{
				Matrix transform =
					Matrix.CreateRotationZ(MathHelper.ToRadians(this.RotationZ)) *
					Matrix.CreateRotationX(MathHelper.ToRadians(this.RotationX)) *
					Matrix.CreateRotationY(MathHelper.ToRadians(this.RotationY)) *
					Matrix.CreateScale(this.Scale);

				MeshHelper.TransformScene(input, transform);
			}


			ProcessSkeletonNodes(input, context, skeletons);

			//there shouldn't be more than one skeleton... but anyway...

			Dictionary<string, int> boneIndices = null;
			if (skeletons.Count > 0 && importAnimations)
			{
				List<BoneData> bones = new List<BoneData>();
				List<Matrix> transforms = new List<Matrix>();

				foreach (SkeletonData sk in skeletons)
				{
					bones.AddRange(sk.BoneData);
					transforms.AddRange(sk.BoneLocalMatrices);
				}


				skeleton = new SkeletonData(transforms.ToArray(), bones.ToArray());


				boneIndices = new Dictionary<string, int>();

				foreach (BoneData bone in skeleton.BoneData)
					if (bone.Name != null && boneIndices.ContainsKey(bone.Name) == false)
						boneIndices.Add(bone.Name, bone.Index);
			}

			if (skeleton != null)
				ProcessAnimations(input, context, animations, boneIndices, skeleton);

			int geometryCount = 0;
			ProcessMeshNodes(input, context, meshes, rootPath, processedContent, skeleton, animations.ToArray(), ref geometryCount);

			return new ModelData(input.Name, meshes.ToArray(), skeleton, animations.ToArray());
		}

		void ProcessSkeletonNodes(NodeContent node, ContentProcessorContext context, List<SkeletonData> skeletons)
		{
			if (node is BoneContent)
			{
				skeletons.Add(ProcessSkeleton(node as BoneContent, context));
				return;
			}

			foreach (NodeContent child in node.Children)
			{
				ProcessSkeletonNodes(child, context, skeletons);
			}
		}

		void ProcessMeshNodes(NodeContent node, ContentProcessorContext context, List<MeshData> meshes, string rootPath, Dictionary<string, object> processedContent, SkeletonData skeleton, AnimationData[] animations, ref int geometryCount)
		{
			if (node is BoneContent && !importBoneContentMehses)
				return;
			if (node is MeshContent)
			{
				meshes.Add(ProcessMesh(node as MeshContent, context, rootPath, processedContent, skeleton, animations, ref geometryCount));
				return;
			}

			foreach (NodeContent child in node.Children)
			{
				ProcessMeshNodes(child, context, meshes, rootPath, processedContent, skeleton, animations, ref geometryCount);
			}
		}

		MeshData ProcessMesh(MeshContent mesh, ContentProcessorContext context, string rootPath, Dictionary<string, object> processedContent, SkeletonData skeletonData, AnimationData[] animations, ref int geometryCount)
		{
			MeshHelper.TransformScene(mesh, mesh.AbsoluteTransform);

			string[] normalMapNames = new string[] { "Bump0", "Bump", "NormalMap", "Normalmap", "Normals", "BumpMap" };
			MeshHelper.OptimizeForCache(mesh);

			foreach (GeometryContent geom in mesh.Geometry)
			{
				if (geom.Material != null)
				{
					string map = MaterialTexture(geom.Material, rootPath, null, null, true, normalMapNames);
					if (map != null && map.Length > 0)
						generateTangents = true;
				}
			}

			if (generateTangents)
			{
				MeshHelper.CalculateNormals(mesh, false);

				bool hasNoTangent = !GeometryContainsChannel(mesh, VertexChannelNames.Tangent(0));
				bool hasNoBinorm = !GeometryContainsChannel(mesh, VertexChannelNames.Binormal(0));
				if (hasNoTangent || hasNoBinorm)
				{
					string tangentChannelName = hasNoTangent ? VertexChannelNames.Tangent(0) : null;
					string binormalChannelName = hasNoBinorm ? VertexChannelNames.Binormal(0) : null;
					MeshHelper.CalculateTangentFrames(mesh, VertexChannelNames.TextureCoordinate(0), tangentChannelName, binormalChannelName);
				}
			}
			if (swapWindingOrder)
			{
				MeshHelper.SwapWindingOrder(mesh);
			}

			List<GeometryData> geometry = new List<GeometryData>();

			BoneContent skeleton = MeshHelper.FindSkeleton(mesh);
			Dictionary<string, int> boneIndices = null;
			if (skeleton != null)
				boneIndices = FlattenSkeleton(skeleton);

			foreach (GeometryContent geom in mesh.Geometry)
			{
				uint[] geometryBoneRemapping;
				int geometryBoneSingleIndex;

				this.ProcessVertexChannels(geom, context, rootPath, boneIndices, null, out geometryBoneRemapping, out geometryBoneSingleIndex);
				MeshHelper.MergeDuplicateVertices(geom);
				
				MaterialData material = new MaterialData(
					MaterialValue<float>("Alpha", geom.Material, 1),
					MaterialValue<float>("SpecularPower", geom.Material, 24),
					MaterialValue<Vector3>("DiffuseColor", geom.Material, Vector3.One),
					MaterialValue<Vector3>("EmissiveColor", geom.Material, Vector3.Zero),
					MaterialValue<Vector3>("SpecularColor", geom.Material, Vector3.Zero),
					MaterialTexture(geom.Material, rootPath, context, processedContent, false, "Texture"),
					MaterialTexture(geom.Material, rootPath, context, processedContent, true, normalMapNames),
					MaterialValue<bool>("VertexColorEnabled", geom.Material, true) && geom.Vertices.Channels.Contains(VertexChannelNames.Color(0)));

				VertexBufferContent vb;
				VertexElement[] ve;
				vb = geom.Vertices.CreateVertexBuffer();//out vb, out ve, context.TargetPlatform
				ve = new List<VertexElement>(vb.VertexDeclaration.VertexElements).ToArray();
				
				int[] indices = new int[geom.Indices.Count];
				geom.Indices.CopyTo(indices, 0);

				geometry.Add(new GeometryData(geometryCount++, geom.Name, ve, vb, vb.VertexData, indices, material, skeletonData, animations, context.TargetPlatform == TargetPlatform.Xbox360, geometryBoneRemapping, geometryBoneSingleIndex));
			}

			return new MeshData(mesh.Name, geometry.ToArray(), animations);
		}

		string MaterialTexture(MaterialContent material, string rootPath, ContentProcessorContext context, Dictionary<string, object> processedContent, bool normalMap, params string[] names)
		{
			ExternalReference<TextureContent> tex;
			foreach (string name in names)
			{
				if (material != null && material.Textures.TryGetValue(name, out tex))
				{
					if (tex != null && tex.Filename != null)
					{
						if (File.Exists(tex.Filename))
						{
							string filename = tex.Filename;
							if (context != null && importTextures)
							{
								ExternalReference<TextureContent> content = null;
								object content_obj = null;
								if (!processedContent.TryGetValue(tex.Filename, out content_obj))
								{
									OpaqueDataDictionary processorParameters = new OpaqueDataDictionary();
									processorParameters["ColorKeyEnabled"] = false;
									processorParameters["TextureFormat"] = this.TextureFormat;
									processorParameters["GenerateMipmaps"] = this.GenerateMipmaps;
									processorParameters["ResizeToPowerOfTwo"] = this.ResizeTexturesToPowerOfTwo;
									processorParameters["PremultiplyAlpha"] = !normalMap;

									content = context.BuildAsset<TextureContent, TextureContent>(tex, typeof(TextureProcessor).Name, processorParameters, null, null);
									processedContent.Add(tex.Filename, content);
								}
								else
									content = (ExternalReference<TextureContent>)content_obj;

								filename = new FileInfo(tex.Filename).Directory + @"\" + new FileInfo(content.Filename).Name;
							}
							return GetSharedPath(filename, rootPath);
						}
						else
							if (context != null)
								context.Logger.LogWarning(null, new ContentIdentity(rootPath), "File not found:\t{1} ({0})", name, @".\" + GetSharedPath(tex.Filename, rootPath) + new FileInfo(tex.Filename).Extension);
						break;
					}
				}
			}
			return "";
		}

		static string GetSharedPath(string filePath, string rootPath)
		{
			FileInfo file = new FileInfo(filePath);
			FileInfo root = new FileInfo(rootPath);

			DirectoryInfo fileDir = file.Directory;
			DirectoryInfo rootDir = root.Directory;
			int subDirs = 0;

			while (fileDir.Parent != null && rootDir.Parent != null)
			{
				if (fileDir.FullName == rootDir.FullName)
				{
					string path = Path.ChangeExtension(file.FullName.Substring(fileDir.FullName.Length), null);
					while (subDirs-- > 0)
					{
						if (subDirs != 0)
							path = @"\" + path;
						path = @".." + path;
					}
					if (path.StartsWith(@"\"))
						path = path.Substring(1);
					return path;
				}
				if (fileDir.Parent == null)
				{
					rootDir = rootDir.Parent;
					subDirs++;
					continue;
				}
				if (rootDir.Parent == null)
				{
					fileDir = fileDir.Parent;
					continue;
				}
				if (fileDir.FullName.Length > rootDir.FullName.Length)
				{
					fileDir = fileDir.Parent;
				}
				else
				{
					rootDir = rootDir.Parent;
					subDirs++;
				}
			}
			return filePath;
		}

		T MaterialValue<T>(string name, MaterialContent material, T defaultValue)
		{
			object obj;
			if (material != null && material.OpaqueData.TryGetValue(name, out obj))
			{
				if (obj is T)
					return (T)obj;
			}
			return defaultValue;
		}


		void ProcessAnimations(NodeContent node, ContentProcessorContext context, List<AnimationData> animations, Dictionary<string, int> indices, SkeletonData skeleton)
		{
			foreach (AnimationContent anim in node.Animations.Values)
				ProcessAnimation(anim, context, animations, indices, skeleton);

			foreach (NodeContent child in node.Children)
				ProcessAnimations(child, context, animations, indices, skeleton);
		}

		void ProcessAnimation(AnimationContent anim, ContentProcessorContext context, List<AnimationData> animations, Dictionary<string, int> indices, SkeletonData skeleton)
		{
			SortedDictionary<TimeSpan, bool> allFrameTimes = new SortedDictionary<TimeSpan, bool>();
			SortedDictionary<TimeSpan, Matrix[]> transforms = new SortedDictionary<TimeSpan, Matrix[]>();
			int totalChannels = 0;

			foreach (KeyValuePair<string, AnimationChannel> channelKVP in anim.Channels)
			{
				if (indices.ContainsKey(channelKVP.Key) == false)
					continue;

				AnimationChannel channel = channelKVP.Value;

				foreach (AnimationKeyframe frame in channel)
				{
					if (allFrameTimes.ContainsKey(frame.Time) == false)
						allFrameTimes.Add(frame.Time, true);
				}
				totalChannels++;
			}

			foreach (TimeSpan time in allFrameTimes.Keys)
			{
				transforms.Add(time, new Matrix[totalChannels]);
			}

			SortedDictionary<TimeSpan, Matrix> keyFrames = new SortedDictionary<TimeSpan, Matrix>();
			List<KeyValuePair<TimeSpan, Matrix>> newFrames = new List<KeyValuePair<TimeSpan, Matrix>>();

			int index = 0;

			foreach (KeyValuePair<string, AnimationChannel> channelKVP in anim.Channels)
			{
				if (indices.ContainsKey(channelKVP.Key) == false)
					continue;

				AnimationChannel channel = channelKVP.Value;

				foreach (AnimationKeyframe frame in channel)
				{
					keyFrames.Add(frame.Time, frame.Transform);
				}

				SortedDictionary<TimeSpan, Matrix>.Enumerator frames = keyFrames.GetEnumerator();
				SortedDictionary<TimeSpan, bool>.Enumerator times = allFrameTimes.GetEnumerator();

				if (!times.MoveNext())
					continue;
				if (!frames.MoveNext())
					continue;
				TimeSpan time = frames.Current.Key;
				Matrix transform = frames.Current.Value;

				while (true)
				{
					Matrix previousTransform = transform;
					TimeSpan previousTime = time;

					time = frames.Current.Key;
					transform = frames.Current.Value;

					if (times.Current.Key.Ticks == frames.Current.Key.Ticks)
					{
						if (!times.MoveNext())
							break;

						if (!frames.MoveNext())
						{
							//frames ends early...
							while (true)
							{
								newFrames.Add(new KeyValuePair<TimeSpan, Matrix>(times.Current.Key, transform));

								if (!times.MoveNext())
									break;
							}
							break;
						}
						continue;
					}

					if (times.Current.Key.Ticks > frames.Current.Key.Ticks)
					{
						//frame is behind
						if (!frames.MoveNext())
						{
							//frames ends early...
							while (true)
							{
								newFrames.Add(new KeyValuePair<TimeSpan, Matrix>(times.Current.Key, transform));

								if (!times.MoveNext())
									break;
							}
							break;
						}
						continue;
					}
					else
					{
						//frame is ahead.. create an inbetween

						double amount = (times.Current.Key - previousTime).TotalSeconds / (frames.Current.Key - previousTime).TotalSeconds;
						Matrix newTransform = Matrix.Lerp(previousTransform, frames.Current.Value, (float)amount);
						//Matrix newTransform = frames.Current.Value.Interpolate(ref previousTransform, (float)(1 - amount));
						newFrames.Add(new KeyValuePair<TimeSpan, Matrix>(times.Current.Key, newTransform));

						transform = newTransform;
						time = times.Current.Key;

						if (!times.MoveNext())
							break;
					}
				}


				foreach (KeyValuePair<TimeSpan, Matrix> newFrame in newFrames)
				{
					keyFrames.Add(newFrame.Key, newFrame.Value);
				}

				foreach (KeyValuePair<TimeSpan, Matrix> kvp in keyFrames)
				{
					Matrix[] array;
					if (transforms.TryGetValue(kvp.Key, out array))
						array[index] = kvp.Value;
					else
						continue;
				}

				newFrames.Clear();
				keyFrames.Clear();

				index++;
			}

			KeyFrameData[] boneKeyFrames = new KeyFrameData[transforms.Count];
			int[] boneIndices = new int[totalChannels];
			index = 0;


			foreach (KeyValuePair<string, AnimationChannel> channelKVP in anim.Channels)
			{
				if (indices.ContainsKey(channelKVP.Key) == false)
					continue;
				boneIndices[index++] = indices[channelKVP.Key];
			}

			index = 0;
			float duration = 0;
			foreach (KeyValuePair<TimeSpan, Matrix[]> kvp in transforms)
			{
				float seconds = (float)kvp.Key.TotalSeconds;

				for (int i = 0; i < kvp.Value.Length; i++)
					kvp.Value[i] = Matrix.Invert(skeleton.BoneLocalMatrices[boneIndices[i]]) * kvp.Value[i];

				try
				{
					boneKeyFrames[index++] = new KeyFrameData(seconds, kvp.Value);
				}
				catch (ArgumentException ex)
				{
					throw new InvalidContentException("Error generating bone transform: " + ex.Message);
				}

				duration = Math.Max(seconds, duration);
			}

			if (boneIndices.Length > 0 && boneKeyFrames.Length > 0)
			{
				AnimationData animation = new AnimationData(anim.Name, boneIndices, boneKeyFrames, duration, animationCompressionTolerancePercent);

				animations.Add(animation);
			}
		}

		SkeletonData ProcessSkeleton(BoneContent root, ContentProcessorContext context)
		{
			Dictionary<string, int> bones = FlattenSkeleton(root);

			int boneCount = 0;
			foreach (int index in bones.Values)
				boneCount = Math.Max(boneCount, index + 1);

			BoneData[] boneData = new BoneData[boneCount];
			Matrix[] boneTransforms = new Matrix[boneCount];
			BuildBoneHirachy(root, -1, null, bones, boneData, boneTransforms);


			SkeletonData skeleton = new SkeletonData(boneTransforms, boneData);

			return skeleton;
		}

		void BuildBoneHirachy(BoneContent bone, int boneIndex, List<BoneData> structure, Dictionary<string, int> indices, BoneData[] allBones, Matrix[] transforms)
		{
			if (bone.Name != null && indices.ContainsKey(bone.Name))
			{
				List<BoneData> children = new List<BoneData>();

				foreach (NodeContent child in bone.Children)
				{
					if (child is BoneContent)
						BuildBoneHirachy((BoneContent)child, indices[bone.Name], children, indices, allBones, transforms);
				}

				int[] childIndices = new int[children.Count];
				for (int i = 0; i < children.Count; i++)
					childIndices[i] = indices[children[i].Name];

				int index = indices[bone.Name];
				BoneData data = new BoneData(bone.Name, index, boneIndex, childIndices);
				if (structure != null)
					structure.Add(data);
				allBones[index] = data;
				transforms[index] = bone.Transform;
			}
		}

		#region XNA mesh hacks

		//funnily enough, Weights and colours screw up VertexContent.CreateVertexBuffer()
		//so the normal XNA model importer hacks around this... which is wonderful.
		//this is an exact copy of the hack
		private void ProcessVertexChannels(GeometryContent geometry, ContentProcessorContext context, string asset, Dictionary<string, int> boneIndices, Dictionary<int, int> boneRemap, out uint[] geometryBoneRemapping, out int geometryBoneSingleIndex)
		{
			geometryBoneRemapping = null;
			geometryBoneSingleIndex = -1;

			VertexChannelCollection collection = geometry.Vertices.Channels;
			List<VertexChannel> list = new List<VertexChannel>(collection);
			int vertexChannelIndex = 0;
			foreach (VertexChannel channel in list)
			{
				if (((vertexChannelIndex < 0) || (vertexChannelIndex >= collection.Count)) || (collection[vertexChannelIndex] != channel))
				{
					vertexChannelIndex = collection.IndexOf(channel);
					if (vertexChannelIndex < 0)
					{
						continue;
					}
				}

				int geometryBoneSingleIndexTemp;
				uint[] geometryBoneRemappingTemp;

				this.ProcessVertexChannel(geometry, vertexChannelIndex, context, asset, boneIndices, boneRemap, out geometryBoneRemappingTemp, out geometryBoneSingleIndexTemp);

				if (geometryBoneRemappingTemp != null) geometryBoneRemapping = geometryBoneRemappingTemp;
				if (geometryBoneSingleIndexTemp != -1) geometryBoneSingleIndex = geometryBoneSingleIndexTemp;

				vertexChannelIndex++;
			}
		}
		private bool GeometryContainsChannel(MeshContent mesh, string channel)
		{
			foreach (GeometryContent content in mesh.Geometry)
				if (content.Vertices.Channels.Contains(channel))
					return true;
			return false;
		}
		//returns bone remapping if needed
		private void ProcessVertexChannel(GeometryContent geometry, int vertexChannelIndex, ContentProcessorContext context, string asset, Dictionary<string, int> boneIndices, Dictionary<int, int> boneRemap, out uint[] boneRemapping, out int geometryBoneSingleIndex)
		{
			boneRemapping = null;
			geometryBoneSingleIndex = -1;

			string str = VertexChannelNames.DecodeBaseName(geometry.Vertices.Channels[vertexChannelIndex].Name);
			if (str != null)
			{
				if (str == "Color")
					ProcessColorChannel(geometry, vertexChannelIndex);
				if (str == "Weights")
					ProcessWeightsChannel(context, asset, geometry, vertexChannelIndex, boneIndices, boneRemap, out boneRemapping, out geometryBoneSingleIndex);
			}
		}
		private static void ProcessColorChannel(GeometryContent geometry, int vertexChannelIndex)
		{
			VertexChannelCollection channels = geometry.Vertices.Channels;
			try
			{
				channels.ConvertChannelContent<Color>(vertexChannelIndex);
			}
			catch (NotSupportedException)
			{
				throw new InvalidContentException("Unable to convert mesh embedded colour channel to Vector4");
			}
		}
		private int[] ComputeBoneSet(GeometryContent geometry, ContentProcessorContext context, string asset, Dictionary<string, int> boneIndices)
		{
			SortedDictionary<int, bool> indicesInUse = new SortedDictionary<int, bool>();
			foreach (VertexChannel vc in geometry.Vertices.Channels)
			{
				string str = VertexChannelNames.DecodeBaseName(vc.Name);
				if (str == "Weights")
				{
					VertexChannel<BoneWeightCollection> channel = vc as VertexChannel<BoneWeightCollection>;
					if (vc == null)
						continue;
					for (int n = 0; n < channel.Count; n++)
						VertexWeightsInUse(context, asset, channel[n], boneIndices, indicesInUse);
				}
			}
			int[] values = new int[indicesInUse.Count];
			int i = 0;
			foreach (int index in indicesInUse.Keys)
				values[i++] = index;
			return values;
		}
		private static void ProcessWeightsChannel(ContentProcessorContext context, string asset, GeometryContent geometry, int vertexChannelIndex, Dictionary<string, int> boneIndices, Dictionary<int, int> boneRemap, out uint[] geometryBoneRemapping, out int geometryBoneSingleIndex)
		{
			geometryBoneSingleIndex = -1;
			geometryBoneRemapping = null;

			if (boneIndices == null)
				throw new InvalidContentException("Mesh has bone weights with no skeleton");

			const int MaxBones = 72;

			VertexChannelCollection channels = geometry.Vertices.Channels;
			VertexChannel channel2 = channels[vertexChannelIndex];
			VertexChannel<BoneWeightCollection> channel = channel2 as VertexChannel<BoneWeightCollection>;
			Int4[] outputIndices = new Int4[channel.Count];
			Vector4[] outputWeights = new Vector4[channel.Count];
			for (int i = 0; i < channel.Count; i++)
			{
				BoneWeightCollection inputWeights = channel[i];
				ConvertVertexWeights(context, asset, inputWeights, boneIndices, outputIndices, outputWeights, i, geometry, boneRemap);
			}

			uint maxBoneIndex = 0;
			for (int i = 0; i < outputIndices.Length; i++)
			{
				maxBoneIndex = Math.Max(maxBoneIndex, outputIndices[i].X + 1);
				maxBoneIndex = Math.Max(maxBoneIndex, outputIndices[i].Y + 1);
				maxBoneIndex = Math.Max(maxBoneIndex, outputIndices[i].Z + 1);
				maxBoneIndex = Math.Max(maxBoneIndex, outputIndices[i].W + 1);
			}

			//see if this geometry is using less bones
			bool[] boneInUse = new bool[maxBoneIndex];

			for (int i = 0; i < outputIndices.Length; i++)
			{
				if (outputWeights[i].X != 0) boneInUse[outputIndices[i].X] = true;
				if (outputWeights[i].Y != 0) boneInUse[outputIndices[i].Y] = true;
				if (outputWeights[i].Z != 0) boneInUse[outputIndices[i].W] = true;
				if (outputWeights[i].W != 0) boneInUse[outputIndices[i].Z] = true;
			}

			uint inUseIndex = 0;
			uint boneInUseCount = 0;
			for (uint i = 0; i < maxBoneIndex; i++)
			{
				if (boneInUse[i])
				{
					inUseIndex = i;
					boneInUseCount++;
				}
			}

			if (boneInUseCount == 1)
			{
				//this geometry doesn't even need to use bones.
				//note, the bone data needs to stay, because this optimisation only works for IShader renderering
				geometryBoneSingleIndex = (int)inUseIndex;
			}

			if (boneIndices.Count > MaxBones)
			{
				//too many bones are in this model, this geometry needs to remap them to fit in the runtime minimum

				if (boneInUseCount > 72)
					throw new InvalidContentException("Geometry within a model cannot use more than 72 unique bones");

				geometryBoneRemapping = new uint[boneInUseCount];
				uint[] vertexRemap = new uint[boneIndices.Count];
				
				boneInUseCount = 0;
				for (uint i = 0; i < maxBoneIndex; i++)
				{
					if (boneInUse[i])
					{
						vertexRemap[i] = boneInUseCount;
						geometryBoneRemapping[boneInUseCount++] = i;
					}
				}

				for (int i = 0; i < outputIndices.Length; i++)
				{
					outputIndices[i].X = vertexRemap[outputIndices[i].X];
					outputIndices[i].Y = vertexRemap[outputIndices[i].Y];
					outputIndices[i].Z = vertexRemap[outputIndices[i].Z];
					outputIndices[i].W = vertexRemap[outputIndices[i].W];
				}
			}

			Byte4[] quantizedIndices = new Byte4[outputIndices.Length];
			for (int i = 0; i < quantizedIndices.Length; i++)
			{
				quantizedIndices[i].PackedValue =
					(outputIndices[i].X << 0) |
					(outputIndices[i].Y << 8) |
					(outputIndices[i].Z << 16) |
					(outputIndices[i].W << 24);
			}

			int usageIndex = VertexChannelNames.DecodeUsageIndex(channel.Name);
			string name = VertexChannelNames.EncodeName(VertexElementUsage.BlendIndices, usageIndex);
			string str = VertexChannelNames.EncodeName(VertexElementUsage.BlendWeight, usageIndex);
			channels.Insert<Byte4>(vertexChannelIndex + 1, name, quantizedIndices);
			channels.Insert<Vector4>(vertexChannelIndex + 2, str, outputWeights);
			channels.RemoveAt(vertexChannelIndex);
		}

		static int[] tempIndices = new int[4];
		static float[] tempWeights = new float[4];
		struct Int4
		{
			public Int4(int x, int y, int z, int w) { X = (uint)x; Y = (uint)y; Z = (uint)z; W = (uint)w; }
			public uint X, Y, Z, W;
		}

		private static void ConvertVertexWeights(ContentProcessorContext context, string asset, BoneWeightCollection inputWeights, Dictionary<string, int> boneIndices, Int4[] outputIndices, Vector4[] outputWeights, int vertexIndex, GeometryContent geometry, Dictionary<int, int> boneRemap)
		{
			inputWeights.NormalizeWeights(4);
			for (int i = 0; i < inputWeights.Count; i++)
			{
				BoneWeight weight = inputWeights[i];
				if (!boneIndices.TryGetValue(weight.BoneName, out tempIndices[i]))
				{
					context.Logger.LogWarning(null, new ContentIdentity(asset), "Unknown bone name: " + weight.BoneName);
					//throw new InvalidContentException("Unknown bone name: " + weight.BoneName);
					continue;
				}
				tempWeights[i] = weight.Weight;
				if (boneRemap != null)
					tempIndices[i] = boneRemap[tempIndices[i]];
			}
			for (int j = inputWeights.Count; j < 4; j++)
			{
				tempIndices[j] = 0;
				tempWeights[j] = 0f;
			}
			outputIndices[vertexIndex] = new Int4(tempIndices[0], tempIndices[1], tempIndices[2], tempIndices[3]);
			outputWeights[vertexIndex] = new Vector4(tempWeights[0], tempWeights[1], tempWeights[2], tempWeights[3]);
		}

		private static void VertexWeightsInUse(ContentProcessorContext context, string asset, BoneWeightCollection inputWeights, Dictionary<string, int> boneIndices, SortedDictionary<int, bool> indicesInUse)
		{
			if (boneIndices == null)
				throw new InvalidContentException("Mesh has bone weights with no skeleton");

			int index;
			for (int i = 0; i < inputWeights.Count; i++)
			{
				BoneWeight weight = inputWeights[i];
				if (!boneIndices.TryGetValue(weight.BoneName, out index))
				{
					context.Logger.LogWarning(null, new ContentIdentity(asset), "Unknown bone name: " + weight.BoneName);
					//throw new InvalidContentException("Unknown bone name: " + weight.BoneName);
					continue;
				}
				if (indicesInUse.ContainsKey(index) == false)
					indicesInUse.Add(index, true);
			}
		}

		private static Dictionary<string, int> FlattenSkeleton(BoneContent skeleton)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			IList<BoneContent> list = MeshHelper.FlattenSkeleton(skeleton);
			int index = 0;
			for (int i = 0; i < list.Count; i++)
			{
				BoneContent content = list[i];
				if (!string.IsNullOrEmpty(content.Name))
				{
					if (dictionary.ContainsKey(content.Name))
						throw new InvalidContentException("Duplicate bone found: " + content.Name);
					dictionary.Add(content.Name, index++);
				}
			}
			return dictionary;
		}

		#endregion


		#region Processor params
		bool generateTangents = false;
		TextureProcessorOutputFormat destinationFormat = TextureProcessorOutputFormat.DxtCompressed;
		bool importTextures = true, generateMipmaps = true, resizeTexturesToPowerOfTwo = true, swapWindingOrder = false, importAnimations = true, importBoneContentMehses;
		float scale = 1, rotationX = 0, rotationY = 0, rotationZ = 0, animationCompressionTolerancePercent = 0.25f;

		[DefaultValue(0.25f), DisplayName("Animation Compression Tolerance (Percent)"), Category("Animation")]
		public virtual float AnimationCompressionTolerancePercent
		{
			get
			{
				return this.animationCompressionTolerancePercent;
			}
			set
			{
				this.animationCompressionTolerancePercent = value;
			}
		}

		[DefaultValue(true), DisplayName("Import animations"), Category("Animation")]
		public virtual bool ImportAnimations
		{
			get
			{
				return this.importAnimations;
			}
			set
			{
				this.importAnimations = value;
			}
		}

		[DefaultValue(false), DisplayName("Import Meshes from BoneContent"), Category("Geometry")]
		public virtual bool ImportBoneContentMeshes
		{
			get
			{
				return this.importBoneContentMehses;
			}
			set
			{
				this.importBoneContentMehses = value;
			}
		}

		[DefaultValue(false), DisplayName("Manual texture import"), Category("Textures")]
		public virtual bool ManualTextureImport
		{
			get
			{
				return !this.importTextures;
			}
			set
			{
				this.importTextures = !value;
			}
		}


		[DefaultValue(TextureProcessorOutputFormat.DxtCompressed), DisplayName("Texture format"), Category("Textures")]
		public virtual TextureProcessorOutputFormat TextureFormat
		{
			get
			{
				return this.destinationFormat;
			}
			set
			{
				this.destinationFormat = value;
			}
		}



		[DefaultValue(true), DisplayName("Generate mipmaps"), Category("Textures")]
		public virtual bool GenerateMipmaps
		{
			get
			{
				return this.generateMipmaps;
			}
			set
			{
				this.generateMipmaps = value;
			}
		}

		[DefaultValue(false), DisplayName("Generate tangent frames"), Category("Geometry")]
		public virtual bool GenerateTangentFrames
		{
			get
			{
				return this.generateTangents;
			}
			set
			{
				this.generateTangents = value;
			}
		}

		[DefaultValue(true), DisplayName("Resize textures to power of two"), Category("Textures")]
		public virtual bool ResizeTexturesToPowerOfTwo
		{
			get
			{
				return this.resizeTexturesToPowerOfTwo;
			}
			set
			{
				this.resizeTexturesToPowerOfTwo = value;
			}
		}

		[DefaultValue(0), DisplayName("Rotation X"), Category("Geometry")]
		public virtual float RotationX
		{
			get
			{
				return this.rotationX;
			}
			set
			{
				this.rotationX = value;
			}
		}

		[DefaultValue(0), DisplayName("Rotation Y"), Category("Geometry")]
		public virtual float RotationY
		{
			get
			{
				return this.rotationY;
			}
			set
			{
				this.rotationY = value;
			}
		}


		[DefaultValue(0), DisplayName("Rotation Z"), Category("Geometry")]
		public virtual float RotationZ
		{
			get
			{
				return this.rotationZ;
			}
			set
			{
				this.rotationZ = value;
			}
		}

		[DefaultValue(1), DisplayName("Scale"), Category("Geometry")]
		public virtual float Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		[DefaultValue(false), DisplayName("Swap winding order"), Category("Geometry")]
		public virtual bool SwapWindingOrder
		{
			get
			{
				return this.swapWindingOrder;
			}
			set
			{
				this.swapWindingOrder = value;
			}
		}

		#endregion
	}

}