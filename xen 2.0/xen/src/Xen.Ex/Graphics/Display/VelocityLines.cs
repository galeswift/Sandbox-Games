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
	//this class is a modified copy of the VelocityBillboard2DParticleDrawer

	/// <summary>
	/// <para>Draws 2D particles as Primitive Lines (Textures are ignored)</para>
	/// <para>Particles lines will be scaled by thier velocity length</para>
	/// <para>For use with a <see cref="ParticleSystem"/> instance.</para>
	/// </summary>
	public sealed class VelocityLineParticles2DElement : ParticleDrawer2DElement
	{
		private IVertices vertices;
		private float velocityScale = 0.01f;
		private bool useRotationToScaleVelocityEffect;

		/// <summary>
		/// Gets/Sets a scale factor to control how much the particles are extended by their velocity
		/// </summary>
		public float VelocityExtentionScale
		{
			get { return velocityScale; }
			set { velocityScale = value; }
		}

		/// <summary>
		/// <para>Gets/Sets a boolean flag to indicate that the Rotation value will be used to scale the Velocity effect</para>
		/// <para>When true, the per particle rotation value will be used to scale the velocity stretching effect</para>
		/// </summary>
		public bool UseRotationValueToScaleVelocityEffect
		{
			get { return useRotationToScaleVelocityEffect; }
			set { useRotationToScaleVelocityEffect = value; }
		}

		/// <summary>
		/// Construct the particle drawer
		/// </summary>
		/// <param name="system"></param>
		/// <param name="useRotationValueToScaleVelocityEffect"><para>When true, the per particle rotation value will be used to scale the velocity stretching effect</para><para>Allowing per-particle scaling based on velocity</para></param>
		public VelocityLineParticles2DElement(ParticleSystem system, bool useRotationValueToScaleVelocityEffect)
			: base(system)
		{
			this.useRotationToScaleVelocityEffect = useRotationValueToScaleVelocityEffect;
		}

		/// <summary>
		/// draws the particles on a GPU system
		/// </summary>
		protected override void DrawGpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint particleCount, AlphaBlendState blendMode, Texture2D positionTex, Texture2D velocityRotation, Texture2D colourTex, Texture2D userValues, bool usesUserValuesPositionBuffer)
		{
			Vector2 targetSize = state.DrawTarget.Size;

			state.RenderState.Push();
			state.RenderState.CurrentBlendState = blendMode;
			state.RenderState.CurrentDepthState.DepthWriteEnabled = false;

			//get the shared vertice
			VelocityLineParticles2DElement.GenerateLinesVertices(state, ref vertices);

			int count = (int)particleCount;

			DrawVelocityParticles_LinesGpuTex shaderNoColour = null;
			DrawVelocityParticlesColour_LinesGpuTex shaderColour = null;
			//user variants
			DrawVelocityParticles_LinesGpuTex_UserOffset shaderNoColour_UO = null;
			DrawVelocityParticlesColour_LinesGpuTex_UserOffset shaderColour_UO = null;

			float resolutionXF = (float)positionTex.Width;
			float resolutionYF = (float)positionTex.Height;

			Vector2 invTextureSize;

			Vector2 velScale = new Vector2(velocityScale, 0);
			if (this.useRotationToScaleVelocityEffect)
				velScale = new Vector2(0, velocityScale);
			invTextureSize = new Vector2(1.0f / resolutionXF, 1.0f / resolutionYF);


			IShader shader;
			if (!usesUserValuesPositionBuffer)
			{
				if (colourTex != null)
				{
					shader = shaderColour = state.GetShader<DrawVelocityParticlesColour_LinesGpuTex>();

					shaderColour.PositionTexture = positionTex;
					shaderColour.ColourTexture = colourTex;
					shaderColour.VelocityTexture = velocityRotation;

					shaderColour.SetVelocityScale(ref velScale);
				}
				else
				{
					shader = shaderNoColour = state.GetShader<DrawVelocityParticles_LinesGpuTex>();

					shaderNoColour.PositionTexture = positionTex;
					shaderNoColour.VelocityTexture = velocityRotation;

					shaderNoColour.SetVelocityScale(ref velScale);
				}
			}
			else
			{
				if (colourTex != null)
				{
					shader = shaderColour_UO = state.GetShader<DrawVelocityParticlesColour_LinesGpuTex_UserOffset>();

					shaderColour_UO.PositionTexture = positionTex;
					shaderColour_UO.ColourTexture = colourTex;
					shaderColour_UO.VelocityTexture = velocityRotation;

					shaderColour_UO.SetVelocityScale(ref velScale);
				}
				else
				{
					shader = shaderNoColour_UO = state.GetShader<DrawVelocityParticles_LinesGpuTex_UserOffset>();

					shaderNoColour_UO.PositionTexture = positionTex;
					shaderNoColour_UO.VelocityTexture = velocityRotation;

					shaderNoColour_UO.SetVelocityScale(ref velScale);
				}
			}


			int drawn = 0;
			while (count > 0)
			{
				int drawCount = Math.Min(count, vertices.Count / 4);

				if (!usesUserValuesPositionBuffer)
				{
					if (colourTex != null)
						shaderColour.TextureSizeOffset = new Vector3(invTextureSize, (float)drawn);
					else
						shaderNoColour.TextureSizeOffset = new Vector3(invTextureSize, (float)drawn);
				}
				else
				{
					if (colourTex != null)
						shaderColour_UO.TextureSizeOffset = new Vector3(invTextureSize, (float)drawn);
					else
						shaderNoColour_UO.TextureSizeOffset = new Vector3(invTextureSize, (float)drawn);
				}

				//bind
				using (state.Shader.Push(shader))
				{
					//draw!
					vertices.Draw(state, null, PrimitiveType.LineList, drawCount, 0, 0);
				}

				count -= drawCount;
				drawn += drawCount;
			}


			state.RenderState.Pop();
		}

