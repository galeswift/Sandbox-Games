using System;
using System.Collections.Generic;
using System.Text;
using Xen.Graphics;
using Xen.Camera;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Xen.Ex.Graphics.Display;
using Xen.Ex.Graphics2D;

namespace Xen.Ex.Graphics.Display
{
	/// <summary>
	/// Abstract base class that displays a particle system as a 2D Element
	/// </summary>
	public abstract class ParticleDrawer2DElement : Xen.Ex.Graphics2D.Element, Content.IParticleSystemDrawer
	{
		private readonly ParticleSystem system;
		private uint drawMask;
		private AlphaBlendState? alphaBlendState;

		/// <summary>
		/// Construct the particle system drawer
		/// </summary>
		/// <param name="system"></param>
		public ParticleDrawer2DElement(ParticleSystem system)
		{
			if (system == null)
				throw new ArgumentNullException();

			this.drawMask = 0xFFFFFFFF;
			this.system = system;
		}

		/// <summary>
		/// Gets the particle system for this drawer
		/// </summary>
		public ParticleSystem ParticleSystem 
		{
			get { return system; }
		}

		bool Content.IParticleSystemDrawer.Enabled
		{
			get { return this.Visible; }
			set { this.Visible = value; } 
		}

		//Element defines an AlphaBlendState property,
		//So in this case, provide a replacement that may be useful.

		/// <summary>
		/// <para>Set this property to a non-null value to override the blend state defined in the particle XML</para>
		/// <para>Note: All particle drawers perform RGB * Alpha modulation in the pixel shader, <see cref="Xen.Graphics.AlphaBlendState.SourceBlend"/> should be set <see cref="Blend.One"/> in place of <see cref="Blend.SourceAlpha"/>.</para>
		/// </summary>
		public new Nullable<AlphaBlendState> AlphaBlendState
		{
			get { return alphaBlendState; }
			set { alphaBlendState = value; }
		}

		/// <summary></summary>
		/// <param name="state"></param>
		protected override sealed void DrawElement(DrawState state)
		{
			system.DrawCallback(state, this);
		}

		/// <summary></summary>
		/// <param name="state"></param>
		/// <param name="maskOnly"></param>
		protected override sealed IShader BindShader(DrawState state, bool maskOnly)
		{
			return null;
		}

		/// <summary></summary>
		protected override sealed Vector2 ElementSize
		{
			get { return Vector2.Zero; }
		}
		/// <summary></summary>
		protected override sealed bool UseSize
		{
			get { return false; }
		}

		//this method is called back from the particle system
		void Content.IParticleSystemDrawer.DrawGpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint count, Texture2D positionSize, Texture2D velocityRotation, Texture2D colour, Texture2D userValues)
		{
			if ((drawMask & (1u << particleType.TypeIndex)) != 0)
				this.DrawGpuParticles(state, particleType, count, alphaBlendState ?? particleType.BlendMode, positionSize, velocityRotation, colour, userValues, particleType.GpuBufferPosition);
		}

		/// <summary>
		///Note: When 'usesUserValuesPositionBuffer' is true, the values 'user1, user2 and user3' (yzw in the UserTexture) store a position offset for the particle 
		/// </summary>
		protected abstract void DrawGpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint count, AlphaBlendState blendMode, Texture2D positionSize, Texture2D velocityRotation, Texture2D colour, Texture2D userValues, bool usesUserValuesPositionBuffer);

#if !XBOX360
		void Content.IParticleSystemDrawer.DrawCpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint count, Vector4[] positionSize, Vector4[] velocityRotation, Vector4[] colour, Vector4[] userValues)
		{
			if ((drawMask & (1u << particleType.TypeIndex)) != 0)
				this.DrawCpuParticles(state, particleType, count, alphaBlendState ?? particleType.BlendMode, positionSize, velocityRotation, colour, userValues);
		}

		/// <summary>
		/// This method is only present on Windows builds
		/// </summary>
		protected abstract void DrawCpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint count, AlphaBlendState blendMode, Vector4[] positionSize, Vector4[] velocityRotation, Vector4[] colour, Vector4[] userValues);
