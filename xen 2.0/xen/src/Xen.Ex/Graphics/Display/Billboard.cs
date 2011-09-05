using System;
using System.Collections.Generic;
using System.Text;
using Xen.Graphics;
using Xen.Camera;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Xen.Ex.Graphics.Display;
using Xen.Ex.Graphics2D;
using Xen.Ex.Graphics.Content;

namespace Xen.Ex.Graphics.Display
{
	/// <summary>
	/// <para>Draws 2D <see cref="Element"/> particles as Billboard Sprites (With rotation)</para>
	/// <para>For use with a <see cref="ParticleSystem"/> instance.</para>
	/// </summary>
	public sealed class BillboardParticles2DElement : ParticleDrawer2DElement
	{
		//for various reasons billboards are used instead of point sprites particles
		//geforce FX cards, for instance, do not support point sprites


		private IVertices vertices;
		private IIndices indices;
#if !XBOX360
		//used by CPU particles
		private Vector4[] positionBuffer;
#endif

		/// <summary>
		/// Construct the Billboard Drawer
		/// </summary>
		/// <param name="system"></param>
		public BillboardParticles2DElement(ParticleSystem system)
			: base(system)
		{
		}

		/// <summary>
		/// implements the method to draw gpu particles
		/// </summary>
		protected override void DrawGpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint particleCount, AlphaBlendState blendMode, Texture2D positionTex, Texture2D velocityRotation, Texture2D colourTex, Texture2D userValues, bool usesUserValuesPositionBuffer)
		{
			Vector2 targetSize = state.DrawTarget.Size;

			state.RenderState.Push();
			state.RenderState.CurrentBlendState = blendMode;
			state.RenderState.CurrentDepthState.DepthWriteEnabled = false;

			//get the display texture, or a white texture if none exists
			Texture2D displayTexture = particleType.Texture ?? state.Properties.WhiteTexture;

			//get / create the shared vertices and indices for drawing billboard particles
			GenerateBillboardVertices(state, ref vertices, ref indices);

			int count = (int)particleCount;

			//instances of the two possible shaders
			DrawBillboardParticles_GpuTex shaderNoColour = null;
			DrawBillboardParticlesColour_GpuTex shaderColour = null;

			//user offset variants
			DrawBillboardParticles_GpuTex_UserOffset shaderNoColour_UO = null;
			DrawBillboardParticlesColour_GpuTex_UserOffset shaderColour_UO = null;

			float resolutionXF = (float)positionTex.Width;
			float resolutionYF = (float)positionTex.Height;

			Vector2 invTextureSize = new Vector2(1.0f / resolutionXF, 1.0f / resolutionYF);
			IShader shader;

			//setup the shader to use..

			if (!usesUserValuesPositionBuffer)
			{
				if (colourTex != null) // does this particle system use colours?
				{
					//get the shader
					shaderColour = state.GetShader<DrawBillboardParticlesColour_GpuTex>();

					//set the samplers
					shaderColour.PositionTexture = positionTex;
					shaderColour.ColourTexture = colourTex;
					shaderColour.VelocityTexture = velocityRotation;
					shaderColour.DisplayTexture = displayTexture;
					shader = shaderColour;
				}
				else
				{
					shaderNoColour = state.GetShader<DrawBillboardParticles_GpuTex>();

					shaderNoColour.PositionTexture = positionTex;
					shaderNoColour.VelocityTexture = velocityRotation;
					shaderNoColour.DisplayTexture = displayTexture;
					shader = shaderNoColour;
				}
			}
			else
			{
				//using the user position offset
				if (colourTex != null) // does this particle system use colours?
				{
					//get the shader
					shaderColour_UO = state.GetShader<DrawBillboardParticlesColour_GpuTex_UserOffset>();

					//set the samplers
					shaderColour_UO.PositionTexture = positionTex;
					shaderColour_UO.ColourTexture = colourTex;
					shaderColour_UO.VelocityTexture = velocityRotation;
					shaderColour_UO.UserTexture = userValues; // new
					shaderColour_UO.DisplayTexture = displayTexture;
					shader = shaderColour_UO;
				}
				else
				{
					shaderNoColour_UO = state.GetShader<DrawBillboardParticles_GpuTex_UserOffset>();

					shaderNoColour_UO.PositionTexture = positionTex;
					shaderNoColour_UO.VelocityTexture = velocityRotation;
					shaderNoColour_UO.UserTexture = userValues; // new
					shaderNoColour_UO.DisplayTexture = displayTexture;
					shader = shaderNoColour_UO;
				}
			}


			int drawn = 0;
			while (count > 0)
			{
				//draw upto vertices.Count / 4 (4 vertices per quad)
				int drawCount = Math.Min(count, vertices.Count / 4);

				//set the inverse texture size, and the start offset value, then bind the shader
				if (!usesUserValuesPositionBuffer)
				{
					if (colourTex != null)
						shaderColour.InvTextureSizeOffset = new Vector3(invTextureSize, (float)drawn);
					else
						shaderNoColour.InvTextureSizeOffset = new Vector3(invTextureSize, (float)drawn);
				}
				else
				{
					if (colourTex != null)
						shaderColour_UO.InvTextureSizeOffset = new Vector3(invTextureSize, (float)drawn);
					else
						shaderNoColour_UO.InvTextureSizeOffset = new Vector3(invTextureSize, (float)drawn);
				}

				using (state.Shader.Push(shader))
				{
					//draw!
					vertices.Draw(state, indices, PrimitiveType.TriangleList, drawCount * 2, 0, 0);
				}

				count -= drawCount;
				drawn += drawCount;
			}

			//and done.
			state.RenderState.Pop();
		}

#if !XBOX360
		/// <summary>
		/// implements the method to draw cpu particles
		/// </summary>
		protected override void DrawCpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint particleCount, AlphaBlendState blendMode, Vector4[] positionSize, Vector4[] velocityRotation, Vector4[] colourData, Vector4[] userValues)
		{
			//this is a bit more complex, but mostly the same as the GPU draw method
			Vector2 targetSize = state.DrawTarget.Size;

			state.RenderState.Push();
			state.RenderState.CurrentBlendState = blendMode;
			state.RenderState.CurrentDepthState.DepthWriteEnabled = false;

			Texture2D displayTexture = particleType.Texture ?? state.Properties.WhiteTexture;

			GenerateBillboardVertices(state, ref this.vertices, ref this.indices);


			int count = (int)particleCount;

			DrawBillboardParticles_BillboardCpu shaderNoColour = null;
			DrawBillboardParticlesColour_BillboardCpu shaderColour = null;

			if (positionBuffer == null)
			{
				positionBuffer = GetPositionBuffer(state, positionBuffer);
			}

			if (colourData != null)
				shaderColour = state.GetShader<DrawBillboardParticlesColour_BillboardCpu>();
			else
				shaderNoColour = state.GetShader<DrawBillboardParticles_BillboardCpu>();
		
			int drawn = 0;
			while (count > 0)
			{
				int drawCount;

				drawCount = Math.Min(count, 120);
				uint drawCountU = (uint)drawCount;
				uint drawnU = (uint)drawn;

				//the only major difference from the GPU drawer is here
				for (int i = 0; i < drawCount; i++)
				{
					//copy position xy and w (size), and velocity.w (rotation)
					positionBuffer[i] = positionSize[drawn + i];
					positionBuffer[i].Z = velocityRotation[drawn + i].W;
				}

				if (colourData != null)
				{
					shaderColour.SetPositionData(positionBuffer, 0, 0, drawCountU);
					shaderColour.SetColourData(colourData, drawnU, 0, drawCountU);

					shaderColour.DisplayTexture = displayTexture;

					state.Shader.Push(shaderColour);
				}
				else
				{
					shaderNoColour.SetPositionData(positionBuffer, 0, 0, drawCountU);

					shaderNoColour.DisplayTexture = displayTexture;

					state.Shader.Push(shaderNoColour);
				}

				vertices.Draw(state, indices, PrimitiveType.TriangleList, drawCount * 2, 0, 0);

				state.Shader.Pop();

				count -= drawCount;
				drawn += drawCount;
			}


			state.RenderState.Pop();
		}
