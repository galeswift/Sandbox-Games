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

namespace Xen.Ex.Graphics
{
	//this namespace is mostly used for the particle system, but here it'll be used for a base class for animation.
	namespace Processor
	{
		/// <summary>
		/// Abstract base class for an animation processor for models and avatars
		/// </summary>
		public abstract class AnimationProcessor : IUpdate, IDisposable, IAction
		{
			internal Transform[] transformedBones;
			internal AnimationTransformArray transformOuput;
			internal bool[] transformIdentity;
			internal readonly List<AnimationStreamControl> animations;

			private readonly List<AnimationStreamControl> threadAnimations;
			private bool disposed, wasDrawn, wasSkipped, isIdentityOutput;
			private readonly List<WeakReference> parents;
			private float deltaTime;
			private WaitCallback waitCallback;
			private int frameIndex, boundsFrameIndex;
			private bool isAsync;
			internal Vector3 boundsMin, boundsMax;

			internal AnimationProcessor(UpdateManager manager, object parent)
			{
				if (manager != null)
				{
					manager.Add(this);
					isAsync = true;

					if (parent != null)
					{
						parents = new List<WeakReference>();
						parents.Add(new WeakReference(parent));
					}

					threadAnimations = new List<AnimationStreamControl>();
				}
				animations = new List<AnimationStreamControl>();

				wasDrawn = true;
			}


			/// <summary></summary>
			public bool IsDisposed
			{
				get { return disposed; }
			}

			/// <summary>Number of animations stored in this controller</summary>
			public abstract int AnimationCount { get; }

			/// <summary>
			/// <para>Clears <i>all</i> global cached animations for this <see cref="ModelData"/>. The cache helps reduce allocation/garbage build up</para>
			/// <para>Note: This purge will effect all <see cref="AnimationController"/> instances for the current <see cref="ModelData"/> Content</para>
			/// </summary>
			public abstract void PurgeAnimationStreamCaches();

			/// <summary></summary>
			public abstract AnimationData GetAnimationData(int index);

			/// <summary>
			/// <para>Gets an animation index by animation string name. -1 if not found.</para>
			/// <para>Performs a linear search of animations in the animation list</para>
			/// </summary>
			/// <param name="name"></param>
			/// <returns></returns>
			public abstract int AnimationIndex(string name);

			/// <summary></summary>
			protected abstract bool ContentLoaded { get; }

			/// <summary>
			/// <para>Gets the transformed bones for this animation controller (Bones are represented in bone world space, not model world space - see remarks for details).</para>
			/// <para>Note: For off-screen models or Avatars, calling this method will force transform computation. For Async controllers, this method may cause the thread to wait for the animation processing to complete</para>
			/// </summary>
			/// <param name="state"></param>
			/// <returns></returns>
			/// <remarks>
			/// <para>
			/// Bone transforms are in transformed bone space. To Get the exact world transform of a bone, multiply <see cref="SkeletonData.BoneWorldTransforms"/> by the transformed bones.
			/// </para>
			/// </remarks>
			public ReadOnlyArrayCollection<Transform> GetTransformedBones(DrawState state)
			{
				WaitForAsyncAnimation(state, state.Properties.FrameIndex, true);
				return new ReadOnlyArrayCollection<Transform>(transformedBones);
			}

			internal void AddParnet(object parent)
			{
				if (this.parents != null)
					this.parents.Add(new WeakReference(parent));
			}

			/// <summary>
			/// Plays an animation that loops continuously, returning a <see cref="AnimationInstance"/> structure
			/// </summary>
			/// <param name="animationData"></param>
			/// <returns></returns>
			/// <param name="fadeInTime">Time, in seconds, to fade the animation in</param>
			protected AnimationInstance PlayLoopingAnimationData(AnimationData animationData, float fadeInTime)
			{
				if (animationData == null)
					throw new ArgumentNullException();
				if (fadeInTime < 0)
					throw new ArgumentException("fadeInTime");
				if (!ContentLoaded)
					throw new InvalidOperationException("ModelInstance / AvatarInstance content has not been added");
				if (disposed)
					throw new ObjectDisposedException("this");
				AnimationStreamControl control = animationData.GetStream();
				control.Initalise(true, fadeInTime, 0);
				animations.Add(control);
				return new AnimationInstance(control);
			}

			/// <summary>
			/// Plays an animation, returning a <see cref="AnimationInstance"/> structure
			/// </summary>
			/// <param name="animationData"></param>
			/// <param name="fadeInTime">Time, in seconds, to fade the animation in</param>
			/// <param name="fadeOutTime">Time, in seconds, to fade the animation out</param>
			/// <returns></returns>
			protected AnimationInstance PlayAnimationData(AnimationData animationData, float fadeInTime, float fadeOutTime)
			{
				if (animationData == null)
					throw new ArgumentNullException();
				if (fadeInTime < 0)
					throw new ArgumentException("fadeInTime");
				if (fadeOutTime < 0)
					throw new ArgumentException("fadeOutTime");
				if (!ContentLoaded)
					throw new InvalidOperationException("ModelInstance / AvatarInstance content has not been added");
				if (disposed)
					throw new ObjectDisposedException("this");
				AnimationStreamControl control = animationData.GetStream();
				control.Initalise(false, fadeInTime, fadeOutTime);
				animations.Add(control);
				return new AnimationInstance(control);
			}

