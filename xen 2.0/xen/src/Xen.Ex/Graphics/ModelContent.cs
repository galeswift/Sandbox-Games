using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Xen.Graphics;
using Xen.Ex.Graphics.Content;
using Xen.Ex.Material;
using Xen.Ex.Compression;
using Xen.Threading;

namespace Xen.Ex.Graphics.Content
{
	/// <summary>
	/// A structure storing bounding box and bounding sphere data
	/// </summary>
	public struct GeometryBounds
	{
		internal Vector3 minimum, maximum;
		internal Vector3 radiusCentre;
		internal float radius;

		/// <summary>
		/// Bounding Box Minimum extents
		/// </summary>
		public Vector3 Minimum { get { return minimum; } }
		/// <summary>
		/// Bounding Box Maximum extents
		/// </summary>
		public Vector3 Maximum { get { return maximum; } }
		/// <summary>
		/// Bounding sphere centre point
		/// </summary>
		public Vector3 RadiusCentre { get { return radiusCentre; } }
		/// <summary>
		/// Bounding sphere radius
		/// </summary>
		public float Radius { get { return radius; } }

#if DEBUG && !XBOX360

		/// <summary></summary>
		public GeometryBounds(Vector3 max, Vector3 min, float radius, Vector3 radiusCentre)
		{
			this.maximum = max;
			this.minimum = min;
			this.radius = radius;
			this.radiusCentre = radiusCentre;
		}

#endif

		/// <summary></summary>
		public GeometryBounds(BinaryReader reader)
		{
			this.maximum = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			this.minimum = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			this.radius = reader.ReadSingle();
			this.radiusCentre = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}

		/// <summary>builds a geometry bounds that encompasses a set of bounds</summary>
		internal GeometryBounds(GeometryBounds[] bounds)
		{
			maximum = new Vector3();
			minimum = new Vector3();
			Vector3 v;

			if (bounds.Length != 0)
				this = bounds[0];
			for (int i = 1; i < bounds.Length; i++)
			{
				v = bounds[i].maximum;
				Vector3.Max(ref v, ref this.maximum, out maximum);
				v = bounds[i].minimum;
				Vector3.Min(ref v, ref this.minimum, out minimum);
			}
			this.radiusCentre = minimum + (maximum - minimum) * 0.5f;
			this.radius = 0;
			for (int i = 0; i < bounds.Length; i++)
			{
				Vector3 point = new Vector3();
				for (int x = 0; x < 2; x++)
				for (int y = 0; y < 2; y++)
				for (int z = 0; z < 2; z++)
				{
					point.X = x == 0 ? bounds[i].minimum.X : bounds[i].maximum.X;
					point.Y = y == 0 ? bounds[i].minimum.Y : bounds[i].maximum.Y;
					point.Z = z == 0 ? bounds[i].minimum.Z : bounds[i].maximum.Z;
					this.radius = Math.Max(this.radius, (this.radiusCentre - point).LengthSquared());
				}
			}
			this.radius = (float)Math.Sqrt(this.radius);
		}

#if DEBUG && !XBOX360

		private GeometryBounds Add(GeometryBounds bound)
		{
			return new GeometryBounds(this.maximum + bound.maximum, this.minimum + bound.minimum, this.radius + bound.Radius, this.radiusCentre + bound.radiusCentre);
		}
		internal GeometryBounds Difference(GeometryBounds bounds)
		{
			return new GeometryBounds(this.maximum - bounds.maximum, this.minimum - bounds.minimum, this.radius - bounds.Radius, this.radiusCentre - bounds.radiusCentre);
		}

		internal GeometryBounds(GeometryBounds staticBound, GeometryBounds[] offsets)
		{
			GeometryBounds[] bounds = new GeometryBounds[offsets.Length];

			for (int i = 0; i < offsets.Length; i++)
				bounds[i] = staticBound.Add(offsets[i]);

			this = new GeometryBounds(bounds).Difference(staticBound);
		}

		/// <summary>Combine this bounds with the specified bounds, return the result</summary>
		public GeometryBounds Combine(ref GeometryBounds bound)
		{
			GeometryBounds previous = this;
			Vector3 v;
			Vector3 RadiusCentre = this.radiusCentre;
			Vector3 Minimum = this.minimum;
			Vector3 Maximum = this.maximum;
			float Radius = this.radius;


			v = bound.maximum;
			Vector3.Max(ref v, ref Maximum, out Maximum);
			v = bound.minimum;
			Vector3.Min(ref v, ref Minimum, out Minimum);

			RadiusCentre = Minimum + (Maximum - Minimum) * 0.5f;
			Radius = 0;

			Vector3 point = new Vector3();
			for (int x = 0; x < 2; x++)
			for (int y = 0; y < 2; y++)
			for (int z = 0; z < 2; z++)
			{
				point.X = x == 0 ? bound.minimum.X : bound.maximum.X;
				point.Y = y == 0 ? bound.minimum.Y : bound.maximum.Y;
				point.Z = z == 0 ? bound.minimum.Z : bound.maximum.Z;
				Radius = Math.Max(Radius, (RadiusCentre - point).LengthSquared());
			}
			for (int x = 0; x < 2; x++)
			for (int y = 0; y < 2; y++)
			for (int z = 0; z < 2; z++)
			{
				point.X = x == 0 ? previous.minimum.X : previous.maximum.X;
				point.Y = y == 0 ? previous.minimum.Y : previous.maximum.Y;
				point.Z = z == 0 ? previous.minimum.Z : previous.maximum.Z;
				Radius = Math.Max(Radius, (RadiusCentre - point).LengthSquared());
			}

			Radius = (float)Math.Sqrt(Radius);
			return new GeometryBounds(Maximum, Minimum, Radius, RadiusCentre);
		}
		internal void Write(BinaryWriter writer)
		{
			writer.Write(maximum.X);
			writer.Write(maximum.Y);
			writer.Write(maximum.Z);

			writer.Write(minimum.X);
			writer.Write(minimum.Y);
			writer.Write(minimum.Z);

			writer.Write(radius);

			writer.Write(radiusCentre.X);
			writer.Write(radiusCentre.Y);
			writer.Write(radiusCentre.Z);
		}

#endif

		/// <summary>
		/// <para>Transforms the bounds by a matrix, generating a new bounds</para>
		/// <para>The output bounds is axis aligned</para>
		/// </summary>
		/// <param name="transform"></param>
		/// <returns></returns>
		public GeometryBounds Transform(ref Matrix transform)
		{
			Vector3 RadiusCentre = this.radiusCentre;
			Vector3 Minimum = this.minimum;
			Vector3 Maximum = this.maximum;
			float Radius = this.radius;

			Vector3.Transform(ref RadiusCentre, ref transform, out RadiusCentre);
			Vector3 min, max;
			Vector3.Transform(ref Minimum, ref transform, out min);
			max = min;

			Vector3 point = new Vector3();
			for (int x = 0; x < 2; x++)
			for (int y = 0; y < 2; y++)
			for (int z = 0; z < 2; z++)
			{
				point.X = x == 0 ? this.minimum.X : this.maximum.X;
				point.Y = y == 0 ? this.minimum.Y : this.maximum.Y;
				point.Z = z == 0 ? this.minimum.Z : this.maximum.Z;
				Vector3.Transform(ref point, ref transform, out point);

				Vector3.Min(ref min, ref point, out min);
				Vector3.Max(ref max, ref point, out max);
			}

			Minimum = min;
			Maximum = max;

			GeometryBounds result = new GeometryBounds();
			result.minimum = Minimum;
			result.maximum = Maximum;

			Vector3 dif = new Vector3();
			dif.X = Minimum.X - Maximum.X;
			dif.Y = Minimum.Y - Maximum.Y;
			dif.Z = Minimum.Z - Maximum.Z;

			result.radius = (float)Math.Sqrt(dif.X*dif.X+dif.Y*dif.Y+dif.Z*dif.Z) * 0.5f; // Radius;

			result.radiusCentre.X = (Minimum.X + Maximum.X) * 0.5f;
			result.radiusCentre.Y = (Minimum.Y + Maximum.Y) * 0.5f;
			result.radiusCentre.Z = (Minimum.Z + Maximum.Z) * 0.5f;
			//result.radiusCentre = RadiusCentre;
			return result;
		}
	}

