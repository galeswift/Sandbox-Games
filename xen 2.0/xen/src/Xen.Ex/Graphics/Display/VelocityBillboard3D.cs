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
	/// <para>Draws 3D particles as Billboard Sprites (Rotation is generated per particle)</para>
	/// <para>Particles will be scaled by thier velocity length, and rotated in the direction they are travelling</para>
	/// <para>For use with a <see cref="ParticleSystem"/> instance.</para>
	/// </summary>
	public sealed class VelocityBillboardParticles3D : ParticleDrawer3D
	{
		private IVertices vertices;
		private IIndices indices;
		private float velocityScale = 0.01f;
		private bool useRotationToScaleVelocityEffect;

		/// <summary>
		/// Construct the Velocity Billboard particles
		/// </summary>
		/// <param name="system"></param>
		/// <param name="useRotationValueToScaleVelocityEffect"><para>When true, the per particle rotation value will be used to scale the velocity stretching effect</para><para>Allowing per-particle scaling based on velocity</para></param>
		/// <param name="velocityExtentionScale">Scale factor to extend the particles</param>
		public VelocityBillboardParticles3D(ParticleSystem system, bool useRotationValueToScaleVelocityEffect, float velocityExtentionScale)
			: base(system)
		{
			this.UseRotationValueToScaleVelocityEffect = useRotationValueToScaleVelocityEffect;
			this.VelocityExtentionScale = velocityExtentionScale;
		}

		/// <summary>
		/// Construct the Velocity Billboard particles
		/// </summary>
		/// <param name="system"></param>
		/// <param name="useRotationValueToScaleVelocityEffect"><para>When true, the per particle rotation value will be used to scale the velocity stretching effect</para><para>Allowing per-particle scaling based on velocity</para></param>
		public VelocityBillboardParticles3D(ParticleSystem system, bool useRotationValueToScaleVelocityEffect)
			: base(system)
		{
			this.UseRotationValueToScaleVelocityEffect = useRotationValueToScaleVelocityEffect;
		}

		/// <summary>
		/// Gets/Sets a scale factor to control how much the particles are extended by their velocity
		/// </summary>
		public float VelocityExtentionScale
		{
			get { return velocityScale; }
			set { velocityScale = value; }
		}

		/// <summary>
		/// <para>Gets/Sets a boolean flag to indicate that the Rotation value will be used to scale the Velocity scaling effect</para>
		/// <para>When true, the per particle rotation value will be used to scale the velocity stretching effect</para>
		/// </summary>
		public bool UseRotationValueToScaleVelocityEffect
		{
			get { return useRotationToScaleVelocityEffect; }
			set { useRotationToScaleVelocityEffect = value; }
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
			BillboardParticles2DElement.GenerateBillboardVertices(state, ref vertices, ref indices);

			int count = (int)particleCount;

			//instances of the two possible shaders
			DrawVelocityBillboardParticles_GpuTex3D shaderNoColour = null;
			DrawVelocityBillboardParticlesColour_GpuTex3D shaderColour = null;
			//user variantes
			DrawVelocityBillboardParticles_GpuTex3D_UserOffset shaderNoColour_UO = null;
			DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset shaderColour_UO = null;

			float resolutionXF = (float)positionTex.Width;
			float resolutionYF = (float)positionTex.Height;

			Vector2 invTextureSize = new Vector2(1.0f / resolutionXF, 1.0f / resolutionYF);

			Matrix cameraMatrix;
			state.Camera.GetCameraMatrix(out cameraMatrix);

			Vector3 worldSpaceYAxis = new Vector3(cameraMatrix.M21, cameraMatrix.M22, cameraMatrix.M23);


			Vector2 velScale = new Vector2(velocityScale, 0);
			if (this.useRotationToScaleVelocityEffect)
				velScale = new Vector2(0, velocityScale);

			IShader shader;

			if (!usesUserValuesPositionBuffer)
			{
				if (colourTex != null) // does this particle system use colours?
				{
					//get the shader
					shaderColour = state.GetShader<DrawVelocityBillboardParticlesColour_GpuTex3D>();

					//set the samplers
					shaderColour.PositionTexture = positionTex;
					shaderColour.ColourTexture = colourTex;
					shaderColour.VelocityTexture = velocityRotation;
					shaderColour.DisplayTexture = displayTexture;

					shaderColour.SetWorldSpaceYAxis(ref worldSpaceYAxis);
					shaderColour.SetVelocityScale(ref velScale);
					shader = shaderColour;
				}
				else
				{
					shaderNoColour = state.GetShader<DrawVelocityBillboardParticles_GpuTex3D>();

					shaderNoColour.PositionTexture = positionTex;
					shaderNoColour.VelocityTexture = velocityRotation;
					shaderNoColour.DisplayTexture = displayTexture;

					shaderNoColour.SetWorldSpaceYAxis(ref worldSpaceYAxis);
					shaderNoColour.SetVelocityScale(ref velScale);
					shader = shaderNoColour;
				}
			}
			else
			{
				if (colourTex != null) // does this particle system use colours?
				{
					//get the shader
					shaderColour_UO = state.GetShader<DrawVelocityBillboardParticlesColour_GpuTex3D_UserOffset>();

					//set the samplers
					shaderColour_UO.PositionTexture = positionTex;
					shaderColour_UO.ColourTexture = colourTex;
					shaderColour_UO.VelocityTexture = velocityRotation;
					shaderColour_UO.UserTexture = userValues;
					shaderColour_UO.DisplayTexture = displayTexture;

					shaderColour_UO.SetWorldSpaceYAxis(ref worldSpaceYAxis);
					shaderColour_UO.SetVelocityScale(ref velScale);
					shader = shaderColour_UO;
				}
				else
				{
					shaderNoColour_UO = state.GetShader<DrawVelocityBillboardParticles_GpuTex3D_UserOffset>();

					shaderNoColour_UO.PositionTexture = positionTex;
					shaderNoColour_UO.VelocityTexture = velocityRotation;
					shaderNoColour_UO.UserTexture = userValues;
					shaderNoColour_UO.DisplayTexture = displayTexture;

					shaderNoColour_UO.SetWorldSpaceYAxis(ref worldSpaceYAxis);
					shaderNoColour_UO.SetVelocityScale(ref velScale);
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

				//bind
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
		/// implements the method to draw cpu processed particles
		/// </summary>
		protected override void DrawCpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint particleCount, AlphaBlendState blendMode, Vector4[] positionSize, Vector4[] velocityRotation, Vector4[] colourData, Vector4[] userValues)
		{
			//this is a bit more complex, but mostly the same as the GPU draw method
			Vector2 targetSize = state.DrawTarget.Size;

			state.RenderState.Push();
			state.RenderState.CurrentBlendState = blendMode;
			state.RenderState.CurrentDepthState.DepthWriteEnabled = false;

			Texture2D displayTexture = particleType.Texture ?? state.Properties.WhiteTexture;

			BillboardParticles2DElement.GenerateBillboardVertices(state, ref this.vertices, ref this.indices);

			Matrix cameraMatrix;
			state.Camera.GetCameraMatrix(out cameraMatrix);

			Vector3 worldSpaceYAxis = new Vector3(cameraMatrix.M21, cameraMatrix.M22, cameraMatrix.M23);


			Vector2 velScale = new Vector2(velocityScale, 0);
			if (this.useRotationToScaleVelocityEffect)
				velScale = new Vector2(0, velocityScale);



			int count = (int)particleCount;

			DrawVelocityBillboardParticles_BillboardCpu3D shaderNoColour = null;
			DrawVelocityBillboardParticlesColour_BillboardCpu3D shaderColour = null;

			if (colourData != null)
			{
				shaderColour = state.GetShader<DrawVelocityBillboardParticlesColour_BillboardCpu3D>();
				shaderColour.SetWorldSpaceYAxis(ref worldSpaceYAxis);
				shaderColour.SetVelocityScale(ref velScale);
			}
			else
			{
				shaderNoColour = state.GetShader<DrawVelocityBillboardParticles_BillboardCpu3D>();
				shaderNoColour.SetWorldSpaceYAxis(ref worldSpaceYAxis);
				shaderNoColour.SetVelocityScale(ref velScale);
			}

			int drawn = 0;
			while (count > 0)
			{
				int drawCount;

				drawCount = Math.Min(count, 75);
				uint drawCountU = (uint)drawCount;
				uint drawnU = (uint)drawn;

				if (colourData != null)
				{
					shaderColour.SetPositionData(positionSize, drawnU, 0, drawCountU);
					shaderColour.SetVelocityData(velocityRotation, drawnU, 0, drawCountU);
					shaderColour.SetColourData(colourData, drawnU, 0 , drawCountU);

					shaderColour.DisplayTexture = displayTexture;

					state.Shader.Push(shaderColour);
				}
				else
				{
					shaderNoColour.SetPositionData(positionSize, drawnU, 0, drawCountU);
					shaderNoColour.SetVelocityData(velocityRotation, drawnU, 0, drawCountU);

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
	}
}