			UpdateFrequency IUpdate.Update(UpdateState state)
			{
				if (disposed)
					return UpdateFrequency.Terminate;

				if (!wasDrawn)
				{
					bool alive = false;
					foreach (WeakReference wr in this.parents)
					{
						if (wr.IsAlive)
						{
							alive = true;
							break;
						}
					}
					if (!alive)
					{
						this.parents.Clear();
						this.isAsync = false;
						return UpdateFrequency.Terminate;
					}
				}

				this.deltaTime = state.DeltaTimeSeconds;

				if (wasDrawn && ContentLoaded)
				{
					BufferAnimationValues();
					this.waitCallback = state.Application.ThreadPool.QueueTask(this, null);
				}
				else
					wasSkipped = true;
				wasDrawn = false;

				//using async here would be slower...
				return UpdateFrequency.OncePerFrame;
			}

			/// <summary></summary>
			protected internal abstract void ComputeAnimationBounds();

			/// <summary></summary>
			protected abstract void OnBufferAnimationValues();

			private void BufferAnimationValues()
			{
				//buffer the animation values, such as current time, so they are thread safe when used in ProcessAnimation()

				OnBufferAnimationValues();

				for (int a = 0; a < animations.Count; a++)
				{
					AnimationStreamControl anim = animations[a];

					if (!anim.SetupTimingInformation(deltaTime, true))
					{
						if (anim is ModelAnimationStreamControl)
							(anim as ModelAnimationStreamControl).Animation.CacheUnusedStream(anim as AnimationStreamControl);
						animations.RemoveAt(a);
						a--;
						continue;
					}
				}

				if (!isAsync)
					return; // no need to go further when not on a thread

				threadAnimations.Clear();

				for (int a = 0; a < animations.Count; a++)
				{
					AnimationStreamControl anim = animations[a];
					if (anim.Enabled == false)
						continue;

					threadAnimations.Add(anim);
				}
			}

			/// <summary>
			/// Called before animation processing
			/// </summary>
			protected abstract bool OnBeginProcessAnimation();
			/// <summary>
			/// Called after animation processing
			/// </summary>
			protected abstract void OnEndProcessAnimation();
			/// <summary></summary>
			protected abstract void ResetTransforms();

			internal void ProcessAnimation(float delta)
			{
				if (!OnBeginProcessAnimation())
					return;

				ResetTransforms();

				bool isIdentity = true;

				List<AnimationStreamControl> animations = this.animations ?? this.threadAnimations;

				for (int a = 0; a < animations.Count; a++)
				{
					ModelAnimationStreamControl anim = animations[a] as ModelAnimationStreamControl;
					
					if (animations[a].RuntimeWeight == 0)
						continue;

					isIdentity = false;

					animations[a].Interpolate();

					if (anim != null)
					{
						ModelAnimationStreamControl.AnimationChannel[] channels = anim.channels;

						for (int i = 0; i < channels.Length; i++)
						{
							int bi = channels[i].boneIndex;

							Transform transform1 = transformedBones[bi];
							Transform transform2 = channels[i].lerpedTransform;
							Quaternion q;
							Vector3 t;
							float s = transform2.Scale * transform1.Scale;

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

							//normalize
							float len = q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;
							if (len != 1)
							{
								len = 1f / ((float)Math.Sqrt((double)len));
								q.X *= len;
								q.Y *= len;
								q.Z *= len;
								q.W *= len;
							}

							transform1.Rotation.X = q.X;
							transform1.Rotation.Y = q.Y;
							transform1.Rotation.Z = q.Z;
							transform1.Rotation.W = q.W;

							transform1.Translation.X = t.X;
							transform1.Translation.Y = t.Y;
							transform1.Translation.Z = t.Z;
							transform1.Scale = s;

							transformedBones[bi] = transform1;
						}
					}
				}

				isIdentityOutput = isIdentity;
				OnEndProcessAnimation();
			}

			internal AnimationTransformArray WaitForAsyncAnimation(IState state, int frameIndex, bool requiresBoneData)
			{
				if (state == null)
					throw new ArgumentNullException();

				this.wasDrawn |= requiresBoneData;

				if (frameIndex != this.boundsFrameIndex)
				{
					ComputeAnimationBounds();

					this.boundsFrameIndex = frameIndex;
				}
				if (requiresBoneData && frameIndex != this.frameIndex)
				{
					if (isAsync)
						this.waitCallback.WaitForCompletion();
					if (!isAsync || wasSkipped)
					{
						this.wasSkipped = false;
						this.deltaTime = state.DeltaTimeSeconds;

						this.BufferAnimationValues();
						this.ProcessAnimation(this.deltaTime);
					}
					if (isIdentityOutput)
						this.transformOuput.ClearTransformArray();
					else
						this.transformOuput.UpdateTransformArray(this.transformedBones);
					this.frameIndex = frameIndex;
				}

				return requiresBoneData && !this.transformOuput.IsIdentiyState ? this.transformOuput : null;
			}

			/// <summary></summary>
			public void Dispose()
			{
				foreach (AnimationStreamControl stream in this.animations)
					if (stream is ModelAnimationStreamControl)
						(stream as ModelAnimationStreamControl).Animation.CacheUnusedStream(stream);
				this.animations.Clear();
				disposed = true;
			}

			void IAction.PerformAction(object data)
			{
				ProcessAnimation(this.deltaTime);
			}
		}

	}