	/// <summary>
	/// Content class for a model imported using the xen model importer
	/// </summary>
	public sealed class ModelData
	{
		private const int version = 1;
		readonly private string name;
		readonly internal MeshData[] meshes;
		readonly internal SkeletonData skeleton;
		readonly internal AnimationData[] animations;
		internal readonly GeometryBounds[] animationStaticBounds;
		internal GeometryBounds staticBounds;

		/// <summary>
		/// Gets the bounding box for the non-animated model data
		/// </summary>
		public GeometryBounds StaticBounds { get { return staticBounds; } }

		internal class RuntimeReader : ContentTypeReader<ModelData>
		{
			protected override ModelData Read(ContentReader input, ModelData existingInstance)
			{
				if (existingInstance != null)
					existingInstance.UpdateTextures(input.ContentManager, Path.GetDirectoryName(input.AssetName));

				return existingInstance ?? new ModelData(input);
			}
		}

		internal static string RuntimeReaderType
		{
			get { return typeof(RuntimeReader).AssemblyQualifiedName; }
		}

#if DEBUG && !XBOX360

		/// <summary></summary>
		public ModelData(string name, MeshData[] meshes, SkeletonData skeleton, AnimationData[] animations)
		{
			this.name = name;
			this.meshes = meshes;
			this.skeleton = skeleton;
			this.animations = animations;

			GeometryBounds[] bounds = new GeometryBounds[meshes.Length];
			for (int i = 0; i < bounds.Length; i++)
				bounds[i] = meshes[i].staticBounds;

			this.staticBounds = new GeometryBounds(bounds);

			this.animationStaticBounds = new GeometryBounds[animations.Length];
			for (int i = 0; i < animations.Length; i++)
			{
				GeometryBounds[] children = new GeometryBounds[meshes.Length];
				for (int n = 0; n < meshes.Length; n++)
					children[n] = meshes[n].AnimationStaticBoundsOffset[i];
				this.animationStaticBounds[i] = new GeometryBounds(this.staticBounds, children);
			}
		}

#endif

		internal void UpdateTextures(ContentManager manager, string baseDir)
		{
			foreach (MeshData mesh in this.meshes)
			{
				foreach (GeometryData geom in mesh.geometry)
				{
					geom.UpdateTextures(manager, baseDir);
				}
			}
		}

		/// <summary>
		/// Name of the model
		/// </summary>
		public string Name { get { return name; } }
		/// <summary>
		/// Gets a readonly array of <see cref="MeshData"/> instances stored in the model data
		/// </summary>
		public ReadOnlyArrayCollection<MeshData> Meshes { get { return new ReadOnlyArrayCollection<MeshData>(meshes); } }
		/// <summary>
		/// Gets a readonly array of animations stored in the model
		/// </summary>
		public ReadOnlyArrayCollection<AnimationData> Animations { get { return new ReadOnlyArrayCollection<AnimationData>(animations); } }
		/// <summary>
		/// Gets the skeleton used by this model (this value may be null)
		/// </summary>
		public SkeletonData Skeleton { get { return skeleton; } }
		/// <summary>
		/// <para>Gets bounds offsets for each animation (assuming animation has a weighting of 1.0f)</para>
		/// <para>This offset is primarily used to adjust the bounding box of a model when an animation is playing</para>
		/// </summary>
		public ReadOnlyArrayCollection<GeometryBounds> AnimationStaticBoundsOffset
		{
			get { return new ReadOnlyArrayCollection<GeometryBounds>(animationStaticBounds); }
		}

		internal ModelData(ContentReader reader)
		{
			int fileVersion = reader.ReadInt32();
			if (version != fileVersion)
				throw new InvalidOperationException("Serialiezd ModelData version mismatch");
			this.name = string.Intern(reader.ReadString());
			int count = reader.ReadInt32();
			this.meshes = new MeshData[count];
			Material.MaterialShader sharedDefaultShader = new MaterialShader();
			for (int i = 0; i < count; i++)
				this.meshes[i] = new MeshData(reader);
			bool hasSkel = reader.ReadBoolean();
			if (hasSkel)
				this.skeleton = new SkeletonData(reader, false);
			count = reader.ReadInt32();
			this.animations = new AnimationData[count];
			for (int i = 0; i < count; i++)
				this.animations[i] = new AnimationData(reader, i);

			GeometryBounds[] bounds = new GeometryBounds[meshes.Length];
			for (int i = 0; i < bounds.Length; i++)
				bounds[i] = meshes[i].staticBounds;

			this.staticBounds = new GeometryBounds(bounds);

			count = (int)reader.ReadInt16();
			animationStaticBounds = new GeometryBounds[count];
			for (int i = 0; i < animationStaticBounds.Length; i++)
				animationStaticBounds[i] = new GeometryBounds(reader);

			foreach (MeshData mesh in this.meshes)
			{
				//static bounds will encompass the entire object. So use something a bit smaller
				foreach (GeometryData geom in mesh.geometry)
					geom.defaultShader.LightingDisplayModelRadius = this.staticBounds.radius / 2;
			}
		}

#if DEBUG && !XBOX360

		internal void Write(BinaryWriter writer, Func<BinaryWriter, object, bool> writeVertexData)
		{
			writer.Write(version);
			writer.Write(name ?? "");
			writer.Write(meshes.Length);
			foreach (MeshData mesh in meshes)
				mesh.Write(writer, writeVertexData);
			writer.Write(skeleton != null);
			if (skeleton != null)
				skeleton.Write(writer, false);
			writer.Write(animations.Length);
			foreach (AnimationData animation in animations)
				animation.Write(writer);

			writer.Write((short)animationStaticBounds.Length);
			for (int i = 0; i < animationStaticBounds.Length; i++)
				animationStaticBounds[i].Write(writer);
		}

#endif
	}

	/// <summary>
	/// Stores content data for a mesh defined within a model
	/// </summary>
	public sealed class MeshData
	{
		readonly private string name;
		readonly internal GeometryData[] geometry;
		internal readonly GeometryBounds[] animationStaticBounds;
		internal GeometryBounds staticBounds;

#if DEBUG && !XBOX360

		/// <summary></summary>
		public MeshData(string name, GeometryData[] data, AnimationData[] animations)
		{
			this.name = name;
			this.geometry = data;

			GeometryBounds[] bounds = new GeometryBounds[data.Length];
			for (int i = 0; i < bounds.Length; i++)
				bounds[i] = data[i].staticBounds;

			this.staticBounds = new GeometryBounds(bounds);

			this.animationStaticBounds = new GeometryBounds[animations.Length];
			for (int i = 0; i < animations.Length; i++)
			{
				GeometryBounds[] children = new GeometryBounds[data.Length];
				for (int n = 0; n < data.Length; n++)
					children[n] = data[n].AnimationStaticBoundsOffset[i];
				this.animationStaticBounds[i] = new GeometryBounds(this.staticBounds, children);
			}
		}

#endif


		/// <summary>
		/// Gets the bounding box for the non-animated mesh data
		/// </summary>
		public GeometryBounds StaticBounds { get { return staticBounds; } }

		/// <summary>
		/// Gets a readonly array of the geometry stored in this mesh
		/// </summary>
		public ReadOnlyArrayCollection<GeometryData> Geometry
		{
			get { return new ReadOnlyArrayCollection<GeometryData>(geometry); }
		}
		/// <summary>
		/// Gets the name of this mesh
		/// </summary>
		public string Name { get { return name; } }

