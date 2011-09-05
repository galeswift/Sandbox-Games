using System;
using System.Collections.Generic;
using System.Text;



using Xen;
using Xen.Camera;
using Xen.Graphics;
using Xen.Ex.Geometry;
using Xen.Ex.Material;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xen.Ex.Graphics.Content;
using Xen.Ex.Graphics2D;
using Xen.Ex.Graphics;


/*
 * This sample extends from Tutorial_01 (Application class)
 * This sample demonstrates:
 * 
 * Using a 3D Particle System
 * Using more than one Particle Drawer, and masking what they draw
 * 
 */
namespace Tutorials.Tutorial_22
{

	[DisplayName(Name = "Tutorial 22: Particle System (3D)")]
	public class Tutorial : Application
	{
		private DrawTargetScreen drawToScreen;

		//NEW CODE
		private ParticleSystem particles;

		//In this example, the fog particles and snow particles will be drawn by separate drawers,
		//even though they are both processed by the same particle system
		private Xen.Ex.Graphics.Display.ParticleDrawer3D fogDrawer;
		private Xen.Ex.Graphics.Display.ParticleDrawer3D snowDrawer;


		private Xen.Ex.Graphics2D.Statistics.DrawStatisticsDisplay stats;


		protected override void Initialise()
		{
			//draw targets usually need a camera.
			Xen.Camera.FirstPersonControlledCamera3D camera = new Xen.Camera.FirstPersonControlledCamera3D(this.UpdateManager);

			//don't allow the camera to move
			camera.MovementSensitivity *= 0;
			camera.Position = new Vector3(0, 5, 0);
			
			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = new Color(64,64,64);

			
			//create the particle system
			this.particles = new ParticleSystem(this.UpdateManager);

			//the snow particles will be drawn as velocity particles
			this.snowDrawer = new Xen.Ex.Graphics.Display.VelocityBillboardParticles3D(this.particles, false, 0.1f);

			//the fog particles will be drawn as normal billboards
			this.fogDrawer = new Xen.Ex.Graphics.Display.BillboardParticles3D(this.particles);

			//add a ground plane to show the horizon
			drawToScreen.Add(new DarkGroundPlane(new Vector4(0.225f, 0.225f, 0.225f, 1f)));

			//add the particles
			drawToScreen.Add(fogDrawer);
			drawToScreen.Add(snowDrawer);

			//Note: The particle drawers are masked in the LoadContent method

			//add draw stats
			stats = new Xen.Ex.Graphics2D.Statistics.DrawStatisticsDisplay(this.UpdateManager);
			drawToScreen.Add(stats);

		}

		protected override void LoadContent(ContentState state)
		{
			this.stats.Font = state.Load<SpriteFont>("Arial");
			this.particles.ParticleSystemData = state.Load<ParticleSystemData>("Particles/Snow");
			
			//NEW CODE
			//Mask rendering for the different particle drawers.
			//This allows the snow and fog particles to be drawn using different drawing methods

			this.fogDrawer.SetParticleTypeDrawMask("fog", true);
			this.fogDrawer.SetParticleTypeDrawMask("snow", false);

			this.snowDrawer.SetParticleTypeDrawMask("fog", false);
			this.snowDrawer.SetParticleTypeDrawMask("snow", true);

			//this will only have effect in DEBUG pc builds
			ParticleSystemHotLoader.Monitor(this.UpdateManager, this.particles, @"..\..\..\..\..\bin\Xen.Graphics.ShaderSystem\");
		}

		//main application draw method
		protected override void Frame(FrameState state)
		{
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

		protected override void InitialisePlayerInput(Xen.Input.PlayerInputCollection playerInput)
		{
			if (playerInput[PlayerIndex.One].ControlInput == Xen.Input.ControlInput.KeyboardMouse)
				playerInput[PlayerIndex.One].InputMapper.CentreMouseToWindow = true;
		}
	}

	//this is used to add some contrast to the scene, and show the ground / horizon
	internal class DarkGroundPlane : IDraw
	{
		private readonly IVertices vertices;
		private Vector4 colour;

		public DarkGroundPlane(Vector4 colour)
		{
			this.colour = colour;

			//make a ground plane using two triangles
			float extents = 1000;

			Vector3[] positions = new Vector3[]
			{
				new Vector3(-extents,-5,-extents),
				new Vector3( extents,-5,-extents),
				new Vector3(-extents,-5, extents),
				new Vector3( extents,-5, extents),
			};

			this.vertices = new Vertices<Vector3>(positions);
		}

		public void Draw(DrawState state)
		{
			Xen.Ex.Shaders.FillSolidColour shader = state.GetShader<Xen.Ex.Shaders.FillSolidColour>();

			shader.FillColour = colour;

			using (state.Shader.Push(shader))
				vertices.Draw(state, null, PrimitiveType.TriangleStrip);
		}

		public bool CullTest(ICuller culler)
		{
			return true;
		}
	}
}
