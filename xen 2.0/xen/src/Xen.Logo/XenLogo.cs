using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xen;
using Microsoft.Xna.Framework;

namespace Xen.Logo
{
	//A special content manager that extracts content embedded within the binary .dll
	sealed class ContentLoader : Microsoft.Xna.Framework.Content.ContentManager
	{
		string [] names = typeof(ContentLoader).Assembly.GetManifestResourceNames();

		public ContentLoader(IServiceProvider services)
			: base(services)
		{
		}

		//loads the particle content from file embedded in the assembly..
		protected override System.IO.Stream OpenStream(string assetName)
		{
			assetName += ".xnb";

			foreach (string name in names)
			{
				if (name.EndsWith(assetName))
					return typeof(ContentLoader).Assembly.GetManifestResourceStream(name);
			}
			return null;
		}
	}

	/// <summary>
	/// Displays the Xen Graphics API logo for approximately 8 seconds
	/// </summary>
	public sealed class XenLogo : IDraw, IUpdate, IDisposable
	{
		private Xen.Ex.Graphics.ParticleSystem particles;
		private Xen.Graphics.Modifier.ClearBufferModifier clear;
		private VelocityBillboardParticles2DElement particleDrawer;
		private Xen.Ex.Graphics.Content.ParticleSystemTrigger particleTrigger;
		private bool drawingBegun, particlesCreated, clearBackground;
		private ContentLoader content;
		private int lineIndex;

		/// <summary>Construct the logo</summary>
		public XenLogo(UpdateManager updateManager)
		{
			if (updateManager == null)
				throw new ArgumentNullException();

			content = new ContentLoader(Application.GetApplicationInstance().Services);

			this.clearBackground = true;
			
			//setup the system and drawer
			this.particles = new Xen.Ex.Graphics.ParticleSystem(updateManager);
			this.particleDrawer = new VelocityBillboardParticles2DElement(this.particles, false, 0.025f);

			//align it...
			this.particleDrawer.VerticalAlignment = Xen.Ex.Graphics2D.VerticalAlignment.Centre;
			this.particleDrawer.HorizontalAlignment = Xen.Ex.Graphics2D.HorizontalAlignment.Centre;

			//clear background is optional.
			this.clear = new Xen.Graphics.Modifier.ClearBufferModifier(Microsoft.Xna.Framework.Graphics.ClearOptions.Target);
			this.clear.ClearColour = new Microsoft.Xna.Framework.Color();

			//load the data straight from the assembly.
			this.particles.ParticleSystemData = content.Load<Xen.Ex.Graphics.Content.ParticleSystemData>("Logo");
			this.particleTrigger = this.particles.GetTriggerByName("Fire");

			updateManager.Add(this);
		}

		/// <summary>
		/// Draw the logo
		/// </summary>
		public void Draw(DrawState state)
		{
			if (clearBackground)
				this.clear.Draw(state);

			if (EffectFinished)
			{
				if (this.particles != null)
					this.particles.Dispose();
				if (content != null)
					this.content.Dispose();
				this.particleDrawer = null;
				this.particles = null;
				this.particleTrigger = null;
				this.content = null;
			}
			else
			{
				//x axis min/max start range for the particles to spawn in at
				this.particles.GlobalValues[0] = -state.DrawTarget.Size.X / 2;
				this.particles.GlobalValues[1] = 0;

				//y axis min/max start range for the particles to spawn in at
				this.particles.GlobalValues[2] = -state.DrawTarget.Size.Y / 2 - 200;
				this.particles.GlobalValues[3] = +state.DrawTarget.Size.Y / 2 + 200;

				drawingBegun = true;

				if (particles.GetParticleCount(0) > 0)
					particlesCreated = true;

				this.particleDrawer.Draw(state);
			}
		}

		bool ICullable.CullTest(ICuller culler)
		{
			return true;
		}

		/// <summary>
		/// Gets/Sets if the effect will clear the background colour to black
		/// </summary>
		public bool ClearBackground { get { return clearBackground; } set { clearBackground = value; } }

		/// <summary>
		/// This value will return true when the effect has completed
		/// </summary>
		public bool EffectFinished
		{
			get { return particles == null || (particlesCreated && this.particles.GetParticleCount(0) == 0); }
		}

		UpdateFrequency IUpdate.Update(UpdateState state)
		{
			// 480 170
			//this.particles


			//image is 480x170,
			//create 8 vertical lines of particles per tick, moving from left to right
			if (drawingBegun && this.particles != null)
			{
				for (int loop = 0; loop < 8; loop++)
				{
					float x = lineIndex - 240;
					float u = (float)lineIndex / 480.0f;
					Vector3 velocity = new Vector3(0, 0, 0);

					if (lineIndex < 480)
					{
						float y = -170 / 2;
						float v = 0;

						for (int i = 0; i < 170; i++)
						{
							//pos the particle will end up in
							Vector3 pos = new Vector3(x, y, 0);
							//uv for the particle, stored as colour data. Read in custom shader
							Vector4 uv = new Vector4(u + 0.5f / 480.0f, 1 - v + 0.5f / 240.0f, 0, 0);

							//create the particle.
							this.particleTrigger.FireTrigger(ref pos, 0.75f, ref velocity, 1, ref uv);
							y++;
							v += 1.0f / 170.0f;
						}
					}
					lineIndex++;
				}

				if (particles.GetParticleCount(0) > 0)
					particlesCreated = true;
			}

			if (EffectFinished)
				return UpdateFrequency.Terminate;

			return UpdateFrequency.FullUpdate60hz;
		}

		/// <summary>
		/// Dispose the effect resources early. The resources will automatically be disposed when the effect completes.
		/// </summary>
		public void Dispose()
		{
			if (this.particles != null)
				this.particles.Dispose();
			if (content != null)
				this.content.Dispose();

			this.particleDrawer = null;
			this.particles = null;
			this.particleTrigger = null;
			this.content = null;
		}
	}
}
