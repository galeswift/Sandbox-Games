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
	/// <summary>
	/// An interface to an object that provides the shaders or materials used by a model
	/// </summary>
	public interface IModelShaderProvider
	{
		/// <summary>
		/// A method called before the model is dawn. Return a shader to use, or return null to keep the current shader.
		/// </summary>
		IShader BeginModel(DrawState state, MaterialLightCollection lights);
		/// <summary>
		/// A method called before model geometry is dawn. Return a shader to use, or return null to keep the current shader.
		/// </summary>
		IShader BeginGeometry(DrawState state, GeometryData geometry);
		/// <summary>
		/// A method called after the model has finished being dawn.
		/// </summary>
		void EndModel(DrawState state);
	}

	namespace Provider
	{


		//internal default shader provider
		/// <summary>
		/// <para>The default shader provider used by a ModelInstance.</para>
		/// <para>This provider will bind the MaterialShader and lights used by the model</para>
		/// </summary>
		public sealed class DefaultMaterialShaderProvider : IModelShaderProvider
		{
			private MaterialLightCollection lights;

			/// <summary></summary>
			IShader IModelShaderProvider.BeginModel(DrawState state, MaterialLightCollection lights)
			{
				this.lights = lights;
				return null;
			}

			/// <summary></summary>
			IShader IModelShaderProvider.BeginGeometry(DrawState state, GeometryData geometry)
			{
				geometry.defaultShader.LightCollection = lights;
				return geometry.defaultShader;
			}

			/// <summary></summary>
			void IModelShaderProvider.EndModel(DrawState state)
			{
			}
		}

		/// <summary>
		/// A model shader provider that displays the model using a user defined shader
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public sealed class SimpleShaderProvider<T> : IModelShaderProvider where T : class, IShader, new()
		{
			private readonly T instance;

			/// <summary>When no shader is specified, the global state provided instance is used. See <see cref="DrawState.GetShader"/></summary>
			public SimpleShaderProvider()
			{
			}

			/// <summary></summary>
			public SimpleShaderProvider(T shader)
			{
				if (shader == null)
					throw new ArgumentNullException();
				this.instance = shader;
			}

			IShader IModelShaderProvider.BeginModel(DrawState state, MaterialLightCollection lights)
			{
				MaterialShader ms = this.instance as MaterialShader;
				if (ms != null) ms.LightCollection = lights;

				return instance ?? state.GetShader<T>();
			}

			IShader IModelShaderProvider.BeginGeometry(DrawState state, GeometryData geometry)
			{
				return null;
			}

			void IModelShaderProvider.EndModel(DrawState state)
			{
			}
		}

		/// <summary>
		/// A model shader provider that displays the model using a custom <see cref="LightingDisplayModel"/>
		/// <para>A base shader provider must be provided</para>
		/// </summary>
		public sealed class LightingDisplayShaderProvider : IModelShaderProvider
		{
			private readonly IModelShaderProvider baseProvider;
			private LightingDisplayModelFlag displayModel;

			/// <summary>
			/// Gets or sets the display model
			/// </summary>
			public LightingDisplayModel DisplayModel
			{
				get { return displayModel.DisplayModel; }
				set { displayModel.DisplayModel = value; }
			}

			/// <summary>
			/// Constructs the shader provider
			/// </summary>
			/// <param name="displayModel"></param>
			/// <param name="baseProvider">(may be null)</param>
			public LightingDisplayShaderProvider(LightingDisplayModel displayModel, IModelShaderProvider baseProvider)
			{
				this.displayModel = new LightingDisplayModelFlag(displayModel);
				this.baseProvider = baseProvider ?? new DefaultMaterialShaderProvider();
			}

			IShader IModelShaderProvider.BeginModel(DrawState state, MaterialLightCollection lights)
			{
				state.DrawFlags.Push(ref this.displayModel);
				return baseProvider.BeginModel(state, lights);
			}

			IShader IModelShaderProvider.BeginGeometry(DrawState state, GeometryData geometry)
			{
				return baseProvider.BeginGeometry(state, geometry);
			}

			void IModelShaderProvider.EndModel(DrawState state)
			{
				state.DrawFlags.Pop<LightingDisplayModelFlag>();
				baseProvider.EndModel(state);
			}
		}
	}

	/// <summary>
	/// <para>A structure that can be used as a Draw Flag to force drawn model instances to use a specific <see cref="IModelShaderProvider"/></para>
	/// </summary>
	public struct ModelShaderProviderFlag
	{
		/// <summary>
		/// <para>Force drawn model instances to use <see cref="ShaderProvider"/></para>
		/// </summary>
		public bool OverrideShaderProvider;
		/// <summary>Shader provider to use, if null, the model will use the currently bound shader</summary>
		public IModelShaderProvider ShaderProvider;

		/// <summary></summary>
		/// <param name="shaderProvider"></param>
		public ModelShaderProviderFlag(IModelShaderProvider shaderProvider)
		{
			this.OverrideShaderProvider = true;
			this.ShaderProvider = shaderProvider;
		}
	}

	/// <summary>
	/// Draws <see cref="ModelData"/> loaded through the content pipeline
	/// </summary>
	public sealed class ModelInstance : IDraw, ICullableInstance
	{
		private ModelData modelData;
		private AnimationController controller;
		private MaterialLightCollection lights;
		private IModelShaderProvider shaderProvider = new Provider.DefaultMaterialShaderProvider();
		
		private static string temporaryAnimationDataName = typeof(ModelInstance).Name + ".TempAnimationData";

		/// <summary>
		/// Construct the model instance. Setting the <see cref="ModelData"/> content is required before drawing
		/// </summary>
		public ModelInstance()
		{
		}

		/// <summary>
		/// Construct the model instance with existing model data
		/// </summary>
		/// <param name="sourceData"></param>
		public ModelInstance(ModelData sourceData)
		{
			this.modelData = sourceData;
		}

		/// <summary>
		/// <para>Extend the <see cref="IModelShaderProvider"/> interface to set the shaders used by this model instance</para>
		/// <para>The default shader provider will bind the <see cref="MaterialShader"/> stored within the <see cref="ModelData"/>.</para>
		/// <para>Set to NULL and the model will be drawn with the current bound shader</para>
		/// </summary>
		public IModelShaderProvider ShaderProvider
		{
			get { return shaderProvider; }
			set { shaderProvider = value; }
		}

		/// <summary>
		/// <see cref="ModelData"/> used by this model instance. ModelData content must be assigned before the instance is drawn
		/// </summary>
		/// <remarks>ModelData may only be assigned once per instance</remarks>
		public ModelData ModelData
		{
			get { return modelData; }
			set
			{
				if (value == null)
					throw new ArgumentNullException();
				if (this.modelData != null && this.modelData != value)
					throw new InvalidOperationException("ModelData may only be assigned once");
				this.modelData = value;
				if (controller != null)
					this.controller.SetModelData(value);
			}
		}

		/// <summary>
		/// Gets/Sets the lights collection used by any material shaders loaded with the model
		/// </summary>
		public MaterialLightCollection LightCollection
		{
			get { return lights; }
			set { lights = value; }
		}

		/// <summary>
		/// Gets/Creates an animation controller for this mesh instance
		/// </summary>
		/// <returns></returns>
		public AnimationController GetAnimationController()
		{
			if (modelData != null && modelData.skeleton == null)
				throw new InvalidOperationException("ModelData has no skeleton");
			if (controller == null)
				controller = new AnimationController(this.modelData, null, this);
			return controller;
		}

		/// <summary>
		/// <para>Gets/Creates an animation controller that runs as a thread task</para>
		/// <para>Async animations require adding to an <see cref="UpdateManager"/> because their processing is initalised at the end of the update loop</para>
		/// </summary>
		/// <param name="manager"></param>
		/// <returns></returns>
		public AnimationController GetAsyncAnimationController(UpdateManager manager)
		{
			if (manager == null)
				throw new ArgumentNullException();
			if (modelData != null && modelData.skeleton == null)
				throw new InvalidOperationException("ModelData has no skeleton");
			if (controller == null)
				controller = new AnimationController(this.modelData, manager, this);
			return controller;
		}

		/// <summary>
		/// <para>Computes the bounding box of the model. This may require calculating the animation bounds as well.</para>
		/// </summary>
		public void CalculateBounds(out Vector3 minBounds, out Vector3 maxBounds)
		{
			if (modelData == null)
				throw new InvalidOperationException("ModelData is null");

			if (controller != null)
			{
				controller.ComputeAnimationBounds();

				minBounds = controller.boundsMin;
				maxBounds = controller.boundsMax;
			}
			else
			{
				minBounds = modelData.staticBounds.minimum;
				maxBounds = modelData.staticBounds.maximum;
			}
		}

		/// <summary>
		/// <para>Share an existing animation controller with this model</para>
		/// <para>Use this method to have a single animation controller animate multiple models</para>
		/// </summary>
		/// <param name="controller"></param>
		public void SetSharedAnimationController(AnimationController controller)
		{
			if (controller == null)
				throw new ArgumentNullException();
			if (this.controller == controller)
				return;
			if (this.controller != null)
				throw new InvalidOperationException("AnimationController already set");
			if (controller.ModelData != this.modelData || (this.modelData == null && controller.ModelData == null))
				throw new ArgumentException("ModelData mismatch");
			controller.AddParnet(this);
			this.controller = controller;
		}

		/// <summary>
		/// Draw the model. If <see cref="ShaderProvider"/> is non-null, this class automatically assigns shaders when drawing
		/// </summary>
		/// <param name="state"></param>
		public void Draw(DrawState state)
		{
			if (modelData == null)
				throw new InvalidOperationException("ModelData is null");

			AnimationTransformArray hierarchy = null;
			if (controller != null)
			{
				hierarchy = controller.WaitForAsyncAnimation(state, state.Properties.FrameIndex, true);

				if (controller.IsDisposed)
					controller = null;
			}

			IModelShaderProvider shaderProvider = this.shaderProvider;
			MaterialLightCollection lights = this.lights;

			ModelShaderProviderFlag providerFlag;
			MaterialLightCollectionFlag lightsFlag;

			Xen.Graphics.Stack.DrawFlagStack flags = state.DrawFlags;

			flags.GetFlag(out providerFlag);
			if (providerFlag.OverrideShaderProvider)
				shaderProvider = providerFlag.ShaderProvider;

			flags.GetFlag(out lightsFlag);
			if (lightsFlag.OverrideLightCollection)
				lights = lightsFlag.LightCollection;

			Vector3 boundsMin, boundsMax;

			ContainmentType cullModel = ContainmentType.Contains;

			//if there is just one geometry object, then the ICullable.CullTest() call will have been suficient.
			bool skipCullTest = this.modelData != null && this.modelData.meshes.Length == 1 && this.modelData.meshes[0].geometry.Length == 1;

			if (!skipCullTest)
			{
				if (controller != null)
					cullModel = (state as ICuller).IntersectBox(ref controller.boundsMin, ref controller.boundsMax);
				else
					cullModel = (state as ICuller).IntersectBox(ref modelData.staticBounds.minimum, ref modelData.staticBounds.maximum);
			}

			if (cullModel != ContainmentType.Disjoint)
			{
				IShader shader = null;
				bool beginCalled = false;

				for (int m = 0; m < modelData.meshes.Length; m++)
				{
					MeshData mesh = modelData.meshes[m];
					ContainmentType cullMesh = cullModel;

					if (cullModel == ContainmentType.Intersects && modelData.meshes.Length > 1)
					{
						if (controller != null)
						{
							controller.ComputeMeshBounds(m, out boundsMin, out boundsMax);
							cullMesh = (state as ICuller).IntersectBox(ref boundsMin, ref boundsMax);
						}
						else
							cullMesh = (state as ICuller).IntersectBox(ref mesh.staticBounds.minimum, ref mesh.staticBounds.maximum);

					}

					if (cullMesh != ContainmentType.Disjoint)
					{
						for (int g = 0; g < mesh.geometry.Length; g++)
						{
							GeometryData geom = mesh.geometry[g];

							bool cullTest = true;

							if (cullMesh == ContainmentType.Intersects && mesh.geometry.Length > 1)
							{
								if (controller != null)
								{
									controller.ComputeGeometryBounds(m, g, out boundsMin, out boundsMax);
									cullTest = (state as ICuller).TestBox(ref boundsMin, ref boundsMax);
								}
								else
									cullTest = (state as ICuller).TestBox(ref geom.staticBounds.minimum, ref geom.staticBounds.maximum);
							}

							if (cullTest)
							{
								IShader geomShader = null;
								if (shaderProvider != null)
								{
									if (!beginCalled)
									{
										shader = shaderProvider.BeginModel(state, lights);
										beginCalled = true;
										if (shader != null)
											state.Shader.Push(shader);
									}
									geomShader = shaderProvider.BeginGeometry(state, geom);

									if (geomShader != null)
										state.Shader.Push(geomShader);
								}

								AnimationTransformArray animationData = hierarchy;
								bool popWorldMatrix = false;

								if (animationData != null)
								{
									//if there is just one value, then this geom uses just a single bone
									if (geom.geometryBoneSingleIndex != -1 && state.Shader.CurrentShader != null)	//only do this if an IShader is bound
									{
										Matrix matrix;
										animationData.GetBoneMatrix(geom.geometryBoneSingleIndex, out matrix);
										state.WorldMatrix.PushMultiply(ref matrix);

										popWorldMatrix = true;
										animationData = null;	//this will force a non-animated shader to be used
									}
									else if (geom.geometryBoneRemapping != null)
									{
										//yikes. animation data is too big to to be rendered
										//has to have the bone data copied
										AnimationTransformArray copy = state.Application.UserValues[temporaryAnimationDataName] as AnimationTransformArray;
										if (copy == null)
										{
											copy = new AnimationTransformArray(72);
											state.Application.UserValues[temporaryAnimationDataName] = copy;
										}

										//copy remapped bones in.
										copy.UpdateTransformArray(animationData, geom.geometryBoneRemapping);
										animationData = copy;
									}
								}

								geom.Vertices.DrawBlending(state, geom.Indices, PrimitiveType.TriangleList, animationData);

								if (popWorldMatrix)
									state.WorldMatrix.Pop();

								if (geomShader != null)
									state.Shader.Pop();
							}
						}
					}
				}

				if (beginCalled)
				{
					shaderProvider.EndModel(state);
					if (shader != null)
						state.Shader.Pop();
				}
			}
		}

		/// <summary>
		/// FrustumCull test the model
		/// </summary>
		/// <param name="culler"></param>
		/// <returns></returns>
		public bool CullTest(ICuller culler)
		{
			if (modelData == null)
				throw new InvalidOperationException("ModelData is null");

			if (controller == null)
				return culler.TestBox(ref modelData.staticBounds.minimum, ref modelData.staticBounds.maximum);
			else
			{
				controller.WaitForAsyncAnimation(culler.GetState(), culler.FrameIndex, false);
				return culler.TestBox(ref controller.boundsMin, ref controller.boundsMax);
			}
		}


		/// <summary>
		/// FrustumCull test the model at the given location
		/// </summary>
		public bool CullTest(ICuller culler, ref Matrix instance)
		{
			if (modelData == null)
				throw new InvalidOperationException("ModelData is null");

			if (controller == null)
				return culler.TestBox(ref modelData.staticBounds.minimum, ref modelData.staticBounds.maximum, ref instance);
			else
			{
				controller.WaitForAsyncAnimation(culler.GetState(), culler.FrameIndex, false);
				return culler.TestBox(ref controller.boundsMin, ref controller.boundsMax, ref instance);
			}
		}
	}
}