		/// <summary>
		/// <para>Gets bounds offsets for each animation (assuming animation has a weighting of 1.0f)</para>
		/// <para>This offset is primarily used to adjust the bounding box of a model when an animation is playing</para>
		/// </summary>
		public ReadOnlyArrayCollection<GeometryBounds> AnimationStaticBoundsOffset
		{
			get { return new ReadOnlyArrayCollection<GeometryBounds>(animationStaticBounds); }
		}

#if DEBUG && !XBOX360

		internal void Write(BinaryWriter writer, Func<BinaryWriter, object, bool> writeVertexData)
		{
			writer.Write(name ?? "");

			writer.Write(geometry.Length);
			for (int i = 0; i < geometry.Length; i++)
				geometry[i].Write(writer, writeVertexData);

			writer.Write((short)animationStaticBounds.Length);
			for (int i = 0; i < animationStaticBounds.Length; i++)
				animationStaticBounds[i].Write(writer);
		}

#endif

		internal MeshData(ContentReader reader)
		{
			name = string.Intern(reader.ReadString());

			int count = reader.ReadInt32();
			this.geometry = new GeometryData[count];
			for (int i = 0; i < count; i++)
				this.geometry[i] = new GeometryData(reader);

			GeometryBounds[] bounds = new GeometryBounds[geometry.Length];
			for (int i = 0; i < bounds.Length; i++)
				bounds[i] = geometry[i].staticBounds;

			this.staticBounds = new GeometryBounds(bounds);

			count = (int)reader.ReadInt16();
			animationStaticBounds = new GeometryBounds[count];
			for (int i = 0; i < animationStaticBounds.Length; i++)
				animationStaticBounds[i] = new GeometryBounds(reader);
		}
	}

	/// <summary>
	/// Structure storing the material properties of a <see cref="GeometryData"/> instance
	/// </summary>
	public struct MaterialData
	{
		internal readonly float alpha, specularPower;
		internal readonly Vector3 diffuse, emissive, specular;
		internal readonly string textureFileName, normalMapFileName;
		internal readonly bool useVertexColour;
		private Texture2D textureMap, normalMap;

		/// <summary>Alpha channel value</summary>
		public float Alpha { get { return alpha; } }
		/// <summary>Light specular (shine) reflection power. Higher numbers produce more focused specular reflections</summary>
		public float SpecularPower { get { return specularPower; } }
		/// <summary>Light diffuse reflection colour</summary>
		public Vector3 DiffuseColour { get { return diffuse; } }
		/// <summary>Light emittance</summary>
		public Vector3 EmissiveColour { get { return emissive; } }
		/// <summary>Light specular reflection colour</summary>
		public Vector3 SpecularColour { get { return specular; } }
		/// <summary>Modulate output colour by the vertex colour</summary>
		public bool UseVertexColour { get { return useVertexColour; } }
		/// <summary></summary>
		public Texture2D Texture { get { return textureMap; } }
		/// <summary></summary>
		public Texture2D NormalMap { get { return normalMap; } }
		/// <summary></summary>
		public string TextureFileName { get { return textureFileName; } }
		/// <summary></summary>
		public string NormalMapFileName { get { return normalMapFileName; } }

#if DEBUG && !XBOX360

		/// <summary></summary>
		public MaterialData(
			float alpha, float specularPower,
			Vector3 diffuse, Vector3 emissive, Vector3 specular,
			string texture, string normalMap,
			bool useVertexColour)
		{
			this.alpha = alpha;
			this.specularPower = specularPower;
			this.diffuse = diffuse;
			this.emissive = emissive;
			this.specular = specular;
			this.textureFileName = texture;
			this.normalMapFileName = normalMap;
			this.useVertexColour = useVertexColour;
			this.textureMap = null;
			this.normalMap = null;
		}

#endif

		internal MaterialData(ContentReader reader)
		{
			this.alpha = reader.ReadSingle();
			this.specularPower = reader.ReadSingle();
			this.diffuse = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			this.emissive = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			this.specular = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			this.textureFileName = string.Intern(reader.ReadString());
			this.normalMapFileName = string.Intern(reader.ReadString());
			this.useVertexColour = reader.ReadBoolean();

			this.textureMap = null;
			this.normalMap = null;

			UpdateTextures(reader.ContentManager, Path.GetDirectoryName(reader.AssetName));
		}

#if DEBUG && !XBOX360

		/// <summary></summary>
		public void Write(BinaryWriter writer)
		{
			writer.Write(alpha);
			writer.Write(specularPower);
			writer.Write(diffuse.X);
			writer.Write(diffuse.Y);
			writer.Write(diffuse.Z);

			writer.Write(emissive.X);
			writer.Write(emissive.Y);
			writer.Write(emissive.Z);

			writer.Write(specular.X);
			writer.Write(specular.Y);
			writer.Write(specular.Z);

			writer.Write(textureFileName);
			writer.Write(normalMapFileName);
			writer.Write(useVertexColour);
		}

#endif

		internal void UpdateTextures(ContentManager manager, string baseDir)
		{
			this.textureMap = null;
			this.normalMap = null;
			if (baseDir.Length > 0)
				baseDir += @"\";

			if (this.textureFileName != null && this.textureFileName.Length > 0)
				this.textureMap = manager.Load<Texture2D>(baseDir.Length == 0 ? this.textureFileName : baseDir + this.textureFileName);
			if (this.normalMapFileName != null && this.normalMapFileName.Length > 0)
				this.normalMap = manager.Load<Texture2D>(baseDir.Length == 0 ? this.normalMapFileName : baseDir + this.normalMapFileName);
		}
	}

	/// <summary>
	/// This class stores geometry data, such as vertex buffers and material
	/// </summary>
	public sealed class GeometryData
	{
		readonly private string name;
		readonly private VertexElement[] vertexElements;
		readonly private IVertices vertices;
		readonly private IIndices indices;
		private readonly int[] boneIndices;
		private readonly GeometryBounds[] boneLocalBounds;
		internal readonly GeometryBounds[] animationStaticBounds;
		internal GeometryBounds staticBounds;
		private readonly int index;
		internal readonly MaterialShader defaultShader;
		internal readonly uint[] geometryBoneRemapping;
		internal readonly int geometryBoneSingleIndex;

		/// <summary>
		/// Get the stored material data for this geometry
		/// </summary>
		public readonly MaterialData MaterialData;

#if DEBUG && !XBOX360
		readonly private byte[] vertexData;
		readonly private object vertexBuffer;
		readonly private int maxIndex;
		readonly private int[] indexData;

		/// <summary></summary>
		public GeometryData(int index, string name, VertexElement[] elements, object vertexBuffer, byte[] vertexData, int[] indexData, MaterialData material, SkeletonData skeleton, AnimationData[] animations, bool targetXbox, uint[] geometryBoneRemapping, int geometryBoneSingleIndex)
		{
			this.index = index;
			this.name = name;
			this.vertexElements = elements;
			this.vertexData = vertexData;
			this.vertexBuffer = vertexBuffer;
			this.vertices = null;
			this.indices = null;
			this.indexData = indexData;
			for (int i = 0; i < indexData.Length; i++)
				maxIndex = Math.Max(indexData[i], maxIndex);
			this.MaterialData = material;
			this.geometryBoneRemapping = geometryBoneRemapping;
			this.geometryBoneSingleIndex = geometryBoneSingleIndex;

			ComputeBoneBounds(skeleton, ExtractBounds(out this.staticBounds, targetXbox), out boneIndices, out boneLocalBounds);

			if (animations != null)
			{
				this.animationStaticBounds = new GeometryBounds[animations.Length];
				ComputeAnimationBounds(skeleton, animations);
			}
			else
				animationStaticBounds = new GeometryBounds[0];
		}