	/// <summary>
	/// <para>Interface to a class that may modify the transforms of a models animation bone hierarchy, through a <see cref="AnimationController"/></para>
	/// <para>Note: Methods implemented for this interface should be thread safe</para>
	/// </summary>
	public interface IAnimationBoneModifier
	{
		/// <summary>
		/// <para>Modify the bones of the animation before the animation is processed. Returning false will prevent the animation / blending process from starting.</para>
		/// <para>Use this method to replace the entire animation, and prevent the standard animations from being processed.</para>
		/// <para>Any modifications to <paramref name="nonAnimatedBones"/> will have no effect unless false is returned.</para>
		/// </summary>
		/// <param name="nonAnimatedBones"></param>
		/// <param name="modelData"></param>
		/// <param name="boneWorldSpaceIdentityTransforms"><para>The world space transforms of the bones</para><para>If no animations are playing, all bones will be Identity transforms. These transforms are their world space default transforms</para></param>
		/// <param name="boneWorldSpaceIdentityInverseTransforms"></param>
		/// <returns></returns>
		bool PreProcessAnimation(Transform[] nonAnimatedBones, ModelData modelData, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityTransforms, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityInverseTransforms);
		/// <summary>
		/// <para>Modify the animation bones of a mesh, before the bones are transformed into bone-world space hierarchy</para>
		/// <para>Return true to have the continue with the animation hierarchy transform, return false if the animation is completed processing</para>
		/// </summary>
		/// <param name="boneSpaceTransforms"></param>
		/// <param name="boneWorldSpaceIdentityTransforms"><para>The world space transforms of the bones</para><para>If no animations are playing, all bones will be Identity transforms. These transforms are their world space default transforms</para></param>
		/// <param name="boneWorldSpaceIdentityInverseTransforms"></param>
		/// <param name="modelData"></param>
		/// <returns>Return true to have the continue with the animation hierarchy transform, return false if the animation is completed processing</returns>
		bool ProcessBonesPreTransform(Transform[] boneSpaceTransforms, ModelData modelData, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityTransforms, ReadOnlyArrayCollection<Transform> boneWorldSpaceIdentityInverseTransforms);

		/// <summary>
		/// Modify the animation bones of a mesh, after they have been transformed into world space hierarchy,
		/// <para>This is the final format of the bones, before they are converted be used to render the model</para>
		/// </summary>
		void ProcessBonesPostTransform(Transform[] worldSpaceTransforms, ModelData modelData);
	}

	/// <summary>
	/// Controls animation streams and calulate transformed bone structures for a model
	/// </summary>
	public sealed class AnimationController : Processor.AnimationProcessor
	{
		private ModelData modelData;
		private IAnimationBoneModifier boneModifier, boneModifierBuffer;

		internal AnimationController(ModelData model, UpdateManager manager, ModelInstance parent)
			: base(manager, parent)
		{
			this.SetModelData(model);
		}

		/// <summary>
		/// Gets/Sets an animation modifier that can modify animation bones before and after the bones are transformed into a hierarchy
		/// </summary>
		public IAnimationBoneModifier AnimationBoneModifier
		{
			get { return boneModifier; }
			set { boneModifier = value; }
		}

		/// <summary>
		/// Plays an animation that loops continuously, returning a <see cref="AnimationInstance"/> structure
		/// </summary>
		/// <param name="animationIndex"></param>
		/// <returns></returns>
		public AnimationInstance PlayLoopingAnimation(int animationIndex)
		{
			return PlayLoopingAnimation(animationIndex, 0);
		}
		/// <summary>
		/// Plays an animation that loops continuously, returning a <see cref="AnimationInstance"/> structure
		/// </summary>
		/// <param name="animationIndex"></param>
		/// <returns></returns>
		/// <param name="fadeInTime">Time, in seconds, to fade the animation in</param>
		public AnimationInstance PlayLoopingAnimation(int animationIndex, float fadeInTime)
		{
			if (animationIndex == -1)
				throw new ArgumentException();
			if (ModelData == null)
				throw new InvalidOperationException("ModelInstance.ModelData == null");
			return this.PlayLoopingAnimationData(this.ModelData.animations[animationIndex], fadeInTime);
		}
		/// <summary>
		/// Plays an animation, returning a <see cref="AnimationInstance"/> structure
		/// </summary>
		/// <param name="animationIndex"></param>
		/// <returns></returns>
		public AnimationInstance PlayAnimation(int animationIndex)
		{
			return PlayAnimation(animationIndex, 0, 0);
		}
		/// <summary>
		/// Plays an animation, returning a <see cref="AnimationInstance"/> structure
		/// </summary>
		/// <param name="animationIndex"></param>
		/// <param name="fadeInTime">Time, in seconds, to fade the animation in</param>
		/// <param name="fadeOutTime">Time, in seconds, to fade the animation out</param>
		/// <returns></returns>
		public AnimationInstance PlayAnimation(int animationIndex, float fadeInTime, float fadeOutTime)
		{
			if (animationIndex == -1)
				throw new ArgumentException();
			if (ModelData == null)
				throw new InvalidOperationException("ModelInstance.ModelData == null");
			return PlayAnimationData(this.ModelData.animations[animationIndex], fadeInTime, fadeOutTime);
		}

