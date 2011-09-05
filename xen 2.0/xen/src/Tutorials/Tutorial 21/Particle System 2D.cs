using System;
using System.Collections.Generic;
using System.Text;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Xen;
using Xen.Camera;
using Xen.Graphics;

using Xen.Ex.Geometry;
using Xen.Ex.Material;
using Xen.Ex.Graphics.Content;
using Xen.Ex.Graphics2D;
using Xen.Ex.Graphics;


/*
 * This sample extends from Tutorial_01 (Application class)
 * This sample demonstrates:
 * 
 * Creating an instance of a Particle System
 * Using a Displayer to draw the Particle System
 * 
 * 
 */
namespace Tutorials.Tutorial_21
{

	[DisplayName(Name = "Tutorial 21: Particle System (2D)")]
	public class Tutorial : Application
	{
		private DrawTargetScreen drawToScreen;

		//NEW CODE
		//the instance of the particle system.
		//A ParticleSystem instance performs the logic required to process the particles
		//This class does not directly draw the particles
		private ParticleSystem particles;

		//An insance of a particle drawer (ParticleDrawer2DElement is an abstract class)
		private Xen.Ex.Graphics.Display.ParticleDrawer2DElement particleDrawer;


		protected override void Initialise()
		{
			Camera2D camera = new Camera2D();
			
			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = Color.Black;


			// uncomment this to force theparticle system to run on the CPU
			// Note: This property is not available on the xbox.

			//ParticleSystem.ForceUseCpuParticleSystem = true;

			// Use ParticleSystem.SystemSupportsGpuParticles to determine if the particle
			// system will be run on the GPU
			//
			// NOTE: When running on the GPU, the particle system will be processed in 16bit floating point
			// The CPU particle system is not supported on the Xbox.



			//NEW CODE
			//create the particle system
			this.particles = new ParticleSystem(this.UpdateManager);

			//Like ModelInstance, the ParticleSystem content must be assigned before it can be drawn

			//create the particle drawer,
			//In this case, use a VelocityBillboardParticles2DElement.
			//This is a special particle drawer that will 'stretch' particles in the direction they are
			//travelling - giving them a streaky look.
			//
			particleDrawer = new Xen.Ex.Graphics.Display.VelocityBillboardParticles2DElement(this.particles, true);
			
			//align the drawer to the bottom centre of the screen
			particleDrawer.VerticalAlignment = VerticalAlignment.Bottom;
			particleDrawer.HorizontalAlignment = HorizontalAlignment.Centre;

			//add it to ths screen
			drawToScreen.Add(particleDrawer);
		}

		protected override void LoadContent(ContentState state)
		{
			//NEW CODE
			//load the particle system data
			this.particles.ParticleSystemData = state.Load<ParticleSystemData>("Particles/Fireworks");


			//This is a special feature for DEBUG PC builds (this method does nothing otherwise).
			//This enables runtime particle system hotloading.
			//Hotloading will reload the particle system if the source file changes, allowing
			//runtime tweaking and editing of the particle system.
			//NOTE: Reloading complex particle systems may be unreliable

			ParticleSystemHotLoader.Monitor(this.UpdateManager, this.particles, @"..\..\..\..\..\bin\Xen.Graphics.ShaderSystem\");
		}


		//main application draw method
		protected override void Frame(FrameState state)
		{
			//draw to the screen.
			drawToScreen.Draw(state);
		}

		protected override void SetupGraphicsDeviceManager(GraphicsDeviceManager graphics, ref RenderTargetUsage presentation)
		{
			if (graphics != null) // graphics is null when starting within a WinForms host
			{
				graphics.PreferredBackBufferWidth = 1280;
				graphics.PreferredBackBufferHeight = 720;
			}
		}

		protected override void Update(UpdateState state)
		{
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}
	}
}