		private void ComputeAnimationBounds(SkeletonData skeleton, AnimationData[] animations)
		{
			if (skeleton == null)
				return;
		
			Matrix[] worldBones = skeleton.BoneLocalMatrices.ToArray();
			Matrix[] transforms = new Matrix[skeleton.BoneCount];

			for (int animIndex = 0; animIndex < animations.Length; animIndex++)
			{
				GeometryBounds bounds = new GeometryBounds();
				AnimationData anim = animations[animIndex];

				BinaryReader[] data = new BinaryReader[anim.BoneCount];
				CompressedTransformReader[] readers = new CompressedTransformReader[anim.BoneCount];


				for (int i = 0; i < anim.BoneCount; i++)
				{
					data[i] = new BinaryReader(anim.GetBoneCompressedTransformStream(i));
					readers[i] = new CompressedTransformReader();
				}

				for (int frame = 0; frame < anim.KeyFrameCount; frame++)
				{
					for (int i = 0; i < transforms.Length; i++)
						transforms[i] = Matrix.Identity;

					Transform t;
					for (int i = 0; i < anim.BoneCount; i++)
					{
						int bi = anim.BoneIndices[i];
						readers[i].MoveNext(data[i]);
						readers[i].GetTransform(out t);

						t.GetMatrix(out transforms[bi]);
						transforms[bi] = worldBones[bi] * transforms[bi];
					}
					skeleton.TransformHierarchy(transforms);

					for (int i = 0; i < this.boneIndices.Length; i++)
					{
						int bi = this.boneIndices[i];

						GeometryBounds bound = this.boneLocalBounds[i];
						bound = bound.Transform(ref transforms[bi]);
						if (frame == 0 && i == 0)
							bounds = bound;
						else
							bounds = bounds.Combine(ref bound);
					}
				}

				animationStaticBounds[animIndex] = bounds.Difference(staticBounds);
			}
		}

		#region compute bounds

		/// <summary>
		/// Gets the bounding box for the non-animated model data
		/// </summary>
		public GeometryBounds StaticBounds { get { return staticBounds; } }

		struct BlendedVertex
		{
			public Vector3 positon;
			public int i0,i1,i2,i3;
		}

		void ComputeBoneBounds(SkeletonData skeleton, BlendedVertex[] vertices, out int[] boneIndices, out GeometryBounds[] boneLocalBounds)
		{
			boneIndices = null;
			boneLocalBounds = null;

			if (skeleton == null)
				return;

			Matrix[] bones = (Matrix[])skeleton.boneLocalMatrices.Clone();
			skeleton.TransformHierarchy(bones);

			for (int i = 0; i < bones.Length; i++)
				Matrix.Invert(ref bones[i], out bones[i]);

			int bc = skeleton.BoneCount;
			List<Vector3>[] boneVertices = new List<Vector3>[skeleton.BoneCount];

			foreach (BlendedVertex v in vertices)
			{
				if (v.i0 >= 0 && v.i0 < bc) AddList(ref boneVertices[v.i0], v.positon, ref bones[v.i0]);
				if (v.i1 >= 0 && v.i1 < bc) AddList(ref boneVertices[v.i1], v.positon, ref bones[v.i1]);
				if (v.i2 >= 0 && v.i2 < bc) AddList(ref boneVertices[v.i2], v.positon, ref bones[v.i2]);
				if (v.i3 >= 0 && v.i3 < bc) AddList(ref boneVertices[v.i3], v.positon, ref bones[v.i3]);
			}

			List<int> indices = new List<int>();
			List<GeometryBounds> bounds = new List<GeometryBounds>();

			for (int i = 0; i < boneVertices.Length; i++)
			{
				List<Vector3> verts = boneVertices[i];
				if (verts == null ||
					verts.Count == 0)
					continue;

				Vector3 max = verts[0], min = verts[0];
				for (int v = 1; v < verts.Count; v++)
				{
					Vector3 vert = verts[v];
					Vector3.Max(ref max, ref vert, out max);
					Vector3.Min(ref min, ref vert, out min);
				}
				Vector3 centre = min + (max - min) * 0.5f;
				float radius = 0;
				for (int v = 0; v < verts.Count; v++)
				{
					radius = Math.Max(radius, (verts[v] - centre).Length());
				}

				indices.Add(i);
				bounds.Add(new GeometryBounds(max, min, radius, centre));
			}

			boneIndices = indices.ToArray();
			boneLocalBounds = bounds.ToArray();
		}
		void AddList(ref List<Vector3> list, Vector3 v, ref Matrix mat)
		{
			if (list == null)
				list = new List<Vector3>();
			Vector3.Transform(ref v, ref mat, out v);
			list.Add(v);
		}

		BlendedVertex[] ExtractBounds(out GeometryBounds staticBounds, bool targetXbox)
		{
			staticBounds = new GeometryBounds();

			VertexElementFormat format, blendFormat, weightFormat;

			int offset, blendOffset, weightOffset;
			int stride = VertexElementAttribute.CalculateVertexStride(this.vertexElements);
			if (!VertexElementAttribute.ExtractUsage(this.vertexElements, VertexElementUsage.Position, 0, out format, out offset))
				throw new ArgumentException("Geometry data vertex format has no position values");
			if (format != VertexElementFormat.Vector3 &&
				format != VertexElementFormat.Vector4)
				return null;


			bool hasBlending = VertexElementAttribute.ExtractUsage(this.vertexElements, VertexElementUsage.BlendIndices, 0, out blendFormat, out blendOffset) &&
				blendFormat == VertexElementFormat.Byte4;

			hasBlending &= VertexElementAttribute.ExtractUsage(this.vertexElements, VertexElementUsage.BlendWeight, 0, out weightFormat, out weightOffset) &&
				weightFormat == VertexElementFormat.Vector4;

			BlendedVertex[] verts = new BlendedVertex[this.vertexData.Length / stride];
			for (int i = 0; i < verts.Length; i++)
			{
				verts[i].i0 = -1;
				verts[i].i1 = -1;
				verts[i].i2 = -1;
				verts[i].i3 = -1;
			}

			Vector3 min = new Vector3(), max = new Vector3();
			float[] values = new float[4];
			BitCast cast = new BitCast();

			int index = offset;
			bool first = true;
			int v = 0;

			while (index < this.vertexData.Length - 12)
			{
				for (int i = 0; i < 12; )
				{
					//if (targetXbox)
					//{
					//    cast.Byte3 = this.vertexData[index + i++];
					//    cast.Byte2 = this.vertexData[index + i++];
					//    cast.Byte1 = this.vertexData[index + i++];
					//    cast.Byte0 = this.vertexData[index + i];
					//}
					//else
					{
						cast.Byte0 = this.vertexData[index + i++];
						cast.Byte1 = this.vertexData[index + i++];
						cast.Byte2 = this.vertexData[index + i++];
						cast.Byte3 = this.vertexData[index + i];
					}

					values[i++ / 4] = cast.Single;
				}
				Vector3 pos = new Vector3(values[0], values[1], values[2]);

				if (first)
				{
					first = false;
					min = pos;
					max = pos;
				}
				else
				{
					Vector3.Max(ref pos, ref max, out max);
					Vector3.Min(ref pos, ref min, out min);
				}


				if (hasBlending)
				{
					//if (targetXbox)
					//{
					//    verts[v].i3 = this.vertexData[blendOffset + 0];
					//    verts[v].i2 = this.vertexData[blendOffset + 1];
					//    verts[v].i1 = this.vertexData[blendOffset + 2];
					//    verts[v].i0 = this.vertexData[blendOffset + 3];
					//}
					//else
					{
						verts[v].i0 = this.vertexData[blendOffset + 0];
						verts[v].i1 = this.vertexData[blendOffset + 1];
						verts[v].i2 = this.vertexData[blendOffset + 2];
						verts[v].i3 = this.vertexData[blendOffset + 3];
					}


					for (int i = 0; i < 16; )
					{
						//if (targetXbox)
						//{
						//    cast.Byte3 = this.vertexData[weightOffset + i++];
						//    cast.Byte2 = this.vertexData[weightOffset + i++];
						//    cast.Byte1 = this.vertexData[weightOffset + i++];
						//    cast.Byte0 = this.vertexData[weightOffset + i];
						//}
						//else
						{
							cast.Byte0 = this.vertexData[weightOffset + i++];
							cast.Byte1 = this.vertexData[weightOffset + i++];
							cast.Byte2 = this.vertexData[weightOffset + i++];
							cast.Byte3 = this.vertexData[weightOffset + i];
						}

						values[i++ / 4] = cast.Single;
					}

					if (values[0] <= 0)
						verts[v].i0 = -1;
					if (values[1] <= 0)
						verts[v].i1 = -1;
					if (values[2] <= 0)
						verts[v].i2 = -1;
					if (values[3] <= 0)
						verts[v].i3 = -1;
				}

				verts[v++].positon = pos;

				index += stride;
				blendOffset += stride;
				weightOffset += stride;
			}

			Vector3 centre = min + (max - min) * 0.5f;
			float radius = 0;
			for (int i = 0; i < verts.Length; i++)
				radius = Math.Max(radius, (verts[i].positon - centre).LengthSquared());

			staticBounds = new GeometryBounds(max, min,(float)Math.Sqrt(radius),centre);
			return verts;
		}
		