		/// <summary></summary>
		protected override void ResetTransforms()
		{
			SkeletonData skeleton = this.modelData.skeleton;
			for (int i = 0; i < transformedBones.Length; i++)
				transformedBones[i] = skeleton.boneLocalTransforms[i];
		}

		internal ModelData ModelData { get { return modelData; } }

		//abstract class implementation

		/// <summary>
		/// <para>Gets an animation index by animation string name. -1 if not found.</para>
		/// <para>Performs a linear search of animations in the <see cref="ModelData"/></para>
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public override int AnimationIndex(string name)
		{
			if (this.modelData == null)
				return -1;
			for (int i = 0; i < this.modelData.animations.Length; i++)
			{
				if (this.modelData.animations[i].Name == name)
					return i;
			}
			return -1;
		}

		/// <summary>Number of animations stored in this controller</summary>
		public override int AnimationCount
		{
			get
			{
				if (IsDisposed)
					throw new ObjectDisposedException("this");
				if (modelData == null)
					return 0;
				return modelData.animations.Length;
			}
		}

		/// <summary></summary>
		protected override void OnBufferAnimationValues()
		{
			this.boneModifierBuffer = boneModifier;
		}

		/// <summary>
		/// <para>Clears <i>all</i> global cached animations for this <see cref="ModelData"/>. The cache helps reduce allocation/garbage build up</para>
		/// <para>Note: This purge will effect all <see cref="AnimationController"/> instances for the current <see cref="ModelData"/> Content</para>
		/// </summary>
		public override void PurgeAnimationStreamCaches()
		{
			if (modelData == null)
				throw new InvalidOperationException("ModelData == null");

			if (modelData.animations != null)
				for (int i = 0; i < modelData.animations.Length; i++)
					modelData.animations[i].ClearAnimationStreamCache();
		}
		
		
		
		/// <summary></summary>
		public override AnimationData GetAnimationData(int index)
		{
			if (IsDisposed)
				throw new ObjectDisposedException("this");
			return modelData.animations[index];
		}
		
		
		internal void SetModelData(ModelData model)
		{
			if (model != null && this.modelData == null)
			{
				if (model.skeleton == null)
					throw new InvalidOperationException("Unable to initalise the AnimationController assigned to this ModelInstance / AvatarInstance. ModelData requires a skeleton to be animated. This model cannot have an AnimationController assigned to it");

				this.modelData = model;

				transformedBones = new Transform[model.Skeleton.BoneCount];
				transformIdentity = new bool[model.Skeleton.BoneCount];

				for (int i = 0; i < model.Skeleton.BoneCount; i++)
					transformedBones[i] = model.Skeleton.BoneLocalTransform[i];

				this.transformOuput = new AnimationTransformArray(transformedBones);
			}
		}



		/// <summary></summary>
		protected override bool ContentLoaded
		{
			get { return modelData != null; }
		}



		/// <summary></summary>
		protected internal override void ComputeAnimationBounds()
		{
			boundsMin = this.modelData.staticBounds.minimum;
			boundsMax = this.modelData.staticBounds.maximum;

			for (int a = 0; a < animations.Count; a++)
			{
				ModelAnimationStreamControl anim = (ModelAnimationStreamControl)animations[a];
				if (anim.Enabled)
				{
					int animIndex = anim.Animation.index;

					Vector3 value = modelData.animationStaticBounds[animIndex].minimum;
					boundsMin.X += value.X * anim.Weighting;
					boundsMin.Y += value.Y * anim.Weighting;
					boundsMin.Z += value.Z * anim.Weighting;

					value = modelData.animationStaticBounds[animIndex].maximum;
					boundsMax.X += value.X * anim.Weighting;
					boundsMax.Y += value.Y * anim.Weighting;
					boundsMax.Z += value.Z * anim.Weighting;
				}
			}
		}

		//computes the animation bounds for the mesh
		internal void ComputeMeshBounds(int meshIndex, out Vector3 boundsMin, out Vector3 boundsMax)
		{
			MeshData mesh = modelData.meshes[meshIndex];
			boundsMin = mesh.staticBounds.minimum;
			boundsMax = mesh.staticBounds.maximum;

			for (int a = 0; a < animations.Count; a++)
			{
				ModelAnimationStreamControl anim = (ModelAnimationStreamControl)animations[a];
				if (anim.Enabled)
				{
					int animIndex = anim.Animation.index;

					Vector3 value = mesh.animationStaticBounds[animIndex].minimum;
					boundsMin.X += value.X * anim.Weighting;
					boundsMin.Y += value.Y * anim.Weighting;
					boundsMin.Z += value.Z * anim.Weighting;

					value = mesh.animationStaticBounds[animIndex].maximum;
					boundsMax.X += value.X * anim.Weighting;
					boundsMax.Y += value.Y * anim.Weighting;
					boundsMax.Z += value.Z * anim.Weighting;
				}
			}
		}