#endif

		
		//chache for vertices and indices from here on...


		static string verticesID = typeof(BillboardParticles2DElement).FullName + ".vertices";
		static string indicesID = typeof(BillboardParticles2DElement).FullName + ".indices";

		static string posBufferID = typeof(BillboardParticles2DElement).FullName + ".positionBuffer";

		//get the global position buffer
		internal static Vector4[] GetPositionBuffer(DrawState state, Vector4[] positionBuffer)
		{
			positionBuffer = state.Application.UserValues[posBufferID] as Vector4[];
			if (positionBuffer == null)
			{
				positionBuffer = new Vector4[256];
				state.Application.UserValues[posBufferID] = positionBuffer;
			}
			return positionBuffer;
		}

		/// <summary>
		/// Get global vertices/indices for drawing billboards
		/// </summary>
		/// <param name="state"></param>
		/// <param name="vertices"></param>
		/// <param name="indices"></param>
		public static void GenerateBillboardVertices(DrawState state, ref IVertices vertices, ref IIndices indices)
		{
			if (vertices != null)
				return;

			vertices = state.Application.UserValues[verticesID] as IVertices;
			indices = state.Application.UserValues[indicesID] as IIndices;

			if (vertices != null)
				return;

			//8192 will be approx 512kb (however there is only one copy for the entire app)
#if XBOX360
			int maxVerts = 8192;
#else
			int maxVerts = ParticleSystem.SystemSupportsGpuParticles ? 8192 : 512;
#endif

			Vector4[] vertexData = new Vector4[maxVerts * 4];
			ushort[] indexData = new ushort[maxVerts * 6];

			int v = 0;
			int i = 0;
			for (int n = 0; n < maxVerts; n++)
			{
				vertexData[v++] = new Vector4(n, -1,  1, 0);
				vertexData[v++] = new Vector4(n,  1,  1, 0);
				vertexData[v++] = new Vector4(n,  1, -1, 0);
				vertexData[v++] = new Vector4(n, -1, -1, 0);

				indexData[i++] = (ushort)(n * 4 + 0);
				indexData[i++] = (ushort)(n * 4 + 1);
				indexData[i++] = (ushort)(n * 4 + 2);

				indexData[i++] = (ushort)(n * 4 + 0);
				indexData[i++] = (ushort)(n * 4 + 2);
				indexData[i++] = (ushort)(n * 4 + 3);
			}

			vertices = new Vertices<Vector4>(vertexData);
			indices = new Indices<ushort>(indexData);

			state.Application.UserValues[verticesID] = vertices;
			state.Application.UserValues[indicesID] = indices;
		}

	}
}