#if !XBOX360
		/// <summary>
		/// draws the particles from a CPU system
		/// </summary>
		protected override void DrawCpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint particleCount, AlphaBlendState blendMode, Vector4[] positionSize, Vector4[] velocityRotation, Vector4[] colourData, Vector4[] userValues)
		{
			Vector2 targetSize = state.DrawTarget.Size;

			state.RenderState.Push();
			state.RenderState.CurrentBlendState = blendMode;
			state.RenderState.CurrentDepthState.DepthWriteEnabled = false;

			VelocityLineParticles2DElement.GenerateLinesVertices(state, ref this.vertices);


			int count = (int)particleCount;



			DrawVelocityParticles_LinesCpu shaderNoColour = null;
			DrawVelocityParticlesColour_LinesCpu shaderColour = null;

			if (colourData != null)
				shaderColour = state.GetShader<DrawVelocityParticlesColour_LinesCpu>();
			else
				shaderNoColour = state.GetShader<DrawVelocityParticles_LinesCpu>();

			Vector2 velScale = new Vector2(velocityScale, 0);
			if (this.useRotationToScaleVelocityEffect)
				velScale = new Vector2(0, velocityScale);

			int drawn = 0;
			while (count > 0)
			{
				int drawCount;

				drawCount = Math.Min(count, 80);
				uint drawCountU = (uint)drawCount;
				uint drawnU = (uint)drawn;

				if (colourData != null)
				{
					shaderColour.SetPositionData(positionSize, drawnU, 0, drawCountU);
					shaderColour.SetVelocityData(velocityRotation, drawnU, 0, drawCountU);
					shaderColour.SetColourData(colourData, drawnU, 0, drawCountU);

					shaderColour.SetVelocityScale(ref velScale);

					state.Shader.Push(shaderColour);
				}
				else
				{
					shaderNoColour.SetPositionData(positionSize, drawnU, 0, drawCountU);
					shaderNoColour.SetVelocityData(velocityRotation, drawnU, 0, drawCountU);

					shaderNoColour.SetVelocityScale(ref velScale);

					state.Shader.Push(shaderNoColour);
				}

				vertices.Draw(state, null, PrimitiveType.LineList, drawCount, 0, 0);

				state.Shader.Pop();

				count -= drawCount;
				drawn += drawCount;
			}


			state.RenderState.Pop();
		}
#endif



		//global storage of vertices for drawing line particles

		static string verticesID = typeof(VelocityLineParticles2DElement).FullName + ".vertices";

		/// <summary>Generates a vertex buffer used for drawing lines in GPU or CPU batches</summary>
		public static void GenerateLinesVertices(DrawState state, ref IVertices vertices)
		{
			if (vertices != null)
				return;

			vertices = state.Application.UserValues[verticesID] as IVertices;

			if (vertices != null)
				return;

			//4096 will be approx 256kb (however there is only one copy for the entire app)
#if XBOX360
			int maxVerts = 4096;
#else
			int maxVerts = ParticleSystem.SystemSupportsGpuParticles ? 4096 : 512;
#endif

			Vector2[] vertexData = new Vector2[maxVerts * 2];

			int v = 0;
			for (int n = 0; n < maxVerts; n++)
			{
				vertexData[v++] = new Vector2(n, -1);
				vertexData[v++] = new Vector2(n, 1);
			}

			vertices = Vertices<Vector2>.CreateSingleElementVertices(vertexData, VertexElementUsage.Position, 0);

			state.Application.UserValues[verticesID] = vertices;
		}

	}
}