		//computes the animation bounds for geometry in a mesh
		internal void ComputeGeometryBounds(int meshIndex, int geometryIndex, out Vector3 boundsMin, out Vector3 boundsMax)
		{
			GeometryData geometry = modelData.meshes[meshIndex].geometry[geometryIndex];
			boundsMin = geometry.staticBounds.minimum;
			boundsMax = geometry.staticBounds.maximum;

			for (int a = 0; a < animations.Count; a++)
			{
				ModelAnimationStreamControl anim = (ModelAnimationStreamControl)animations[a];
				float weighting = anim.Weighting;
				if (anim.Enabled)
				{
					int animIndex = anim.Animation.index;

					Vector3 value = geometry.animationStaticBounds[animIndex].minimum;
					boundsMin.X += value.X * weighting;
					boundsMin.Y += value.Y * weighting;
					boundsMin.Z += value.Z * weighting;

					value = geometry.animationStaticBounds[animIndex].maximum;
					boundsMax.X += value.X * weighting;
					boundsMax.Y += value.Y * weighting;
					boundsMax.Z += value.Z * weighting;
				}
			}
		}

		/// <summary></summary>
		protected override bool OnBeginProcessAnimation()
		{
			if (boneModifierBuffer != null)
			{
				if (!boneModifierBuffer.PreProcessAnimation(transformedBones, this.modelData,
					new ReadOnlyArrayCollection<Transform>(this.modelData.skeleton.boneWorldTransforms),
					new ReadOnlyArrayCollection<Transform>(this.modelData.skeleton.boneWorldTransformsInverse)))
					return false;
			}
			return true;
		}

		/// <summary></summary>
		protected override void OnEndProcessAnimation()
		{
			if (boneModifierBuffer != null)
			{
				if (!boneModifierBuffer.ProcessBonesPreTransform(transformedBones, this.modelData,
					new ReadOnlyArrayCollection<Transform>(this.modelData.skeleton.boneWorldTransforms),
					new ReadOnlyArrayCollection<Transform>(this.modelData.skeleton.boneWorldTransformsInverse)))
					return;
			}

			modelData.skeleton.TransformHierarchy(transformedBones);

			if (boneModifierBuffer != null)
				boneModifierBuffer.ProcessBonesPostTransform(transformedBones, this.modelData);

			//revert the bones out of world space,
			Transform transform1, transform2;
			Quaternion q;
			Vector3 t;
			Transform[] boneWorldInv = this.modelData.skeleton.boneWorldTransformsInverse;

			for (int i = 0; i < transformedBones.Length; i++)
			{
				transform1 = boneWorldInv[i];
				transform2 = transformedBones[i];
				float s = transform2.Scale * transform1.Scale;

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

				//normalize
				float len = q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;
				if (len != 1)
				{
					len = 1f / ((float)Math.Sqrt((double)len));
					q.X *= len;
					q.Y *= len;
					q.Z *= len;
					q.W *= len;
				}

				transform1.Rotation.X = q.X;
				transform1.Rotation.Y = q.Y;
				transform1.Rotation.Z = q.Z;
				transform1.Rotation.W = q.W;

				transform1.Translation.X = t.X;
				transform1.Translation.Y = t.Y;
				transform1.Translation.Z = t.Z;
				transform1.Scale = s;

				transformedBones[i] = transform1;
			}
		}
	}

	/// <summary>
	/// Handle to an animation that is playing through an <see cref="AnimationController"/>
	/// </summary>
	public struct AnimationInstance
	{
		readonly private AnimationStreamControl control;
		readonly private int usageIndex;

		internal AnimationInstance(AnimationStreamControl control)
		{
			this.control = control;
			this.usageIndex = control.UsageIndex;
		}

		/// <summary>
		/// Returns true if this animation instance exists
		/// </summary>
		public bool ValidAnimation { get { return control != null; } }
		/// <summary>
		/// True if the animation has finished playing, any changes made will have no effect if true.
		/// </summary>
		public bool AnimationFinished { get { return control == null || usageIndex != control.UsageIndex; } }
		/// <summary>
		/// Playback speed multiplier. 1.0f is the default, for normal playback speed. Set to 0.5f for half speed, etc.
		/// </summary>
		public float PlaybackSpeed 
		{
			get { if (AnimationFinished) return 0; return control.AnimationSpeed; } 
			set { if (AnimationFinished) return; control.AnimationSpeed = value; }
		}
		/// <summary></summary>
		public string AnimationName { get { return control.AnimationName; } }
		/// <summary>
		/// Current playback time in the duration of this animation instance (seconds)
		/// </summary>
		public float Time { get { if (AnimationFinished) return AnimationDuration; return control.FrameTimer; } }
		/// <summary>
		/// <para>Duration of the animation, in seconds</para>
		/// <para>This value may be slightly larger if <see cref="LoopTransitionEnabled"/> is true</para>
		/// </summary>
		public float AnimationDuration 
		{
			get { if (AnimationFinished) return 0; return control.Duration(); }
		}
		/// <summary>
		/// <para>The weighting of this animation instance. 1.0f by default.</para>
		/// <para>When set to 0.0, the animation will have no effect. If set to 0.5f, the animation will only have a 50 percent effect on rotations, translations, etc.</para>
		/// <para>Modify this value to fade animations in and out</para>
		/// </summary>
		public float Weighting
		{
			get { if (AnimationFinished) return 0; return control.Weighting; }
			set { if (AnimationFinished) return; control.Weighting = value; }
		}
		/// <summary>
		/// <para>The weighting of this animation instance, multiplied by the fadein/fadeout value of the animation.</para>
		/// </summary>
		public float GetFadeScaledWeighting(IState state)
		{
			if (AnimationFinished) 
				return 0; 
			return control.GetWeightedScale(state);
		}
		/// <summary></summary>
		public bool Enabled
		{
			get { if (AnimationFinished) return false; return control.Enabled; }
			set { if (AnimationFinished) return; control.Enabled = value; }
		}
		/// <summary></summary>
		public bool Looping
		{
			get { if (AnimationFinished) return false; return control.Looping; ; }
		}
		/// <summary>
		/// <para>If true, the animation will smoothly transition from the last frame to the first during a loop.</para>
		/// <para>Defaults to true for looping animations</para>
		/// </summary>
		public bool LoopTransitionEnabled
		{
			get { if (AnimationFinished) return false; return control.LoopTransition; }
			set
			{
				if (AnimationFinished || !control.Looping) 
					return;
				if (control is AvatarAnimationStreamControl)
					throw new InvalidOperationException("LoopTransitionEnabled cannot be set for a preset avatar animation");
				control.LoopTransition = value; 
			}
		}

