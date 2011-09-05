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

	//This code is largely identical to ModelImporter.
	//There isn't any easy way to get around a lot of duplicate code here...

	//the mesh is still processed, but most of it gets thrown away.

	[ContentTypeWriter]
	class AvatarDataWriter : ContentTypeWriter<AvatarAnimationData>
	{
		protected override void Write(ContentWriter output, AvatarAnimationData value)
		{
			typeof(AvatarAnimationData).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(value, new object[] { output });
		}

		public override string GetRuntimeReader(TargetPlatform targetPlatform)
		{
			return (string)typeof(AvatarAnimationData).GetProperty("RuntimeReaderType", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, new object[0]);
		}
	}




	[ContentProcessor(DisplayName = "Avatar Animations - Xen")]
	public class AvatarImporter : ContentProcessor<NodeContent, AvatarAnimationData>
	{
		public override AvatarAnimationData Process(NodeContent input, ContentProcessorContext context)
		{
			List<MeshData> meshes = new List<MeshData>();
			SkeletonData skeleton = null;
			List<AnimationData> animations = new List<AnimationData>();

			List<SkeletonData> skeletons = new List<SkeletonData>();


			string rootPath = input.Identity.SourceFilename;

			Dictionary<string, object> processedContent = new Dictionary<string, object>();


			ProcessSkeletonNodes(input, context, skeletons);

			//there shouldn't be more than one skeleton... but anyway...

			if (skeletons.Count != 1 || skeletons[0].BoneCount < Microsoft.Xna.Framework.GamerServices.AvatarRenderer.BoneCount)
			{
				throw new InvalidDataException(string.Format(@"Avatar Animation Model Data must contain a skeleton with {0} bones",Microsoft.Xna.Framework.GamerServices.AvatarRenderer.BoneCount));
			}

			Dictionary<string, int> boneIndices = null;

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

			ProcessAnimations(input, context, animations, boneIndices, skeleton);

			int geometryCount = 0;
			ProcessMeshNodes(input, context, meshes, rootPath, processedContent, skeleton, animations.ToArray(), ref geometryCount);

			return new AvatarAnimationData(input.Name, meshes.ToArray(), skeleton, animations.ToArray());
		}

		void ProcessSkeletonNodes(NodeContent node, ContentProcessorContext context, List<SkeletonData> skeletons)
		{
			if (node is BoneContent)
			{
				RemoveEndBonesAndFixBoneNames(node);

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
			if (node is BoneContent)
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

			List<GeometryData> geometry = new List<GeometryData>();

			BoneContent skeleton = MeshHelper.FindSkeleton(mesh);
			Dictionary<string, int> boneIndices = null;
			if (skeleton != null)
				boneIndices = FlattenSkeleton(skeleton);

			foreach (GeometryContent geom in mesh.Geometry)
			{
				this.ProcessVertexChannels(geom, context, rootPath, boneIndices, null);
				MeshHelper.MergeDuplicateVertices(geom);

				VertexBufferContent vb;
				VertexElement[] ve;
				vb = geom.Vertices.CreateVertexBuffer();
				ve = new List<VertexElement>(vb.VertexDeclaration.VertexElements).ToArray();

				int[] indices = new int[geom.Indices.Count];
				geom.Indices.CopyTo(indices, 0);

				geometry.Add(new GeometryData(geometryCount++, geom.Name, ve, vb, vb.VertexData, indices, new MaterialData(), skeletonData, animations, context.TargetPlatform == TargetPlatform.Xbox360, null, -1));
			}

			return new MeshData(mesh.Name, geometry.ToArray(), animations);
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
				if (indices.ContainsKey(CleanBoneName(channelKVP.Key)) == false)
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
				int boneIndex = 0;
				if (indices.TryGetValue(CleanBoneName(channelKVP.Key), out boneIndex) == false)
					continue;

				AnimationChannel channel = channelKVP.Value;
				Matrix transform;

				foreach (AnimationKeyframe frame in channel)
				{
					transform = frame.Transform;

					if (boneIndex == 0)
					{
						CorrectRootBoneTransform(ref transform, skeleton);
					}
					else
					{
						//remove translation
						transform.Translation = new Vector3();
					}

					keyFrames.Add(frame.Time, transform);
				}

				SortedDictionary<TimeSpan, Matrix>.Enumerator frames = keyFrames.GetEnumerator();
				SortedDictionary<TimeSpan, bool>.Enumerator times = allFrameTimes.GetEnumerator();

				if (!times.MoveNext())
					continue;
				if (!frames.MoveNext())
					continue;
				TimeSpan time = frames.Current.Key;
				transform = frames.Current.Value;

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
				if (indices.ContainsKey(CleanBoneName(channelKVP.Key)) == false)
					continue;
				boneIndices[index++] = indices[CleanBoneName(channelKVP.Key)];
			}

			//compute world space bone default skeleton
			Matrix[] worldBoneTransforms = skeleton.BoneLocalMatrices.ToArray();
			skeleton.TransformHierarchy(worldBoneTransforms);

			index = 0;
			float duration = 0;
			foreach (KeyValuePair<TimeSpan, Matrix[]> kvp in transforms)
			{
				float seconds = (float)kvp.Key.TotalSeconds;
				/*
				//make transforms realtive to skeleton
				//first get static bone transforms
				Matrix[] worldTransform = new Matrix[skeleton.BoneCount];
				for (int i = 0; i < worldTransform.Length; i++)
					worldTransform[i] = skeleton.BoneLocalMatrices[i];

				//replace the animated bones..
				for (int i = 0; i < kvp.Value.Length; i++)
					worldTransform[boneIndices[i]] = kvp.Value[i];

				//transform into a world space skeleton
				skeleton.TransformHierarchy(worldTransform);

				//multiply the world space transforms with the inverse of the world space static skeleton
				for (int i = 0; i < worldTransform.Length; i++)
				{
					Matrix m = worldBoneTransforms[i];
					Matrix.Invert(ref m, out m);

					worldTransform[i] = m * worldTransform[i];
				}

				//transform back out of world space into joint space
				skeleton.TransformHierarchyInverse(worldTransform);


				for (int i = 0; i < kvp.Value.Length; i++)
					kvp.Value[i] = worldTransform[boneIndices[i]];
				*/
				boneKeyFrames[index++] = new KeyFrameData(seconds, kvp.Value);
				duration = Math.Max(seconds, duration);
			}

			if (boneIndices.Length > 0 && boneKeyFrames.Length > 0)
			{
				AnimationData animation = new AnimationData(anim.Name, boneIndices, boneKeyFrames, duration, animationCompressionTolerancePercent);

				animations.Add(animation);
			}
		}

		private void CorrectRootBoneTransform(ref Matrix transform, SkeletonData skeleton)
		{
			//this is taken from the XNA sample...

			// When the animation is exported the bind pose can have the 
			// wrong translation of the root node so we hard code it here

			Matrix keyTransfrom = transform;

			Matrix inverseBindPose = skeleton.BoneLocalMatrices[0];
			inverseBindPose.Translation -= bindPoseTranslation;
			inverseBindPose = Matrix.Invert(inverseBindPose);

			Matrix keyframeMatrix = (keyTransfrom * inverseBindPose);
			keyframeMatrix.Translation -= bindPoseTranslation;

			// Scale from cm to meters
			keyframeMatrix.Translation *= 0.01f;

			transform = keyframeMatrix;
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
		private void ProcessVertexChannels(GeometryContent geometry, ContentProcessorContext context, string asset, Dictionary<string, int> boneIndices, Dictionary<int, int> boneRemap)
		{
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
				this.ProcessVertexChannel(geometry, vertexChannelIndex, context, asset, boneIndices, boneRemap);
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
		private void ProcessVertexChannel(GeometryContent geometry, int vertexChannelIndex, ContentProcessorContext context, string asset, Dictionary<string, int> boneIndices, Dictionary<int, int> boneRemap)
		{
			string str = VertexChannelNames.DecodeBaseName(geometry.Vertices.Channels[vertexChannelIndex].Name);
			if (str != null)
			{
				if (str == "Color")
					ProcessColorChannel(geometry, vertexChannelIndex);
				if (str == "Weights")
					ProcessWeightsChannel(context, asset, geometry, vertexChannelIndex, boneIndices, boneRemap);
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
				throw new InvalidCastException("Unable to convert mesh embedded colour channel to Vector4");
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
		private static void ProcessWeightsChannel(ContentProcessorContext context, string asset, GeometryContent geometry, int vertexChannelIndex, Dictionary<string, int> boneIndices, Dictionary<int, int> boneRemap)
		{
			if (boneIndices == null)
				throw new InvalidContentException("Mesh has bone weights with no skeleton");

			VertexChannelCollection channels = geometry.Vertices.Channels;
			VertexChannel channel2 = channels[vertexChannelIndex];
			VertexChannel<BoneWeightCollection> channel = channel2 as VertexChannel<BoneWeightCollection>;
			Byte4[] outputIndices = new Byte4[channel.Count];
			Vector4[] outputWeights = new Vector4[channel.Count];
			for (int i = 0; i < channel.Count; i++)
			{
				BoneWeightCollection inputWeights = channel[i];
				ConvertVertexWeights(context, asset, inputWeights, boneIndices, outputIndices, outputWeights, i, geometry, boneRemap);
			}
			int usageIndex = VertexChannelNames.DecodeUsageIndex(channel.Name);
			string name = VertexChannelNames.EncodeName(VertexElementUsage.BlendIndices, usageIndex);
			string str = VertexChannelNames.EncodeName(VertexElementUsage.BlendWeight, usageIndex);
			channels.Insert<Byte4>(vertexChannelIndex + 1, name, outputIndices);
			channels.Insert<Vector4>(vertexChannelIndex + 2, str, outputWeights);
			channels.RemoveAt(vertexChannelIndex);
		}
		static int[] tempIndices = new int[4];
		static float[] tempWeights = new float[4];
		private static void ConvertVertexWeights(ContentProcessorContext context, string asset, BoneWeightCollection inputWeights, Dictionary<string, int> boneIndices, Byte4[] outputIndices, Vector4[] outputWeights, int vertexIndex, GeometryContent geometry, Dictionary<int, int> boneRemap)
		{
			inputWeights.NormalizeWeights(4);
			for (int i = 0; i < inputWeights.Count; i++)
			{
				BoneWeight weight = inputWeights[i];
				if (!boneIndices.TryGetValue(weight.BoneName, out tempIndices[i]))
				{
					string boneName = weight.BoneName.Replace("__Skeleton", "");

					if (!boneIndices.TryGetValue(boneName, out tempIndices[i]))
					{
						context.Logger.LogWarning(null, new ContentIdentity(asset), "Unknown bone name: " + weight.BoneName);
						//throw new InvalidContentException("Unknown bone name: " + weight.BoneName);
						continue;
					}
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
			outputIndices[vertexIndex] = new Byte4((float)tempIndices[0], (float)tempIndices[1], (float)tempIndices[2], (float)tempIndices[3]);
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

			//avatars require their bones to be sorted in a slighty strange order.
			//the order is not setup in the file

			//they must be sorted first by depth (so root first, then it's children, then all their children)
			//and within those groups, sorted by name.

			//this is done in a method ripped from the XNA samples... (XnaFlattenSkeleton)

			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			
			IList<BoneContent> list = XnaFlattenSkeleton(skeleton);

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


		#region XNA skeleton hacks

		//taken from XNA tutorials...

		/// <summary>
		/// Removes each bone node that contains "_END" in the name/
		/// </summary>
		/// <remarks>
		/// These bones are not needed by the AvatarRenderer runtime but
		/// are part of the Avatar rig used in modeling programs
		/// </remarks>
		static void RemoveEndBonesAndFixBoneNames(NodeContent bone)
		{
			// safety-check the parameter
			if (bone == null)
			{
				throw new ArgumentNullException("bone");
			}

			// Remove unneeded text from the bone name
			bone.Name = CleanBoneName(bone.Name);

			// Remove each child bone that contains "_END" in the name
			for (int i = 0; i < bone.Children.Count; ++i)
			{
				NodeContent child = bone.Children[i];
				if (child.Name.Contains("_END"))
				{
					bone.Children.Remove(child);
					--i;
				}
				else
				{
					// Recursively search through the remaining child bones
					RemoveEndBonesAndFixBoneNames(child);
				}
			}
		}


		/// <summary>
		/// Removes extra text from the bone names
		/// </summary>
		static string CleanBoneName(string boneName)
		{
			boneName = boneName.Replace("__Skeleton", "");
			return boneName;
		}

		/// <summary>
		/// Flattens the skeleton into a list. The order in the list is sorted by
		/// depth first and then by name. This is taken from the XNA avatar sample.
		/// </summary>
		private static IList<BoneContent> XnaFlattenSkeleton(BoneContent skeleton)
		{
			// Create the destination list of bones
			List<BoneContent> bones = new List<BoneContent>();

			// Create a list to track current items in the level of tree
			List<BoneContent> currentLevel = new List<BoneContent>();

			// Add the root node of the skeleton to the list
			currentLevel.Add(skeleton);

			while (currentLevel.Count > 0)
			{
				// Create a list of bones to track the next level of the tree
				List<BoneContent> nextLevel = new List<BoneContent>();

				// Sort the bones in the current level 
				currentLevel.Sort(delegate(BoneContent a, BoneContent b) { return a.Name.CompareTo(b.Name); });

				// Add the newly sorted items to the output list
				foreach (BoneContent bone in currentLevel)
				{
					bones.Add(bone);
					// Add the bone's children to the next-level list
					foreach (NodeContent child in bone.Children)
					{
						if (child is BoneContent)
							nextLevel.Add(child as BoneContent);
					}
				}

				// the next level is now the current level
				currentLevel = nextLevel;
			}

			// return the flattened array of bones
			return bones;
		}


		#endregion


		#region Processor params
		float animationCompressionTolerancePercent = 0.25f;
		Vector3 bindPoseTranslation = new Vector3(0.000f, 75.5199f, -0.8664f);

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

		#endregion
	}

}