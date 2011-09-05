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
	/// Content class for an AvatarInstance imported using the xen model importer
	/// </summary>
	public sealed class AvatarAnimationData
	{
		private const int version = 1001;
		readonly private string name;
		readonly internal SkeletonData skeleton;
		readonly internal AnimationData[] animations;
		internal readonly GeometryBounds[] animationStaticBounds;
		internal GeometryBounds staticBounds;

		/// <summary>
		/// Gets the bounding box for the non-animated model data
		/// </summary>
		public GeometryBounds StaticBounds { get { return staticBounds; } }

		internal class RuntimeReader : ContentTypeReader<AvatarAnimationData>
		{
			protected override AvatarAnimationData Read(ContentReader input, AvatarAnimationData existingInstance)
			{
				return existingInstance ?? new AvatarAnimationData(input);
			}
		}

		internal static string RuntimeReaderType
		{
			get { return typeof(RuntimeReader).AssemblyQualifiedName; }
		}

#if DEBUG && !XBOX360

		/// <summary></summary>
		public AvatarAnimationData(string name, MeshData[] meshes, SkeletonData skeleton, AnimationData[] animations)
		{
			this.name = name;
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

		/// <summary>
		/// Name of the model
		/// </summary>
		public string Name { get { return name; } }
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

		internal AvatarAnimationData(ContentReader reader)
		{
			int fileVersion = reader.ReadInt32();
			if (version != fileVersion)
				throw new InvalidOperationException("Serialiezd AvatarAnimationData version mismatch");
			this.name = string.Intern(reader.ReadString());
			
			this.skeleton = new SkeletonData(reader, true);

			int count = reader.ReadInt32();
			this.animations = new AnimationData[count];
			for (int i = 0; i < count; i++)
				this.animations[i] = new AnimationData(reader, i);

			this.staticBounds = new GeometryBounds(reader);

			count = (int)reader.ReadInt16();
			animationStaticBounds = new GeometryBounds[count];
			for (int i = 0; i < animationStaticBounds.Length; i++)
				animationStaticBounds[i] = new GeometryBounds(reader);
		}

#if DEBUG && !XBOX360

		internal void Write(BinaryWriter writer)
		{
			writer.Write(version);
			writer.Write(name ?? "");

			skeleton.Write(writer, true);

			writer.Write(animations.Length);
			foreach (AnimationData animation in animations)
				animation.Write(writer);

			this.staticBounds.Write(writer);

			writer.Write((short)animationStaticBounds.Length);
			for (int i = 0; i < animationStaticBounds.Length; i++)
				animationStaticBounds[i].Write(writer);
		}

#endif
	}
}