		/// <summary>
		/// Seeks the animation to a new time. This may require decoding extra animation frames
		/// </summary>
		/// <param name="animationTime"></param>
		/// <remarks>True if the seek was successful</remarks>
		public bool SeekAnimation(float animationTime)
		{
			if (AnimationFinished)
				return false;
			control.Seek(animationTime);
			return true;
		}

		/// <summary></summary>
		public bool StopAnimation()
		{
			return StopAnimation(0);
		}
		/// <summary></summary>
		public bool StopAnimation(float fadeOutTime)
		{
			if (fadeOutTime < 0)
				throw new ArgumentException();

			if (AnimationFinished)
				return false;
			if (fadeOutTime > 0)
				control.FadeOutStop(fadeOutTime);
			else
				control.FadeOutStop(-1);
			return true;
		}
	}


	internal abstract class AnimationStreamControl
	{
		private float frameTimer, speed;
		private int usageIndex;
		private float weighting, scale;
		private bool enabled;
		private bool looping = true, loopTransition = true;
		private float fadeInTime, fadeOutTime, fadeOutStopTime, fadeOutStart;

		public float Weighting { get { return weighting; } set { weighting = value; } }
		public float FrameTimer { get { return frameTimerBuffer; } }
		public int UsageIndex { get { return usageIndex; } }
		public bool Enabled { get { return enabled; } set { enabled = value; } }
		public bool Looping { get { return looping; } }
		public bool LoopTransition { get { return loopTransition; } set { loopTransition = value; } }
		public float FadeInTime { get { return fadeInTime; } }
		public float FadeOutTime { get { return fadeOutTime; } }

		public void Initalise(bool looping, float fadeIn, float fadeOut)
		{
			this.looping = looping;
			this.loopTransition = true;
			this.enabled = true;
			this.speed = 1;
			this.frameTimer = 0;
			this.weighting = 1;
			this.weightedScaleCalcTick = -1;
			this.fadeOutStopTime = 0;
			this.fadeOutStart = 0;
			this.fadeInTime = fadeIn;
			this.fadeOutTime = fadeOut;
			this.scale = 1;
			this.frameTimeBuffer = 0;
			this.frameTimerBuffer = 0;
			this.durationBuffer = 0;
		}

		public float AnimationSpeed
		{
			get { return speed; }
			set { if (value < 0) throw new ArgumentException("Negative values are not supported"); speed = value; }
		}
		public abstract string AnimationName { get; }

		internal AnimationStreamControl()
		{
			speed = 1;
			weighting = 1;
			scale = 1;
			enabled = true;

			Reset(true,false);
		}

		virtual internal void Reset(bool newUsage, bool keepStored)
		{
			if (newUsage)
			{
				usageIndex++;
			}
		}

		public void Seek(float time)
		{
			this.frameTimer = time;
			weightedScaleCalcTick = -1;
		}

		public void FadeOutStop(float time)
		{
			if (fadeOutStart == 0)
			{
				this.fadeOutStopTime = time;
				this.fadeOutStart = this.frameTimer;
				weightedScaleCalcTick = -1;
			}

			if (time <= 0)
				this.usageIndex++;
		}

		//thread buffered values
		protected float frameTimeBuffer, frameTimerBuffer, weightingBuffer, durationBuffer, scaleBuffer, runtimeWeight;
		protected bool loopTransitionBuffer, loopingBuffer;

		private float weightedScale;
		private long weightedScaleCalcTick = -1;

		public float GetWeightedScale(IState state)
		{
			if (weightedScaleCalcTick != state.TotalTimeTicks)
			{
				SetupTimingInformation(0, false);
				weightedScale = scale * weighting;
				weightedScaleCalcTick = state.TotalTimeTicks;
			}
			return weightedScale; 
		}
		internal float WeightedScale { get { return weightedScale; } }
		internal float RuntimeWeight { get { return runtimeWeight; } }

