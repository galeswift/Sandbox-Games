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
	/// <para>Draws <see cref="BatchModelInstance"/> objects in a batch, using <see cref="ModelData"/> loaded through the content pipeline</para>
	/// <para>Note: BatchModel does not support model animation</para>
	/// </summary>
	public sealed class BatchModel : IDraw
	{
		private ModelData modelData;
		private int childCount;
		private int drawCount;
		private Xen.Graphics.InstanceBuffer[] buffers;
		private IModelShaderProvider shaderProvider = new Provider.DefaultMaterialShaderProvider();
		private MaterialLightCollection lights;

		/// <summary>
		/// Extend the <see cref="IModelShaderProvider"/> interface to set the shaders used by this model instance
		/// <para>The default shader provider will bind the <see cref="MaterialShader"/> stored within the <see cref="ModelData"/>.</para>
		/// <para>Set to NULL and the model will be drawn with the current bound shader</para>
		/// </summary>
		public IModelShaderProvider ShaderProvider
		{
			get { return shaderProvider; }
			set { shaderProvider = value; }
		}

		/// <summary>
		/// <see cref="ModelData"/> used by this batch model. ModelData content must be assigned before the batch or an instance is drawn
		/// </summary>
		/// <remarks>ModelData may only be assigned once per batch instance</remarks>
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


		internal void CountChild()
		{
			childCount++;
		}

		//the child isn't drawn right now, but for every bit of geometry that is visible, the world matrix is stored
		internal void DrawChild(DrawState state)
		{
			if (modelData == null)
				throw new InvalidOperationException("ModelData is null");

			if (buffers == null)
				SetupBuffers();
			
			ContainmentType cullModel = ContainmentType.Contains;

			//if there is just one geometry object, then the ICullable.CullTest() call will have been suficient.
			bool skipCullTest = this.modelData != null && this.modelData.meshes.Length == 1 && this.modelData.meshes[0].geometry.Length == 1;
			ICuller culler = state as ICuller;

			if (!skipCullTest)
			{
				cullModel = culler.IntersectBox(ref modelData.staticBounds.minimum, ref modelData.staticBounds.maximum);
			}

			int geometryIndex = 0;
			bool drawn = false;

			//loop through the model data
			if (cullModel != ContainmentType.Disjoint)
			{
				for (int m = 0; m < modelData.meshes.Length; m++)
				{
					MeshData mesh = modelData.meshes[m];

					ContainmentType cullMesh = cullModel;

					//cull testing along the way
					if (cullModel == ContainmentType.Intersects && modelData.meshes.Length > 1)
						cullMesh = culler.IntersectBox(ref mesh.staticBounds.minimum, ref mesh.staticBounds.maximum);

					if (cullMesh != ContainmentType.Disjoint)
					{
						for (int g = 0; g < mesh.geometry.Length; g++)
						{
							GeometryData geom = mesh.geometry[g];

							bool cullTest = true;

							if (cullMesh == ContainmentType.Intersects && mesh.geometry.Length > 1)
								cullTest = culler.TestBox(ref geom.staticBounds.minimum, ref geom.staticBounds.maximum);

							//finally, is the geometry visible?
							if (cullTest)
							{
								//add the world matrix to the geometry set
								InstanceBuffer buffer = this.buffers[geometryIndex];

								if (buffer == null)
								{
									buffer = state.GetDynamicInstanceBuffer(childCount);
									this.buffers[geometryIndex] = buffer;
								}

								buffer.AddInstance(state);
								drawn = true;
							}

							geometryIndex++;
						}
					}
					else
						geometryIndex += mesh.geometry.Length;
				}
			}
			if (drawn)
				drawCount++;
		}

		internal bool CullChild(ICuller culler)
		{
			if (modelData == null)
				throw new InvalidOperationException("ModelData is null");
			return culler.TestBox(ref modelData.staticBounds.minimum, ref modelData.staticBounds.maximum);
		}
		internal bool CullChild(ICuller culler, ref Matrix instance)
		{
			if (modelData == null)
				throw new InvalidOperationException("ModelData is null");
			return culler.TestBox(ref modelData.staticBounds.minimum, ref modelData.staticBounds.maximum, ref instance);
		}

		/// <summary>
		/// Draw all the model batch instances
		/// </summary>
		/// <param name="state"></param>
		public void Draw(DrawState state)
		{
			if (modelData == null)
				throw new InvalidOperationException("ModelData is null");

			if (buffers == null)
				SetupBuffers();

			int bufferIndex = 0;

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

			IShader shader = null;
			if (shaderProvider != null)
			{
				shader = shaderProvider.BeginModel(state, lights);
				if (shader != null)
					state.Shader.Push(shader);
			}

			//loop through the model data
			for (int m = 0; m < modelData.meshes.Length; m++)
			{
				MeshData mesh = modelData.meshes[m];

				for (int g = 0; g < mesh.geometry.Length; g++)
				{
					GeometryData geom = mesh.geometry[g];
					InstanceBuffer buffer = this.buffers[bufferIndex];

					if (buffer != null)
					{
						IShader geomShader = null;
						if (shaderProvider != null)
						{
							geomShader = shaderProvider.BeginGeometry(state, geom);
							if (geomShader != null)
								state.Shader.Push(geomShader);
						}

						//draw the geometry
						geom.Vertices.DrawInstances(state, geom.Indices, PrimitiveType.TriangleList, buffer);

						if (geomShader != null)
							state.Shader.Pop();
					}

					this.buffers[bufferIndex] = null;
					bufferIndex++;
				}
			}

			if (shaderProvider != null)
			{
				shaderProvider.EndModel(state);
				if (shader != null)
					state.Shader.Pop();
			}

			drawCount = 0;
		}

		bool ICullable.CullTest(ICuller culler)
		{
			return drawCount > 0;
		}

		private void SetupBuffers()
		{
			//count up the total number of geometry objects in the modelData

			int count = 0;
			foreach (MeshData mesh in modelData.Meshes)
				count += mesh.Geometry.Length;

			//allocate a GeometrySet for each

			this.buffers = new Xen.Graphics.InstanceBuffer[count];
		}
	}

	/// <summary>
	/// <para>Represents an instance of a model, drawn with a <see cref="BatchModel"/></para>
	/// </summary>
	public sealed class BatchModelInstance : IDraw, ICullableInstance
	{
		private readonly BatchModel parent;

		/// <summary>
		/// Construct the batch model instance
		/// </summary>
		/// <param name="model">The BatchModel that will draw this instance</param>
		public BatchModelInstance(BatchModel model)
		{
			if (model == null)
				throw new ArgumentNullException();
			this.parent = model;
			this.parent.CountChild();
		}

		/// <summary>
		/// Draw this instance of the BatchModel
		/// </summary>
		/// <param name="state"></param>
		public void Draw(DrawState state)
		{
			parent.DrawChild(state);
		}

		/// <summary>
		/// Cull test this instance of the BatchModel
		/// </summary>
		/// <param name="culler"></param>
		/// <returns></returns>
		public bool CullTest(ICuller culler)
		{
			return parent.CullChild(culler);
		}

		/// <summary>
		/// Cull test this instance of the BatchModel
		/// </summary>
		public bool CullTest(ICuller culler, ref Matrix instance)
		{
			return parent.CullChild(culler, ref instance);
		}
	}

}