		#endregion

#endif

		/// <summary>
		/// <para>Gets bounds offsets for each animation (assuming animation has a weighting of 1.0f)</para>
		/// <para>This offset is primarily used to adjust the bounding box of a model when an animation is playing</para>
		/// </summary>
		public ReadOnlyArrayCollection<GeometryBounds> AnimationStaticBoundsOffset
		{
			get { return new ReadOnlyArrayCollection<GeometryBounds>(animationStaticBounds); }
		}

		/// <summary>
		/// Gets the indices of the bones this geometry uses
		/// </summary>
		public ReadOnlyArrayCollection<int> BoneIndices { get { return new ReadOnlyArrayCollection<int>(boneIndices); } }
		/// <summary>
		/// <para>Gets the bone-local bounds for this geometry (based on <see cref="BoneIndices"/>)</para>
		/// <para>These bounds could be used as collision detection bounding boxes</para>
		/// </summary>
		public ReadOnlyArrayCollection<GeometryBounds> BoneLocalBounds { get { return new ReadOnlyArrayCollection<GeometryBounds>(boneLocalBounds); } }

		internal void UpdateTextures(ContentManager manager, string baseDir)
		{
			MaterialData.UpdateTextures(manager, baseDir);

			defaultShader.Textures = new MaterialTextures();
			defaultShader.Textures.TextureMap = MaterialData.Texture;
			defaultShader.Textures.NormalMap = MaterialData.NormalMap;
		}

		/// <summary>
		/// Gets the vertices used to draw this geometry
		/// </summary>
		public IVertices Vertices
		{
			get { return vertices; }
		}
		/// <summary>
		/// Gets the indicies used to draw this geometry
		/// </summary>
		public IIndices Indices
		{
			get { return indices; }
		}
		/// <summary>
		/// Gets the name of this geometry
		/// </summary>
		public string Name { get { return name; } }
		/// <summary>
		/// Linear index of this geometry data in the root model data
		/// </summary>
		public int Index { get { return index; } }

#if DEBUG && !XBOX360

		internal void Write(BinaryWriter writer, Func<BinaryWriter, object, bool> writeVertexData)
		{
			writer.Write(this.index);
			writer.Write(name ?? "");
			writer.Write(vertexElements.Length);
			for (int i = 0; i < vertexElements.Length; i++)
			{
				Write(vertexElements[i], writer);
			}

			writeVertexData(writer, vertexBuffer);
		//	writer.Write(vertexData.Length);
		//	writer.Write(vertexData);
			
			byte bits = 8;
			if (maxIndex > byte.MaxValue)
				bits = 16;
			if (maxIndex > short.MaxValue)
				bits = 32;

			writer.Write(bits);
			writer.Write(this.indexData.Length);
			if (bits == 8)
				for (int i = 0; i < this.indexData.Length; i++)
					writer.Write((byte)this.indexData[i]);
			if (bits == 16)
				for (int i = 0; i < this.indexData.Length; i++)
					writer.Write((ushort)this.indexData[i]);
			if (bits == 32)
				for (int i = 0; i < this.indexData.Length; i++)
					writer.Write(this.indexData[i]);

			this.MaterialData.Write(writer);

			this.staticBounds.Write(writer);

			if (boneIndices == null)
				writer.Write((short)0);
			else
			{
				writer.Write((short)boneIndices.Length);
				for (int i = 0; i < boneIndices.Length; i++)
					writer.Write((short)boneIndices[i]);
				for (int i = 0; i < boneLocalBounds.Length; i++)
					boneLocalBounds[i].Write(writer);
			}

			writer.Write((short)animationStaticBounds.Length);
			for (int i = 0; i < animationStaticBounds.Length; i++)
				animationStaticBounds[i].Write(writer);

			writer.Write(geometryBoneRemapping == null ? (short)-1 : (short)geometryBoneRemapping.Length);
			if (geometryBoneRemapping != null)
			{
				for (int i = 0; i < geometryBoneRemapping.Length; i++)
					writer.Write((short)geometryBoneRemapping[i]);
			}
			writer.Write(geometryBoneSingleIndex);
		}

#endif

		internal GeometryData(ContentReader reader)
		{
			this.index = reader.ReadInt32();
			name = string.Intern(reader.ReadString());
			int count = reader.ReadInt32();
			this.vertexElements = new VertexElement[count];
			for (int i = 0; i < count; i++)
				this.vertexElements[i] = ReadElement(reader);

			var vb = reader.ReadObject<VertexBuffer>();
			this.vertices = Vertices<byte>.CreateRawDataVertices(vb, vertexElements);
			
			byte bits = reader.ReadByte();
			count = reader.ReadInt32();

			if (bits == 8 || bits == 16)
			{
				ushort[] inds = new ushort[count];

				if (bits == 8)
				{
					for (int i = 0; i < count; i++)
						inds[i] = (ushort)reader.ReadByte();
				}
				else
				{
					for (int i = 0; i < count; i++)
						inds[i] = reader.ReadUInt16();
				}
				this.indices = new Indices<ushort>(inds);
			}
			if (bits == 32)
			{
				int[] inds = new int[count];
				for (int i = 0; i < count; i++)
					inds[i] = reader.ReadInt32();
				this.indices = new Indices<int>(inds);
			}

			//XNA created vertex buffer is always readable... can't be controlled. So make the IB readable too.
			this.indices.ResourceUsage |= ResourceUsage.Readable;

			Xen.Application.ApplicationProviderService app =
				(Xen.Application.ApplicationProviderService)reader.ContentManager.ServiceProvider.GetService(typeof(Xen.Application.ApplicationProviderService));

			if (app != null)
			{
				this.vertices.Warm(app.Application);
				this.indices.Warm(app.Application);
			}

			this.MaterialData = new MaterialData(reader);

			this.staticBounds = new GeometryBounds(reader);

			count = (int)reader.ReadInt16();

			boneIndices = new int[count];
			boneLocalBounds = new GeometryBounds[count];

			for (int i = 0; i < count; i++)
				boneIndices[i] = (int)reader.ReadInt16();

			for (int i = 0; i < count; i++)
				boneLocalBounds[i] = new GeometryBounds(reader);

			count = (int)reader.ReadInt16();
			animationStaticBounds = new GeometryBounds[count];
			for (int i = 0; i < animationStaticBounds.Length; i++)
				animationStaticBounds[i] = new GeometryBounds(reader);

			this.defaultShader = new MaterialShader();

			defaultShader.Alpha = this.MaterialData.alpha;
			defaultShader.DiffuseColour = this.MaterialData.diffuse;
			defaultShader.EmissiveColour = this.MaterialData.emissive;
			defaultShader.Textures = new MaterialTextures();
			defaultShader.Textures.NormalMap = this.MaterialData.NormalMap;
			defaultShader.Textures.TextureMap = this.MaterialData.Texture;
			defaultShader.UseVertexColour = this.MaterialData.useVertexColour;

			count = (int)reader.ReadInt16();

			if (count != -1)
			{
				geometryBoneRemapping = new uint[count];
				for (int i = 0; i < geometryBoneRemapping.Length; i++)
					geometryBoneRemapping[i] = (uint)reader.ReadInt16();
			}
			geometryBoneSingleIndex = reader.ReadInt32();
		}