		public bool SetupTimingInformation(float deltaTime, bool writeValues)
		{
			float frameTime = 0;
			float duration = Duration();

			//not looping? stop anim
			if (!looping && frameTimer >= duration)
			{
				if (writeValues)
				{
					this.frameTimer = 0;
					this.usageIndex++;
				}
				return false;
			}

			if (duration != 0)
				frameTime = (float)(frameTimer - Math.Floor(frameTimer / duration) * duration);

			scale = 1;

			if (fadeInTime != 0)
				scale *= Math.Max(0.0f, Math.Min(1.0f, frameTime / fadeInTime));
			if (fadeOutTime != 0)
				scale *= Math.Max(0.0f, Math.Min(1.0f, (duration - frameTime) / fadeOutTime));

			if (fadeOutStopTime != 0)
			{
				if (writeValues && fadeOutStopTime == -1)
				{
					fadeOutStopTime = 0;
					fadeOutStart = 0;
					this.usageIndex++;
					return false;
				}
				scale *= 1 - (this.frameTimer - this.fadeOutStart) / fadeOutStopTime;
				if (writeValues && this.frameTimer >= this.fadeOutStart + this.fadeOutStopTime)
				{
					fadeOutStopTime = 0;
					fadeOutStart = 0;
					this.usageIndex++;
					return false;
				}
			}

			if (scale < 0)
				scale = 0;

			if (writeValues)
			{
				frameTimerBuffer = frameTimer;
				frameTimeBuffer = frameTime;
				weightingBuffer = weighting;

				durationBuffer = duration;
				scaleBuffer = scale;

				loopTransitionBuffer = loopTransition;
				loopingBuffer = looping;

				frameTimer += deltaTime * speed;
			}

			runtimeWeight = weightingBuffer * scaleBuffer;
			if (!enabled)
				runtimeWeight = 0;

			return true;
		}

		//this method potentially runs on a thread task
		public abstract void Interpolate();

		internal abstract float Duration();
	}


	internal sealed class ModelAnimationStreamControl : AnimationStreamControl
	{
		internal struct AnimationChannel
		{
			public Transform storeTransform, lerpedTransform;
			public CompressedTransformReader frameReader;
			public byte[] sourceData;
			public Transform[] sourceTransformData;
			public int readIndex;
			public int boneIndex;
		}

		private readonly AnimationData animation;
		internal readonly AnimationChannel[] channels;

		private int readFrameIndex;
		private float currentFrameTime, previousFrameTime;

		public AnimationData Animation { get { return animation; } }
		public override string AnimationName { get { return animation.Name; } }

		internal ModelAnimationStreamControl(AnimationData animation)
		{
			channels = new AnimationChannel[animation.BoneCount];
			this.animation = animation;

			Reset(true, false);
		}

		internal override void Reset(bool newUsage, bool keepStored)
		{
			base.Reset(newUsage, keepStored);

			readFrameIndex = -1;
			currentFrameTime = 0;
			previousFrameTime = 0;

			if (channels != null)
			{
				for (int i = 0; i < channels.Length; i++)
				{
					if (!keepStored)
					{
						channels[i] = new AnimationChannel();
						channels[i].storeTransform = Transform.Identity;
					}
					channels[i].frameReader = new CompressedTransformReader();
					channels[i].lerpedTransform = Transform.Identity;
					channels[i].sourceData = animation.GetBoneCompressedTransformData(i);
					channels[i].sourceTransformData = animation.GetBoneDecompressedTransformData(i);
					channels[i].boneIndex = animation.BoneIndices[i];
					channels[i].readIndex = 0;
				}
			}
		}

