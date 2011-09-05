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
using Xen.Ex.Graphics;

namespace Xen.Logo
{
	/// <summary>
	/// <para>Draws 2D <see cref="Element"/> particles as Billboard Sprites (Rotation is generated per particle)</para>
	/// <para>Particles will be scaled by thier velocity length, and rotated in the direction they are travelling</para>
	/// <para>For use with a <see cref="ParticleSystem"/> instance.</para>
	/// </summary>
	internal sealed class VelocityBillboardParticles2DElement : ParticleDrawer2DElement
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
		public VelocityBillboardParticles2DElement(ParticleSystem system, bool useRotationValueToScaleVelocityEffect, float velocityExtentionScale)
			: base(system)
		{
			this.UseRotationValueToScaleVelocityEffect = useRotationValueToScaleVelocityEffect;
			this.VelocityExtentionScale = velocityExtentionScale;
		}

		/// <summary>
		/// Construct the particle drawer
		/// </summary>
		/// <param name="system"></param>
		/// <param name="useRotationValueToScaleVelocityEffect"><para>When true, the per particle rotation value will be used to scale the velocity stretching effect</para><para>Allowing per-particle scaling based on velocity</para></param>
		public VelocityBillboardParticles2DElement(ParticleSystem system, bool useRotationValueToScaleVelocityEffect)
			: base(system)
		{
			this.useRotationToScaleVelocityEffect = useRotationValueToScaleVelocityEffect;
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
		/// draws the particles on a GPU system
		/// </summary>
		protected override void DrawGpuParticles(DrawState state, Xen.Ex.Graphics.Content.ParticleSystemTypeData particleType, uint particleCount, AlphaBlendState blendMode, Texture2D positionTex, Texture2D velocityRotation, Texture2D colourTex, Texture2D userValues, bool usesUserValuesPositionBuffer)
		{
			//this is very similar to the billboard drawer (see it for reference)

			using (state.RenderState.Push())
			{
				state.RenderState.CurrentBlendState = blendMode;
				state.RenderState.CurrentDepthState.DepthWriteEnabled = false;

				Texture2D displayTexture = particleType.Texture;

				//get the shared vertice
				BillboardParticles2DElement.GenerateBillboardVertices(state, ref vertices, ref indices);

				int count = (int)particleCount;

				DrawVelocityParticles_GpuTex shaderNoColour = null;
				DrawVelocityParticlesColour_GpuTex shaderColour = null;
				//user variants
				DrawVelocityParticles_GpuTex_UserOffset shaderNoColour_UO = null;
				DrawVelocityParticlesColour_GpuTex_UserOffset shaderColour_UO = null;

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
						shader = shaderColour = state.GetShader<DrawVelocityParticlesColour_GpuTex>();

						shaderColour.PositionTexture = positionTex;
						shaderColour.ColourTexture = colourTex;
						shaderColour.VelocityTexture = velocityRotation;
						shaderColour.DisplayTexture = displayTexture;

						shaderColour.SetVelocityScale(ref velScale);
					}
					else
					{
						shader = shaderNoColour = state.GetShader<DrawVelocityParticles_GpuTex>();

						shaderNoColour.PositionTexture = positionTex;
						shaderNoColour.VelocityTexture = velocityRotation;
						shaderNoColour.DisplayTexture = displayTexture;

						shaderNoColour.SetVelocityScale(ref velScale);
					}
				}
				else
				{
					if (colourTex != null)
					{
						shader = shaderColour_UO = state.GetShader<DrawVelocityParticlesColour_GpuTex_UserOffset>();

						shaderColour_UO.PositionTexture = positionTex;
						shaderColour_UO.ColourTexture = colourTex;
						shaderColour_UO.VelocityTexture = velocityRotation;
						shaderColour_UO.UserTexture = userValues;
						shaderColour_UO.DisplayTexture = displayTexture;

						shaderColour_UO.SetVelocityScale(ref velScale);
					}
					else
					{
						shader = shaderNoColour_UO = state.GetShader<DrawVelocityParticles_GpuTex_UserOffset>();

						shaderNoColour_UO.PositionTexture = positionTex;
						shaderNoColour_UO.VelocityTexture = velocityRotation;
						shaderNoColour_UO.UserTexture = userValues;
						shaderNoColour_UO.DisplayTexture = displayTexture;

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
						vertices.Draw(state, indices, PrimitiveType.TriangleList, drawCount * 2, 0, 0);

					count -= drawCount;
					drawn += drawCount;
				}


			}
		}

#if !XBOX360
		//draws on CPU particle systems
		/// <summary>
		/// draws the particles from a CPU system
		/// </summary>
		protected override void DrawCpuParticles(DrawState state, Xen.Ex.Graphics.Content.ParticleSystemTypeData particleType, uint particleCount, AlphaBlendState blendMode, Vector4[] positionSize, Vector4[] velocityRotation, Vector4[] colourData, Vector4[] userValues)
		{
			//this is very similar to the billboard drawer (see it for reference)
			Vector2 targetSize = state.DrawTarget.Size;

			using (state.RenderState.Push())
			{
				state.RenderState.CurrentBlendState = blendMode;
				state.RenderState.CurrentDepthState.DepthWriteEnabled = false;

				Texture2D displayTexture = particleType.Texture;

				BillboardParticles2DElement.GenerateBillboardVertices(state, ref this.vertices, ref this.indices);


				int count = (int)particleCount;



				DrawVelocityParticles_BillboardCpu shaderNoColour = null;
				DrawVelocityParticlesColour_BillboardCpu shaderColour = null;


				Vector2 velScale = new Vector2(velocityScale, 0);
				if (this.useRotationToScaleVelocityEffect)
					velScale = new Vector2(0, velocityScale);

				if (colourData != null)
				{
					shaderColour = state.GetShader<DrawVelocityParticlesColour_BillboardCpu>();

					shaderColour.DisplayTexture = displayTexture;
					shaderColour.SetVelocityScale(ref velScale);
				}
				else
				{
					shaderNoColour = state.GetShader<DrawVelocityParticles_BillboardCpu>();

					shaderNoColour.DisplayTexture = displayTexture;
					shaderNoColour.SetVelocityScale(ref velScale);
				}

				int drawn = 0;
				while (count > 0)
				{
					int drawCount;

					drawCount = Math.Min(count, 80);
					IShader shader = null;

					if (colourData != null)
					{
						shaderColour.SetPositionData(positionSize, (uint)drawn, 0, (uint)drawCount);
						shaderColour.SetVelocityData(velocityRotation, (uint)drawn, 0, (uint)drawCount);
						shaderColour.SetColourData(colourData, (uint)drawn, 0, (uint)drawCount);

						shader = shaderColour;
					}
					else
					{
						shaderNoColour.SetPositionData(positionSize, (uint)drawn, 0, (uint)drawCount);
						shaderNoColour.SetVelocityData(velocityRotation, (uint)drawn, 0, (uint)drawCount);

						shader = shaderNoColour;
					}

					using (state + shader)
						vertices.Draw(state, indices, PrimitiveType.TriangleList, drawCount * 2, 0, 0);

					count -= drawCount;
					drawn += drawCount;
				}
			}
		}
#endif
	}
}