		static internal void Write(VertexElement element, BinaryWriter writer)
		{
			writer.Write((Int16)element.Offset);
			writer.Write((byte)element.VertexElementFormat);
			writer.Write((byte)element.VertexElementUsage);
			writer.Write((byte)element.UsageIndex);
		}

		static internal VertexElement ReadElement(ContentReader reader)
		{
			return new VertexElement(
				(int)reader.ReadInt16(),
				(VertexElementFormat)reader.ReadByte(),
				(VertexElementUsage)reader.ReadByte(),
				(int)reader.ReadByte());
		}
	}

	/// <summary>
	/// Stores animation data for a model
	/// </summary>
	public sealed class AnimationData
	{
		private readonly string name;
		private readonly int[] boneIndices;
		private readonly float[] keyframeTimes;
		private readonly byte[][] keyframeChannels; // not used at runtime
		private readonly Transform[][] keyframeChannelTransforms;
		private readonly float duration;
		internal readonly int index;
		private Stack<AnimationStreamControl> streamCache;

#if DEBUG && !XBOX360

		/// <summary></summary>
		public AnimationData(string name, int[] indices, KeyFrameData[] keyframes, float duration, float tollerance)
		{
			this.name = name;
			this.boneIndices = indices;
			this.duration = duration;

			this.keyframeTimes = new float[keyframes.Length];
			for (int i = 0; i < keyframes.Length; i++)
				keyframeTimes[i] = keyframes[i].Time;

			if (tollerance < 0)
				tollerance = 0;

			float keyframeTranslateRange = 0;

			for (int frame = 0; frame < keyframes.Length; frame++)
			{
				if (boneIndices.Length == 0)
					continue;
				Vector3 start = keyframes[frame].BoneTransforms[0].Translation;
				for (int i = 1; i < boneIndices.Length; i++)
				{
					float dist = (keyframes[frame].BoneTransforms[i].Translation - start).Length();
					keyframeTranslateRange = Math.Max(dist, keyframeTranslateRange);
				}
			}
			if (keyframeTranslateRange > 1)
				keyframeTranslateRange = (float)Math.Sqrt(keyframeTranslateRange);

			keyframeChannels = new byte[boneIndices.Length][];

			for (int i = 0; i < boneIndices.Length; i++)
			{
				MemoryStream ms = new MemoryStream();
				BinaryWriter writer = new BinaryWriter(ms);
				CompressedTransformWriter compressor = new CompressedTransformWriter(Math.Min(0.5f,tollerance * 0.01f), keyframeTranslateRange * tollerance * 0.001f, Math.Min(0.5f,tollerance * 0.005f));

				for (int frame = 0; frame < keyframes.Length; frame++)
					compressor.Write(keyframes[frame].BoneTransforms[i], writer, false);
				compressor.EndWriting(writer);
				writer.Flush();

				keyframeChannels[i] = ms.ToArray();
			}
		}

#endif

		/// <summary>
		/// Get the number of keyframes stored within this animation
		/// </summary>
		public int KeyFrameCount
		{
			get { return keyframeTimes.Length; }
		}
		/// <summary>
		/// Get the index of this animation (eg, 0 if this is the first animation in the model)
		/// </summary>
		public int AnimationIndex
		{
			get { return index; }
		}
		/// <summary>
		/// Get the number of bones used in this animation
		/// </summary>
		public int BoneCount
		{
			get { return boneIndices.Length; }
		}
		/// <summary>
		/// Gets a readonly array of bone indices for each of the bones used by this model (this array will have a length of <see cref="BoneCount"/>)
		/// </summary>
		public ReadOnlyArrayCollection<int> BoneIndices
		{
			get { return new ReadOnlyArrayCollection<int>(boneIndices); }
		}
		/// <summary>
		/// Gets a readonly array of time values for keyframes in the animation
		/// </summary>
		public ReadOnlyArrayCollection<float> KeyFrameTime
		{
			get { return new ReadOnlyArrayCollection<float>(keyframeTimes); }
		}

		internal AnimationStreamControl GetStream()
		{
			lock (boneIndices)//private member, so saves a sync object
			{
				if (streamCache == null || streamCache.Count == 0)
					return new ModelAnimationStreamControl(this);
				return streamCache.Pop();
			}
		}
		internal void CacheUnusedStream(AnimationStreamControl stream)
		{
			if (stream != null)
			{
				stream.Reset(true, false);
				lock (boneIndices)
				{
					if (streamCache == null)
						streamCache = new Stack<AnimationStreamControl>();
					streamCache.Push(stream);
				}
			}
		}
		/// <summary>
		/// <para>As animations are played, their playback streams are cached to reduce garbage collection and allocation.</para>
		/// <para>Call this method to clear the animation stream cache</para>
		/// </summary>
		public void ClearAnimationStreamCache()
		{
			streamCache.Clear();
		}
		/// <summary>
		/// <para>Returns a stream for reading compressed <see cref="Transform"/> data for an animation stream. Read bone transform data using a <see cref="CompressedTransformReader"/></para>
		/// <para>NOTE: Xen 1.5 decodes some complex animation streams at load time. If this method returns null, the <see cref="Transform"/> stream has already been decompressed (see <see cref="TryGetBoneDecompressedTransforms"/>).</para>
		/// </summary>
		public Stream GetBoneCompressedTransformStream(int boneIndex)
		{
			if (keyframeChannels[boneIndex] == null)
				return null;
			return new MemoryStream(keyframeChannels[boneIndex], false);
		}

		/// <summary>
		/// <para>Gets bone transform data that was decompressed at load time (simple animation streams are not decompressed at load time)</para>
		/// <para>Returns true if the stream was decompressed. Use <see cref="GetBoneCompressedTransformStream"/> to get the compressed transform stream</para>
		/// </summary>
		public bool TryGetBoneDecompressedTransforms(int boneIndex, out ReadOnlyArrayCollection<Transform> transforms)
		{
			transforms = ReadOnlyArrayCollection<Transform>.Empty;
			if (keyframeChannelTransforms[boneIndex] != null)
			{
				transforms = new ReadOnlyArrayCollection<Transform>(keyframeChannelTransforms[boneIndex]);
				return true;
			}
			return false;
		}

		//may be null if the keyframes were decompressed at loadtime
		internal byte[] GetBoneCompressedTransformData(int boneIndex)
		{
			return keyframeChannels[boneIndex];
		}
		//may be null if the keyframes were not decompressed at loadtime
		internal Transform[] GetBoneDecompressedTransformData(int boneIndex)
		{
			return keyframeChannelTransforms[boneIndex];
		}

		/// <summary>
		/// Gets the duration of the animation, in seconds
		/// </summary>
		public float Duration
		{
			get { return duration; }
		}

		/// <summary>
		/// Gets the name of the animation
		/// </summary>
		public string Name { get { return name; } }

#if DEBUG && !XBOX360

		internal void Write(BinaryWriter writer)
		{
			writer.Write(name ?? "");
			writer.Write(duration);
			writer.Write(boneIndices.Length);
			for (int i = 0; i < boneIndices.Length; i++)
				writer.Write((short)boneIndices[i]);

			writer.Write(keyframeTimes.Length);
			for (int i = 0; i < keyframeTimes.Length; i++)
				writer.Write(keyframeTimes[i]);

			for (int i = 0; i < this.keyframeChannels.Length; i++)
			{
				writer.Write(this.keyframeChannels[i].Length);
				writer.Write(this.keyframeChannels[i]);
			}
		}

#endif