		//this method potentially runs on a thread task
		public override void Interpolate()
		{
			float frameTime = frameTimeBuffer;
			float weighting = weightingBuffer;
			float duration = durationBuffer;
			float scale = scaleBuffer;
			bool loopTransition = loopTransitionBuffer;
			bool looping = loopingBuffer;

			while (true)
			{
				if (readFrameIndex == -1)
				{
					for (int i = 0; i < channels.Length; i++)
					{
						if (channels[i].sourceData != null)
							channels[i].frameReader.MoveNext(channels[i].sourceData, ref channels[i].readIndex);
						else
							channels[i].frameReader.value = channels[i].sourceTransformData[0];
					}
					readFrameIndex = 0;
					currentFrameTime = 0;
					previousFrameTime = 0;
				}

				if (frameTime <= currentFrameTime &&
					frameTime >= previousFrameTime)
					break;

				if (frameTime > currentFrameTime)
				{
					//move to next frame
					previousFrameTime = currentFrameTime;

					if (readFrameIndex != animation.KeyFrameCount - 1)
					{
						for (int i = 0; i < channels.Length; i++)
						{
							channels[i].storeTransform = channels[i].frameReader.value;
							if (channels[i].sourceData != null)
								channels[i].frameReader.MoveNext(channels[i].sourceData, ref channels[i].readIndex);
							else
								channels[i].frameReader.value = channels[i].sourceTransformData[readFrameIndex + 1];
						}
					}

					//special case, hit the end of the animation, need to interpolate to the start again
					if (readFrameIndex == animation.KeyFrameCount - 1 && (loopTransition && looping))
					{
						for (int i = 0; i < channels.Length; i++)
							channels[i].storeTransform = channels[i].frameReader.value;

						//reset now...
						this.Reset(false, true);

						for (int i = 0; i < channels.Length; i++)
						{
							if (channels[i].sourceData != null)
								channels[i].frameReader.MoveNext(channels[i].sourceData, ref channels[i].readIndex);
							else
								channels[i].frameReader.value = channels[i].sourceTransformData[0];
						}
						readFrameIndex = 0;
						currentFrameTime = duration;
						previousFrameTime = animation.Duration;
						continue;
					}
					else
					{
						readFrameIndex++;
						currentFrameTime = animation.KeyFrameTime[readFrameIndex];
					}
				}

				if (frameTime < previousFrameTime)
				{
					//need to reset
					if (!(loopTransition && looping))
					{
						//gone off the end of the animation
						this.Reset(false, false);
					}
					else
					{
						//special case, interpolating between the last and first frame
						if (readFrameIndex != 0)
						{
							//managed to skip over the entire interpolate frame, so reload
							this.Reset(false, true);

							for (int i = 0; i < channels.Length; i++)
							{
								if (channels[i].sourceData != null)
									channels[i].frameReader.MoveNext(channels[i].sourceData, ref channels[i].readIndex);
								else
									channels[i].frameReader.value = channels[i].sourceTransformData[0];
							}
							readFrameIndex = 0;
						}
						currentFrameTime = 0;
						previousFrameTime -= animation.Duration;
					}
				}
			}

			//interpolate
			float interp = 0;

			if (previousFrameTime != currentFrameTime)
				interp = (frameTime - previousFrameTime) / (currentFrameTime - previousFrameTime);

			if (interp == 1)
			{
				for (int i = 0; i < channels.Length; i++)
					channels[i].lerpedTransform = channels[i].storeTransform;
			}
			else if (interp == 0)
			{
				for (int i = 0; i < channels.Length; i++)
					channels[i].lerpedTransform = channels[i].frameReader.value;
			}
			else
			{
				//interpolate the keyframes
				for (int i = 0; i < channels.Length; i++)
				{
					float invInterp = 1f - interp;
#if NO_INLINE
					Quaternion.Lerp(ref channels[i].storeTransform.Rotation, ref channels[i].frameReader.value.Rotation, interp, out channels[i].lerpedTransform.Rotation);
					Vector3.Lerp(ref channels[i].storeTransform.Translation, ref channels[i].frameReader.value.Translation, interp, out channels[i].lerpedTransform.Translation);
#else

					Vector3 v1 = channels[i].storeTransform.Translation;
					Vector3 v2 = channels[i].frameReader.value.Translation;
					Vector3 v3 = new Vector3();

					v3.X = v1.X * invInterp + v2.X * interp;
					v3.Y = v1.Y * invInterp + v2.Y * interp;
					v3.Z = v1.Z * invInterp + v2.Z * interp;

					channels[i].lerpedTransform.Translation = v3;


					Quaternion quaternion = new Quaternion(), quaternion1, quaternion2;
					quaternion1 = channels[i].storeTransform.Rotation;
					quaternion2 = channels[i].frameReader.value.Rotation;
					float num5 = interp;
					if ((((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W) < 0)
						num5 = -num5;

					quaternion.X = (invInterp * quaternion1.X) + (num5 * quaternion2.X);
					quaternion.Y = (invInterp * quaternion1.Y) + (num5 * quaternion2.Y);
					quaternion.Z = (invInterp * quaternion1.Z) + (num5 * quaternion2.Z);
					quaternion.W = (invInterp * quaternion1.W) + (num5 * quaternion2.W);

					if (quaternion.W != 1 && (quaternion.X != 0 & quaternion.Y != 0 & quaternion.Z != 0))
					{
						//normalize
						float len = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
						if (len != 1)
						{
							len = 1f / ((float)Math.Sqrt((double)len));
							quaternion.X *= len;
							quaternion.Y *= len;
							quaternion.Z *= len;
							quaternion.W *= len;
						}
					}

					channels[i].lerpedTransform.Rotation = quaternion;

#endif
					channels[i].lerpedTransform.Scale = channels[i].frameReader.value.Scale * interp + channels[i].storeTransform.Scale * invInterp;
				}
			}

			scale *= weighting;

			//apply weighting
			if (scale != 1)
			{
				for (int i = 0; i < channels.Length; i++)
				{
#if NO_INLINE
					channels[i].lerpedTransform.InterpolateToIdentity(scale);
#else
					Transform t = channels[i].lerpedTransform;
					t.Translation.X *= scale;
					t.Translation.Y *= scale;
					t.Translation.Z *= scale;
					t.Scale = t.Scale * scale + (1 - scale);

					t.Rotation.X = (scale * t.Rotation.X);
					t.Rotation.Y = (scale * t.Rotation.Y);
					t.Rotation.Z = (scale * t.Rotation.Z);

					if (t.Rotation.W >= 0)
						t.Rotation.W = (scale * t.Rotation.W) + (1 - scale);
					else
						t.Rotation.W = (scale * t.Rotation.W) - (1 - scale);

					if (t.Rotation.W != 1 && (t.Rotation.X != 0 & t.Rotation.Y != 0 & t.Rotation.Z != 0))
					{
						//normalize
						float len = (((t.Rotation.X * t.Rotation.X) + (t.Rotation.Y * t.Rotation.Y)) + (t.Rotation.Z * t.Rotation.Z)) + (t.Rotation.W * t.Rotation.W);
						if (len != 1)
						{
							len = 1f / ((float)Math.Sqrt((double)len));
							t.Rotation.X *= len;
							t.Rotation.Y *= len;
							t.Rotation.Z *= len;
							t.Rotation.W *= len;
						}
					}
					channels[i].lerpedTransform = t;
#endif
				}
			}
		}

		internal override float Duration()
		{
			float duration = animation.Duration;

			//special case, add a frame that interpolates from the end of the animation to the start
			if ((LoopTransition && Looping) && animation.KeyFrameCount > 0)
				duration += animation.Duration / (float)animation.KeyFrameCount;
			return duration;
		}
	}
}