#endif

		/// <summary>
		/// Mask drawing for a specified particle type (enable or disable drawing of the particle type in the particle system)
		/// </summary>
		/// <param name="particleTypeName"></param>
		/// <param name="draw"></param>
		public void SetParticleTypeDrawMask(string particleTypeName, bool draw)
		{
			SetParticleTypeDrawMask(ref this.drawMask, this.system, particleTypeName, draw);
		}

		/// <summary>
		/// Mask drawing for a specified particle type (enable or disable drawing of the particle type in the particle system)
		/// </summary>
		/// <param name="particleType"></param>
		/// <param name="draw"></param>
		public void SetParticleTypeDrawMask(Xen.Ex.Graphics.Content.ParticleSystemTypeData particleType, bool draw)
		{
			SetParticleTypeDrawMask(ref this.drawMask, this.system, particleType, draw);
		}

		/// <summary>
		/// Mask drawing for a specified particle type (enable or disable drawing of the particle type in the particle system)
		/// </summary>
		/// <param name="particleTypeIndex"></param>
		/// <param name="draw"></param>
		public void SetParticleTypeDrawMask(int particleTypeIndex, bool draw)
		{
			SetParticleTypeDrawMask(ref this.drawMask, particleTypeIndex, draw);
		}
		/// <summary>
		/// Set a mask bit that will enable/disable drawing of all particle types
		/// </summary>
		/// <param name="draw"></param>
		public void SetParticleTypeDrawMaskAllTypes(bool draw)
		{
			this.drawMask = draw ? uint.MaxValue : 0;
		}

		static internal void SetParticleTypeDrawMask(ref uint mask, ParticleSystem system, Xen.Ex.Graphics.Content.ParticleSystemTypeData particleType, bool draw)
		{
			if (particleType == null)
				throw new ArgumentNullException();
			SetParticleTypeDrawMask(ref mask, system, particleType.Name, draw);
		}

		static internal void SetParticleTypeDrawMask(ref uint mask, ParticleSystem system, string particleTypeName, bool draw)
		{
			if (system == null)
				throw new InvalidOperationException("ParticleSystem == null");
			if (system.ParticleSystemData == null)
				throw new InvalidOperationException("ParticleSystem.ParticleSystemData == null");
			ReadOnlyArrayCollection<Xen.Ex.Graphics.Content.ParticleSystemTypeData> array = system.ParticleSystemData.ParticleTypeData;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Name == particleTypeName)
				{
					SetParticleTypeDrawMask(ref mask, i, draw);
					return;
				}
			}
			throw new ArgumentException(string.Format("ParticleSystem.ParticleSystemData.ParticleTypeData does not contain a particle type '{0}'",particleTypeName));
		}

		static internal void SetParticleTypeDrawMask(ref uint mask, int particleTypeIndex, bool draw)
		{
			if (particleTypeIndex < 0 ||
				particleTypeIndex > 31)
				throw new ArgumentException("Draw masking only supports the first 32 particle system types");

			if (draw)
				mask |= (1u << particleTypeIndex);
			else
				mask &= ~(1u << particleTypeIndex);
		}
	}

	/// <summary>
	/// Abstract base class that displays a particle system in 3D
	/// </summary>
	public abstract class ParticleDrawer3D : Content.IParticleSystemDrawer
	{
		private bool enabled;
		private readonly ParticleSystem system;
		private uint drawMask;
		private ICullable cullProxy;
		private AlphaBlendState? alphaBlendState;

		/// <summary>
		/// Construct the particle system drawer
		/// </summary>
		/// <param name="system"></param>
		protected ParticleDrawer3D(ParticleSystem system)
		{
			if (system == null)
				throw new ArgumentNullException();

			this.drawMask = 0xFFFFFFFF;
			this.system = system;
			this.enabled = true;
		}

		/// <summary>
		/// Get the partile system
		/// </summary>
		public ParticleSystem ParticleSystem { get { return system; } }
		/// <summary>
		/// Gets/Sets if this particle system drawer is enabled (visible)
		/// </summary>
		public bool Enabled { get { return enabled; } set { enabled = false; } }

		/// <summary>
		/// <para>Gets/Sets a culling proxy, to cull test this particle system</para>
		/// <para>Due to their nature, particle systems do not know their bouding voumes, so cannot cull test themselves</para>
		/// </summary>
		public ICullable CullProxy { get { return cullProxy; } set { cullProxy = value; } }

		/// <summary>
		/// <para>Set this property to a non-null value to override the blend state defined in the particle XML</para>
		/// <para>Note: All particle drawers perform RGB * Alpha modulation in the pixel shader, <see cref="Xen.Graphics.AlphaBlendState.SourceBlend"/> should be set <see cref="Blend.One"/> in place of <see cref="Blend.SourceAlpha"/>.</para>
		/// </summary>
		public Nullable<AlphaBlendState> AlphaBlendState
		{
			get { return alphaBlendState; }
			set { alphaBlendState = value; }
		}

		/// <summary>
		/// Mask drawing for a specified particle type (enable or disable drawing of the particle type in the particle system)
		/// </summary>
		/// <param name="particleTypeName"></param>
		/// <param name="draw"></param>
		public void SetParticleTypeDrawMask(string particleTypeName, bool draw)
		{
			ParticleDrawer2DElement.SetParticleTypeDrawMask(ref this.drawMask, this.system, particleTypeName, draw);
		}

		/// <summary>
		/// Mask drawing for a specified particle type (enable or disable drawing of the particle type in the particle system)
		/// </summary>
		/// <param name="particleType"></param>
		/// <param name="draw"></param>
		public void SetParticleTypeDrawMask(Xen.Ex.Graphics.Content.ParticleSystemTypeData particleType, bool draw)
		{
			ParticleDrawer2DElement.SetParticleTypeDrawMask(ref this.drawMask, this.system, particleType, draw);
		}

		/// <summary>
		/// Mask drawing for a specified particle type (enable or disable drawing of the particle type in the particle system)
		/// </summary>
		/// <param name="particleTypeIndex"></param><param name="draw"></param>
		public void SetParticleTypeDrawMask(int particleTypeIndex, bool draw)
		{
			ParticleDrawer2DElement.SetParticleTypeDrawMask(ref this.drawMask, particleTypeIndex, draw);
		}
		/// <summary>
		/// Set a mask bit that will enable/disable drawing of all particle types
		/// </summary><param name="draw"></param>
		public void SetParticleTypeDrawMaskAllTypes(bool draw)
		{
			this.drawMask = draw ? uint.MaxValue : 0;
		}

		/// <summary>
		/// Run a cull test using the optional <see cref="CullProxy"/>
		/// </summary>
		/// <param name="culler"></param>
		/// <returns></returns>
		public bool CullTest(ICuller culler)
		{
			return enabled && (cullProxy == null || cullProxy.CullTest(culler));
		}

		/// <summary>
		/// Draw the particle system
		/// </summary>
		/// <param name="state"></param>
		public void Draw(DrawState state)
		{
			system.DrawCallback(state,this);
		}

		void Validate()
		{
			if (system.SystemData == null)
				throw new InvalidOperationException("Attempting to draw a particle system that has not had it's ParticleSystemData content assigned");
		}

		//this method is called back from the particle system
		void Content.IParticleSystemDrawer.DrawGpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint count, Texture2D positionSize, Texture2D velocityRotation, Texture2D colour, Texture2D userValues)
		{
			if ((drawMask & (1u << particleType.TypeIndex)) != 0)
				this.DrawGpuParticles(state, particleType, count, alphaBlendState ?? particleType.BlendMode, positionSize, velocityRotation, colour, userValues, particleType.GpuBufferPosition);
		}

		
		/// <summary>
		/// <para>Method to override to draw 3D particles</para>
		/// <para>Note: When 'usesUserValuesPositionBuffer' is true, the values 'user1, user2 and user3' (yzw in the UserTexture) store a position offset for the particle</para></summary>
		/// <param name="state"></param><param name="particleType"></param><param name="count"></param><param name="blendMode"></param>
		/// <param name="positionSize"></param><param name="velocityRotation"></param><param name="colour"></param><param name="userValues"></param><param name="usesUserValuesPositionBuffer"></param>
		protected abstract void DrawGpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint count, AlphaBlendState blendMode, Texture2D positionSize, Texture2D velocityRotation, Texture2D colour, Texture2D userValues, bool usesUserValuesPositionBuffer);
		
#if !XBOX360
		void Content.IParticleSystemDrawer.DrawCpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint count, Vector4[] positionSize, Vector4[] velocityRotation, Vector4[] colour, Vector4[] userValues)
		{
			if ((drawMask & (1u << particleType.TypeIndex)) != 0)
				this.DrawCpuParticles(state, particleType, count, alphaBlendState ?? particleType.BlendMode, positionSize, velocityRotation, colour, userValues);
		}

		/// <summary>
		/// This method is only present on Windows builds
		/// </summary>
		/// <param name="blendMode"></param><param name="colour"></param><param name="count"></param><param name="particleType"></param>
		/// <param name="positionSize"></param><param name="state"></param><param name="userValues"></param><param name="velocityRotation"></param>
		protected abstract void DrawCpuParticles(DrawState state, Content.ParticleSystemTypeData particleType, uint count, AlphaBlendState blendMode, Vector4[] positionSize, Vector4[] velocityRotation, Vector4[] colour, Vector4[] userValues);
#endif

	}
}