		internal AnimationData(ContentReader reader, int index)
		{
			this.index = index;

			name = string.Intern(reader.ReadString());
			duration = reader.ReadSingle();

			int count = reader.ReadInt32();

			this.keyframeChannels = new byte[count][];
			this.keyframeChannelTransforms = new Transform[count][];
			this.boneIndices = new int[count];

			for (int i = 0; i < count; i++)
			{
				this.boneIndices[i] = (int)reader.ReadInt16();
			}

			count = reader.ReadInt32();
			this.keyframeTimes = new float[count];

			for (int i = 0; i < count; i++)
				this.keyframeTimes[i] = reader.ReadSingle();

			List<Transform> keyframes = new List<Transform>();
			
			//decode the animation streams into transforms

			for (int i = 0; i < this.keyframeChannelTransforms.Length; i++)
			{
				count = reader.ReadInt32(); // number of bytes...

#if XBOX360
				if (count > 128)
#else
				if (count > 1024)
#endif
				{
					//if the stream is complex, decode it right now

					CompressedTransformReader transformReader = new CompressedTransformReader();
					while (transformReader.MoveNext(reader))
						keyframes.Add(transformReader.value);

					//keyframeChannels[i] = reader.ReadBytes(count);
					this.keyframeChannelTransforms[i] = keyframes.ToArray();
					keyframes.Clear();
				}
				else
				{
					//otherwise, keep it compressed
					//lots of streams store the same transform for the entire stream (usually ~50% of them)
					this.keyframeChannels[i] = reader.ReadBytes(count);
				}

			}
		}

	}

#if DEBUG && !XBOX360

	/// <summary>
	/// DEBUG ONLY
	/// </summary>
	public sealed class KeyFrameData
	{
		readonly float time;
		readonly Transform[] transforms;
		
		/// <summary></summary>
		public KeyFrameData(float time, Transform[] transforms)
		{
			this.time = time;
			this.transforms = transforms;
		}
		/// <summary></summary>
		public KeyFrameData(float time, Matrix[] transforms)
		{
			this.time = time;
			this.transforms = new Transform[transforms.Length];
			for (int i = 0; i < transforms.Length; i++)
				this.transforms[i] = new Transform(ref transforms[i]);
		}

		/// <summary>
		/// Gets the local bone transforms for this keyframe
		/// </summary>
		public ReadOnlyArrayCollection<Transform> BoneTransforms
		{
			get { return new ReadOnlyArrayCollection<Transform>(transforms); }
		}

		/// <summary>Gets the time value of this keyframe</summary>
		public float Time { get { return time; } }
	}

#endif

	/// <summary>
	/// Stores data about a skeleton structure used by an animated model
	/// </summary>
	public sealed class SkeletonData
	{
		internal readonly Transform[] boneLocalTransforms, boneWorldTransforms, boneWorldTransformsInverse;
		private readonly BoneData[] boneData;
		private readonly int[] hierarchy;

#if DEBUG && !XBOX360
		internal readonly Matrix[] boneLocalMatrices;

		/// <summary></summary>
		public SkeletonData(Matrix[] boneTransforms, BoneData[] bones)
		{
			this.boneLocalMatrices = boneTransforms;
			this.boneLocalTransforms = new Transform[boneTransforms.Length];
			for (int i = 0; i < this.boneLocalTransforms.Length; i++)
				this.boneLocalTransforms[i] = new Transform(ref boneTransforms[i]);
			this.boneData = bones;

			hierarchy = new int[boneData.Length * 2];
			CreateHierarchy();

			boneWorldTransforms = BoneLocalTransform.ToArray();
			TransformHierarchy(boneWorldTransforms);
		}

#endif

		/// <summary>
		/// Transforms a hierarchy of local bone transforms into world space bone transforms
		/// </summary>
		/// <param name="transforms"></param>
		public void TransformHierarchy(Transform[] transforms)
		{
			for (int i = 0; i < hierarchy.Length; i+=2)
			{
				int parent = hierarchy[i];
				int index = hierarchy[i+1];

				if (parent != -1)
				{
#if NO_INLINE
					Transform.Multiply(ref transforms[index], ref transforms[parent], out transforms[index]);
#else
					Transform transform1 = transforms[index];
					Transform transform2 = transforms[parent];
					Quaternion q;
					Vector3 t;
					float s = transform2.Scale * transform1.Scale; ;

					if (transform2.Rotation.W == 1 &&
						(transform2.Rotation.X == 0 && transform2.Rotation.Y == 0 && transform2.Rotation.Z == 0))
					{
						q.X = transform1.Rotation.X;
						q.Y = transform1.Rotation.Y;
						q.Z = transform1.Rotation.Z;
						q.W = transform1.Rotation.W;
						t.X = transform1.Translation.X;
						t.Y = transform1.Translation.Y;
						t.Z = transform1.Translation.Z;
					}
					else
					{
						float num12 = transform2.Rotation.X + transform2.Rotation.X;
						float num2 = transform2.Rotation.Y + transform2.Rotation.Y;
						float num = transform2.Rotation.Z + transform2.Rotation.Z;
						float num11 = transform2.Rotation.W * num12;
						float num10 = transform2.Rotation.W * num2;
						float num9 = transform2.Rotation.W * num;
						float num8 = transform2.Rotation.X * num12;
						float num7 = transform2.Rotation.X * num2;
						float num6 = transform2.Rotation.X * num;
						float num5 = transform2.Rotation.Y * num2;
						float num4 = transform2.Rotation.Y * num;
						float num3 = transform2.Rotation.Z * num;
						t.X = ((transform1.Translation.X * ((1f - num5) - num3)) + (transform1.Translation.Y * (num7 - num9))) + (transform1.Translation.Z * (num6 + num10));
						t.Y = ((transform1.Translation.X * (num7 + num9)) + (transform1.Translation.Y * ((1f - num8) - num3))) + (transform1.Translation.Z * (num4 - num11));
						t.Z = ((transform1.Translation.X * (num6 - num10)) + (transform1.Translation.Y * (num4 + num11))) + (transform1.Translation.Z * ((1f - num8) - num5));

						num12 = (transform2.Rotation.Y * transform1.Rotation.Z) - (transform2.Rotation.Z * transform1.Rotation.Y);
						num11 = (transform2.Rotation.Z * transform1.Rotation.X) - (transform2.Rotation.X * transform1.Rotation.Z);
						num10 = (transform2.Rotation.X * transform1.Rotation.Y) - (transform2.Rotation.Y * transform1.Rotation.X);
						num9 = ((transform2.Rotation.X * transform1.Rotation.X) + (transform2.Rotation.Y * transform1.Rotation.Y)) + (transform2.Rotation.Z * transform1.Rotation.Z);
						q.X = ((transform2.Rotation.X * transform1.Rotation.W) + (transform1.Rotation.X * transform2.Rotation.W)) + num12;
						q.Y = ((transform2.Rotation.Y * transform1.Rotation.W) + (transform1.Rotation.Y * transform2.Rotation.W)) + num11;
						q.Z = ((transform2.Rotation.Z * transform1.Rotation.W) + (transform1.Rotation.Z * transform2.Rotation.W)) + num10;
						q.W = (transform2.Rotation.W * transform1.Rotation.W) - num9;
					}

					t.X = t.X * transform2.Scale + transform2.Translation.X;
					t.Y = t.Y * transform2.Scale + transform2.Translation.Y;
					t.Z = t.Z * transform2.Scale + transform2.Translation.Z;


					transform1.Rotation.X = q.X;
					transform1.Rotation.Y = q.Y;
					transform1.Rotation.Z = q.Z;
					transform1.Rotation.W = q.W;

					transform1.Translation.X = t.X;
					transform1.Translation.Y = t.Y;
					transform1.Translation.Z = t.Z;
					transform1.Scale = s;

					transforms[index] = transform1;
#endif
				}
			}
		}

		/// <summary>
		/// Transforms a hierarchy of local bone transforms into world space bone transforms
		/// </summary>
		/// <param name="transforms"></param>
		public void TransformHierarchy(Matrix[] transforms)
		{
			for (int i = 0; i < hierarchy.Length; i += 2)
			{
				int parent = hierarchy[i];
				int index = hierarchy[i + 1];

				if (parent != -1)
					Matrix.Multiply(ref transforms[index], ref transforms[parent], out transforms[index]);
			}
		}

		/// <summary>
		/// Applies the inverse of the <see cref="TransformHierarchy(Transform[])"/> method. (This operation is considerably slower and should not be performed at runtime)
		/// </summary>
		/// <param name="transforms"></param>
		public void TransformHierarchyInverse(Transform[] transforms)
		{
			for (int i = hierarchy.Length - 2; i >= 0; i -= 2)
			{
				int parent = hierarchy[i];
				int index = hierarchy[i + 1];

				if (parent != -1)
				{
					Matrix matrix;
					transforms[parent].GetMatrix(out matrix);
					Matrix.Invert(ref matrix, out matrix);
					Transform transform = new Transform(ref matrix);

					Transform.Multiply(ref transforms[index], ref transform, out transforms[index]);
				}
			}
		}
#if DEBUG && !XBOX360
		/// <summary>
		/// Applies the inverse of the <see cref="TransformHierarchy(Matrix[])"/> method. (This operation is considerably slower and should not be performed at runtime)
		/// </summary>
		/// <param name="transforms"></param>
		public void TransformHierarchyInverse(Matrix[] transforms)
		{
			for (int i = hierarchy.Length - 2; i >= 0; i -= 2)
			{
				int parent = hierarchy[i];
				int index = hierarchy[i + 1];

				if (parent != -1)
				{
					Matrix matrix;
					Matrix.Invert(ref transforms[parent], out matrix);
					Matrix.Multiply(ref transforms[index], ref matrix, out transforms[index]);
				}
			}
		}
#endif

		private void CreateHierarchy()
		{
			for (int i = 0; i < hierarchy.Length; i++)
				hierarchy[i] = -1;

			int index = 0;
			FillChildren(ref index,0,-1);
		}

		private void FillChildren(ref int start, int index, int parent)
		{
			hierarchy[start * 2 + 0] = parent;
			hierarchy[start * 2 + 1] = index;
			start++;

			for (int i = 0; i < boneData[index].Children.Length; i++)
				FillChildren(ref start, boneData[index].Children[i], index);
		}

		/// <summary>
		/// Gets the bone count of this skeleton
		/// </summary>
		public int BoneCount
		{
			get { return boneData.Length; }
		}
		/// <summary>
		/// Gets a readonly array of Transforms representing the local bone space transform of each bone in the skeleton
		/// </summary>
		public ReadOnlyArrayCollection<Transform> BoneLocalTransform
		{
			get { return new ReadOnlyArrayCollection<Transform>(boneLocalTransforms); }
		}
		/// <summary>
		/// Gets a readonly array of Transforms representing the world space transform of each bone in the skeleton
		/// </summary>
		public ReadOnlyArrayCollection<Transform> BoneWorldTransforms
		{
			get { return new ReadOnlyArrayCollection<Transform>(boneWorldTransforms); }
		}
		/// <summary>
		/// Gets a readonly array of Transforms representing the inverse world space transform of each bone in the skeleton
		/// </summary>
		public ReadOnlyArrayCollection<Transform> BoneWorldInverseTransforms
		{
			get { return new ReadOnlyArrayCollection<Transform>(boneWorldTransformsInverse); }
		}
#if DEBUG && !XBOX360
		/// <summary>
		/// This value will be null at runtime, and is only used at content build time
		/// </summary>
		public ReadOnlyArrayCollection<Matrix> BoneLocalMatrices
		{
			get { return new ReadOnlyArrayCollection<Matrix>(boneLocalMatrices); }
		}
#endif
		/// <summary>
		/// Gets a readonly array of BoneData for each bone (eg, data storing names and children)
		/// </summary>
		public ReadOnlyArrayCollection<BoneData> BoneData
		{
			get { return new ReadOnlyArrayCollection<BoneData>(boneData); }
		}

		/// <summary>
		/// Performs a linear search to find the bone matching the given name, -1 if not found.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public int GetBoneIndexByName(string name)
		{
			for (int i = 0; i < boneData.Length; i++)
			{
				if (boneData[i].Name == name)
					return i;
			}
			return -1;
		}

#if DEBUG && !XBOX360
		internal void Write(BinaryWriter writer, bool writeAsAvatarData)
		{
			writer.Write(boneLocalTransforms.Length);
			for (int i = 0; i < boneLocalTransforms.Length; i++)
			{
				if (!writeAsAvatarData)
					boneLocalTransforms[i].Write(writer);
				boneData[i].Write(writer);
			}
		}
#endif

		internal SkeletonData(ContentReader reader, bool loadAsAvatarData)
		{
			int count = reader.ReadInt32();

			this.boneData = new BoneData[count];
			if (!loadAsAvatarData)
				this.boneLocalTransforms = new Transform[count];

			for (int i = 0; i < count; i++)
			{
				if (!loadAsAvatarData)
					this.boneLocalTransforms[i] = new Transform(reader);
				this.boneData[i] = new BoneData(reader);
			}

			hierarchy = new int[boneData.Length * 2];
			CreateHierarchy();

			if (!loadAsAvatarData)
			{
				boneWorldTransforms = BoneLocalTransform.ToArray();
				TransformHierarchy(boneWorldTransforms);

				boneWorldTransformsInverse = new Transform[boneWorldTransforms.Length];
				for (int i = 0; i < boneWorldTransformsInverse.Length; i++)
				{
					Matrix matrix, inv;
					boneWorldTransforms[i].GetMatrix(out matrix);
					Matrix.Invert(ref matrix, out inv);
					boneWorldTransformsInverse[i] = new Transform(ref inv);
				}
			}
		}
	}

	/// <summary>
	/// Stores data about a skeleton bone
	/// </summary>
	public struct BoneData
	{
		private readonly string name;
		private readonly int boneIndex;
		private readonly int[] children;
		private readonly int parent;

#if DEBUG && !XBOX360

		/// <summary></summary>
		public BoneData(string name, int boneIndex, int parent, int[] children)
		{
			this.name = name;
			this.boneIndex = boneIndex;
			this.children = children;
			this.parent = parent;
		}

#endif
		/// <summary>
		/// Gets the name of this bone
		/// </summary>
		public string Name
		{
			get { return name; }
		}
		/// <summary>
		/// Gets the index of this bone in the skeleton
		/// </summary>
		public int Index
		{
			get { return boneIndex; }
		}
		/// <summary>
		/// Gets the index of this bone's parent in the skeleton
		/// </summary>
		public int Parent
		{
			get { return parent; }
		}

		/// <summary>
		/// Gets a readonly array of the indices of each child connected to this bone
		/// </summary>
		public ReadOnlyArrayCollection<int> Children
		{
			get { return new ReadOnlyArrayCollection<int>(children); }
		}

#if DEBUG && !XBOX360

		internal void Write(BinaryWriter writer)
		{
			if (name == null ||
				children == null)
			{
				//null bone.. skip
				writer.Write(false);
				return;
			}
			writer.Write(true);
			writer.Write(name);
			writer.Write((short)boneIndex);
			writer.Write((short)parent);
			writer.Write((short)children.Length);
			for (int i = 0; i < children.Length; i++)
				writer.Write((short)children[i]);
		}

#endif

		internal BoneData(ContentReader reader)
		{
			if (reader.ReadBoolean() == false)
			{
				this.name = null;
				this.boneIndex = -1;
				this.children = null;
				this.parent = -1;
				return;
			}

			this.name = string.Intern(reader.ReadString());
			this.boneIndex = (int)reader.ReadInt16();
			this.parent = (int)reader.ReadInt16();
			int count = (int)reader.ReadInt16();

			this.children = new int[count];
			for (int i = 0; i < count; i++)
				this.children[i] = (int)reader.ReadInt16();
		}
	}
}
