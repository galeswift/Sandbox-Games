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
using Xen.Ex.Graphics.Display;


/*
 * This sample extends from Tutorial_22 (Particle Systems 3D)
 * This sample demonstrates:
 * 
 * Drawing many instances of two 3D Particle Systems
 * Cull Testing particle drawers and sorting them
 * 
 */
namespace Tutorials.Tutorial_23
{

	// The most expensive part of a particle system is the ParticleSystem class itself
	//
	// This class will allocate memory for every particle generated, and for particles
	// processed on the GPU, large render targets are created to store particle data.
	//
	// For this reason it is recommended to use as few ParticleSystem classes as is
	// reasonable (However merging ParticleSystems together will have little advantage)
	// Using multiple ParticleSystem instances that generate the same particle types is
	// strongly dicouraged.
	//
	// For example, this tutorial draws 10 fire effects, which are defined within
	// 'fire.particles' and 'smoke.particles'. Merging these two files would have
	// little, if any, performance benefit (and in some cases, be a performance loss)
	//
	// However creating a ParticleSystem for each fire that is displayed would be a 
	// *significant* performance/memory hit. The combined smoke and fire effects can
	// generate around 100,000 particles - which have to be calculated every frame and
	// stored in memory. Duplicating the ParticleSystem classes would add enormous
	// overhead.
	//
	// In this example, each fire draws the same ParticleSystem effects. This adds no
	// extra overhead for the ParticleSystem classes.

	// One major limitation of the particle system, is that the system does not know
	// it's bounding volume.
	// This could be calculated, however for the GPU processor this would be a very
	// tricky task (that would add a lot of processing *and* memory overhead) - and
	// would also be out of sync with the CPU (by up to 3 frames).
	// The last problem cannot be solved without potentially stalling the CPU, so
	// as a result particle systems simply do not know their bounding volume.
	//
	// This means the 3D particle drawers can not CullTest themselves efficiently.
	// As a result, in order to use a 3D particle system efficiently, it is 
	// recommended to CullTest them manually, using approximate bounding volumes.
	//
	// The ParticleDrawer3D class includes a 'CullProxy' property, which allows
	// an ICullable to be attached to the particle system. This will act as the
	// CullTest() logic for the system.
	// 
	// However, in this tutorial, the particle drawers are shared between
	// multiple fire effects, so the culling is performed manually.
	//
	// Which is exactly what this class does...
	class CullableParticleWrapper : IDraw
	{
		//the particle system to display
		private ParticleDrawer3D particleDrawer;
		private Matrix worldMatrix;

		//the offset of the bounding sphere used to cull the effect
		private Vector3 sphereOffset;
		//the radius of the bounding sphere
		private float sphereRadius;

		public CullableParticleWrapper(ParticleDrawer3D particleDrawer, Vector3 worldPosition, Vector3 cullSphereOffset, float cullSphereRadius)
		{
			//set the world matrix
			Matrix.CreateTranslation(ref worldPosition, out this.worldMatrix);

			//put the culling sphere into world space of the system
			this.sphereOffset = Vector3.Transform(cullSphereOffset, this.worldMatrix);
			this.sphereRadius = cullSphereRadius;

			this.particleDrawer = particleDrawer;
		}

		public void Draw(DrawState state)
		{
			//draw the particles (cull test has passed)
			using (state.WorldMatrix.PushMultiply(ref worldMatrix))
			{
				this.particleDrawer.Draw(state);
			}
		}

		public bool CullTest(ICuller culler)
		{
			//apply the manual cull test
			return culler.TestSphere(this.sphereRadius, ref this.sphereOffset);
		}
	}

	// This is the recommended method for processing Looping particle effects.
	// However, some effects fire intermittently, such as an explosion effect or a
	// puff of dust when two objects collide.
	//
	// These are often very short lived effects, and creating a ParticleSystem instance
	// every time one occurs would be very bad for performance (creating and disposing
	// ParticleSystems is expensive).
	//
	// In such a case, a recommended method would be to use a Trigger defined in the
	// particle system. This trigger can be fired by the application on request.
	// The application can store a single ParticleSystem for the effect, drawn with a
	// non-culled particle drawer.
	//
	// Culling can be performed by simply not firing the Trigger if it's off screen.
	// For most effects, this will be an adequate compromise.

	// This class does approximately this.
	// Because this effect is intended for effects that are very short lived and
	// do not require perfect accuracy / culling, this is all processed in the 'Draw'
	// method.
	// The particle system is setup to allocate enough memory for 25 concurrent 'burst'
	// effects to be firing.
	class BurstSource : IDraw
	{
		private bool burstEnabled; // is enabled? (set by the tutorial)
		private float burstTimer; // when above 2, the burst is fired

		//the trigger to fire
		private ParticleSystemTrigger burstTrigger;

		//the position to fire from
		private Vector3 effectPosition;
		private float effectRadius;

		//the trigger is Content, so can't be set right yet
		public BurstSource(Vector3 position, float radius, float randomTimeValue)
		{
			burstTimer = randomTimeValue;

			this.effectPosition = position;
			this.effectRadius = radius;
		}

		//called at content load time
		public void SetBurstTrigger(Xen.Ex.Graphics.Content.ParticleSystemTrigger trigger)
		{
			this.burstTrigger = trigger;
		}

		//The burst will fire every 2 seconds while this instance is visible and 'burstEnabled' is true.
		//This will cause the effect to get out of sync quite easily (intended).
		public void Draw(DrawState state)
		{
			if (burstEnabled && burstTrigger != null)
			{
				burstTimer += state.DeltaTimeSeconds;

				if (burstTimer > 2.0f)
				{
					//Fire the burst! (this will become visible in the next frame)
					burstTrigger.FireTrigger(ref this.effectPosition);

					burstTimer -= 2.0f;
				}
			}
		}

		public bool CullTest(ICuller culler)
		{
			return culler.TestSphere(this.effectRadius, ref this.effectPosition);
		}

		public void SetEnabled(bool enabled)
		{
			this.burstEnabled = enabled;
		}
	}

	//
	// There is one extra feature the Burst effect uses:
	//
	// The burst effect uses particles that are emitted at positions that are a long
	// distance from the origin.
	// This is fine for the CPU particle processor, however, with the GPU processor
	// particle data is stored in 16bit floating point.
	//
	// The limitation of this format become obvious here, as particles at positions
	// such as '50,10,20' may have a very small velocity, requring subtle movement.
	// This causes problems, as the change in position may be as little as 0.001
	// per frame. This accuracy cannot be represented in 16bit float, so the
	// particles appear in 'layers' when emitted. Their randomness appears reduced.
	//
	// To help aleveate this, the GPU particle system can store the original position
	// of the particle (the position the particle is emitted from) in User Values.
	// The position is then calculated starting at 0,0,0 instead of, say, 50,10,20.
	//
	// The particle system will no longer have access to user1, user2 or user3
	// (user0 is still accessible).
	// This flag can be set with 'gpu_buffer_position' in the particle type
	// definition in the XML.
	//
	// Set this value to false and the burst effect will appear far less random,
	// especially at the farthest emitters.
	// 

	// This class is used as a CullProxy for the burst particle effect drawer
	// This method of culling probably isn't the most efficient :-)
	class BurstCullProxy : ICullable
	{
		//all the burst sources
		private BurstSource[] bursts;

		public BurstCullProxy(params BurstSource[] bursts)
		{
			this.bursts = bursts;
		}

		//cull test ALL of them :-)
		public bool CullTest(ICuller culler)
		{
			bool visible = false;
			for (int i = 0; i < this.bursts.Length; i++)
				visible |= this.bursts[i].CullTest(culler);

			return visible;
		}
	}

	//This is a very simple class that draws a list of items.
	//It is used to compare sorted drawing with unsorted drawing
	public class DrawList : IDraw
	{
		private readonly List<IDraw> children = new List<IDraw>();
		private bool visible = true;

		//add an item
		public void Add(IDraw child)
		{
			this.children.Add(child);
		}

		//draw the items
		public void Draw(DrawState state)
		{
			foreach (IDraw child in children)
			{
				if (child.CullTest(state))
					child.Draw(state);
			}
		}

		public bool CullTest(ICuller culler)
		{
			return visible;
		}

		//set visibility
		public bool Visible
		{
			get { return visible; }
			set { visible = value; }
		}
	}


	[DisplayName(Name = "Tutorial 23: Advanced Particle System (3D)")]
	public class Tutorial : Application
	{
		private DrawTargetScreen drawToScreen;

		//the particle system processors (only one of each)
		private ParticleSystem fireParticleSystem;
		private ParticleSystem smokeParticleSystem;
		private ParticleSystem burstParticleSystem;

		private BurstSource[] burstSources;

		//draw the scene in either sorted or unsorted order
		private Xen.Ex.Scene.DepthDrawSorter drawSorted;
		private DrawList drawUnsorted;

		private Xen.Ex.Scene.CullTestVisualizer cullTestVisualizer;
		private Xen.Ex.Graphics2D.Statistics.DrawStatisticsDisplay stats;

		//help text
		private TextElement text;

		protected override void Initialise()
		{
			//draw targets usually need a camera.
			var camera = new Xen.Camera.FirstPersonControlledCamera3D(this.UpdateManager, Vector3.Zero, false);

			//don't allow the camera to move too fast
			camera.MovementSensitivity *= 0.1f;
			camera.LookAt(new Vector3(0,3,0), new Vector3(1, 5, 10), new Vector3(0, 1, 0));
			
			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = new Color(45,50,60);

			
			//create the fire and smoke particle system
			this.fireParticleSystem = new ParticleSystem(this.UpdateManager);
			this.smokeParticleSystem = new ParticleSystem(this.UpdateManager);

			//IMPORTANT
			//The following flags are FALSE by default.
			//For looping effects, such as the fire and smoke, it's highly
			//recommended to enable this flag. Otherwise, while the effect
			//is offscreen, the particle system will continue to process.
			this.fireParticleSystem.PauseUpdatingWhileCulled = true;
			this.smokeParticleSystem.PauseUpdatingWhileCulled = true;


			this.drawSorted = new Xen.Ex.Scene.DepthDrawSorter(Xen.Ex.Scene.DepthSortMode.BackToFront);
			this.drawUnsorted = new DrawList();

			var fireDrawer = new Xen.Ex.Graphics.Display.VelocityBillboardParticles3D(this.fireParticleSystem, true);
			var smokeDrawer = new Xen.Ex.Graphics.Display.BillboardParticles3D(this.smokeParticleSystem);

			for (int i = 0; i < 10; i++)
			{
				Vector3 position = new Vector3((float)Math.Cos(i * Math.PI / 5.0) * 6.0f, 0, (float)Math.Sin(i * Math.PI / 5.0) * 6.0f);
				
				CullableParticleWrapper fireEffect, smokeEffect;

				fireEffect = new CullableParticleWrapper(fireDrawer, position, new Vector3(0, 2, 0), 4);
				smokeEffect = new CullableParticleWrapper(smokeDrawer, position, new Vector3(0, 6, 0), 5);

				this.drawSorted.Add(fireEffect);
				this.drawSorted.Add(smokeEffect);

				this.drawUnsorted.Add(fireEffect);
				this.drawUnsorted.Add(smokeEffect);

				var light = new GroundLightDisk(position);
				this.drawSorted.Add(light);
				this.drawUnsorted.Add(light);
			}


			//setup the burst effect
			this.burstParticleSystem = new ParticleSystem(this.UpdateManager);

			//for this case, PauseUpdatingWhileCulled is not set to true.
			//The particle emitting is culled when offscreen. If set to true,
			//Any particles left offscreen could 'pause', when they naturally
			//wouldn't be emitted anyway.
			//(The particle system will use very few resources when it has no
			//active particles)

			this.burstSources = new BurstSource[20];
			Random rand = new Random();

			for (int i = 0; i < this.burstSources.Length; i++)
			{
				//create the bursts out in the distance
				Vector3 position = new Vector3((float)i * 5.0f - this.burstSources.Length * 2.5f, 0, -20); 
				float radius = 10; // with a decent radius

				//give them a random starting time
				this.burstSources[i] = new BurstSource(position, radius, (float)rand.NextDouble() * 2);

				this.drawSorted.Add(this.burstSources[i]);
				this.drawUnsorted.Add(this.burstSources[i]);
			}

			//the bursts need to be drawn as a group..
			var burstDrawer = new Xen.Ex.Graphics.Display.VelocityBillboardParticles3D(this.burstParticleSystem,false,0.5f);

			this.drawSorted.Add(burstDrawer);
			this.drawUnsorted.Add(burstDrawer);

			//Use all the burst sources to cull the drawer (may not be ideal if there were many sources...)
			//Use the particle drawer CullProxy to do it
			burstDrawer.CullProxy = new BurstCullProxy(this.burstSources);
			


			//add a ground plane to show the horizon
			drawToScreen.Add(new Tutorial_22.DarkGroundPlane(new Vector4(0.125f,0.15f,0.135f,1)));

			//add the sorted and unsorted lists
			drawToScreen.Add(drawSorted);
			drawToScreen.Add(drawUnsorted);


			//finally, create a CullTestVisualizer, which will visually show the cull tests performed
			cullTestVisualizer = new Xen.Ex.Scene.CullTestVisualizer();

			//the visualizer is added as a draw modifier
			this.drawToScreen.AddModifier(cullTestVisualizer);

			//add help text
			this.text = new TextElement();
			this.text.VerticalAlignment = VerticalAlignment.Bottom;
			this.text.Position = new Vector2(50, 100);
			drawToScreen.Add(this.text);

			//add draw stats
			stats = new Xen.Ex.Graphics2D.Statistics.DrawStatisticsDisplay(this.UpdateManager);
			drawToScreen.Add(stats);
		}

		protected override void LoadContent(ContentState state)
		{
			SpriteFont font = state.Load<SpriteFont>("Arial");
			this.stats.Font = font;
			this.text.Font = font;

			//load the particle systems
			this.fireParticleSystem.ParticleSystemData = state.Load<ParticleSystemData>("Particles/Fire");
			this.smokeParticleSystem.ParticleSystemData = state.Load<ParticleSystemData>("Particles/Smoke");
			this.burstParticleSystem.ParticleSystemData = state.Load<ParticleSystemData>("Particles/Burst");

			//set the trigger for the burst effect
			ParticleSystemTrigger trigger = this.burstParticleSystem.GetTriggerByName("burst");

			for (int i = 0; i < this.burstSources.Length; i++)
				this.burstSources[i].SetBurstTrigger(trigger);


			//this will only have effect in DEBUG pc builds
			ParticleSystemHotLoader.Monitor(this.UpdateManager, this.smokeParticleSystem, @"..\..\..\..\..\bin\Xen.Graphics.ShaderSystem\");
			ParticleSystemHotLoader.Monitor(this.UpdateManager, this.fireParticleSystem, @"..\..\..\..\..\bin\Xen.Graphics.ShaderSystem\");
			ParticleSystemHotLoader.Monitor(this.UpdateManager, this.burstParticleSystem, @"..\..\..\..\..\bin\Xen.Graphics.ShaderSystem\");
		}

		//main application draw method
		protected override void Frame(FrameState state)
		{
			drawToScreen.Draw(state);
		}


		protected override void Update(UpdateState state)
		{
			//burst button
			bool enableBurst = state.PlayerInput[PlayerIndex.One].InputState.Buttons.A;
			for (int i = 0; i < this.burstSources.Length; i++)
				this.burstSources[i].SetEnabled(enableBurst);

			//sorted order button
			bool drawInSortedOrder = state.PlayerInput[PlayerIndex.One].InputState.Buttons.B == false;

			drawSorted.Visible = drawInSortedOrder;
			drawUnsorted.Visible = !drawInSortedOrder;

			//control tutorial input
			cullTestVisualizer.Enabled = state.PlayerInput[PlayerIndex.One].InputState.Buttons.X;



			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}

		protected override void InitialisePlayerInput(Xen.Input.PlayerInputCollection playerInput)
		{
			//setup text
			//generate a string to indicate the buttons to hold to adjust the effects
			string button1, button2, button3;
			if (playerInput[PlayerIndex.One].ControlInput == Xen.Input.ControlInput.KeyboardMouse)
			{
				button1 = playerInput[PlayerIndex.One].KeyboardMouseControlMapping.A.ToString();
				button2 = playerInput[PlayerIndex.One].KeyboardMouseControlMapping.B.ToString();
				button3 = playerInput[PlayerIndex.One].KeyboardMouseControlMapping.X.ToString();

				playerInput[PlayerIndex.One].InputMapper.CentreMouseToWindow = true;
			}
			else
			{
				button1 = "A";
				button2 = "B";
				button3 = "X";
			}

			text.Text.Clear();
			text.Text.AppendFormatLine("Hold '{0}' to fire particle bursts", button1);
			text.Text.AppendFormatLine("Hold '{0}' to disable back to front draw sorting", button2);
			text.Text.AppendFormatLine("Hold '{0}' to show the Cull Tests performed", button3);
		}

		protected override void SetupGraphicsDeviceManager(GraphicsDeviceManager graphics, ref RenderTargetUsage presentation)
		{
			if (graphics != null) // graphics is null when starting within a WinForms host
			{
				graphics.PreferredBackBufferWidth = 1280;
				graphics.PreferredBackBufferHeight = 720;
			}
		}

	}

	//this is a hacky bit of geometry to draw a flickery light below the fire
	//it makes the scene look a bit nicer
	class GroundLightDisk : IDraw
	{
		private readonly IVertices vertices;
		private readonly IIndices indices;
		private Matrix worldMatrix;

		private float scale;
		private float scaleTarget;

		private static Random random = new Random();

		public GroundLightDisk(Vector3 position)
		{
			Matrix.CreateTranslation(ref position, out worldMatrix);

			const int vertCount = 32;
			//make a ground plane using two triangles
			//this is a bit nasty as the vertex buffer is recreated for each fire
			var verts = new VertexPositionColor[vertCount+2];
			var indices = new List<int>();

			//create vertices in a circle. The first vertex is in the centre. (Triangle Fan)
			verts[0] = new VertexPositionColor(Vector3.Zero, new Color(64,40,20));
			for (int i = 0; i <= vertCount; i++)
			{
				float angle = ((float)i / (float)vertCount) * MathHelper.TwoPi;
				verts[i + 1] = new VertexPositionColor(new Vector3((float)Math.Sin(angle) * 3, 0, (float)Math.Cos(angle) * 3), Color.Black);
				if (i > 0)
				{
					indices.Add(0);
					indices.Add(i);
					indices.Add(i + 1);
				}
			}
			this.indices = new Indices<int>(indices);
			this.vertices = new Vertices<VertexPositionColor>(verts);
		}

		public void Draw(DrawState state)
		{
			//scale the mesh
			Matrix scaleMatrix;
			Matrix.CreateScale(this.scale, out scaleMatrix);

			Xen.Ex.Shaders.FillVertexColour shader = state.GetShader<Xen.Ex.Shaders.FillVertexColour>();

			using (state.RenderState.Push())
			using (state.Shader.Push(shader))
			using (state.WorldMatrix.PushMultiply(ref worldMatrix))
			{
				state.WorldMatrix.Multiply(ref scaleMatrix);

				//setup blending
				state.RenderState.CurrentRasterState.CullMode = CullMode.None;
				state.RenderState.CurrentBlendState = AlphaBlendState.AdditiveSaturate;
				state.RenderState.CurrentDepthState.DepthWriteEnabled = false;

				//draw
				vertices.Draw(state, indices, PrimitiveType.TriangleList);
			}

			//this is a hack :-)
			//flicker the scale

			//every so often, target a new scale
			if (random.Next(100) > 75)
				scaleTarget = (float)random.NextDouble() * 0.4f + 0.6f;

			//interpolate to the scale target
			this.scale = this.scale * 0.75f + this.scaleTarget * 0.25f;
		}

		public bool CullTest(ICuller culler)
		{
			return culler.TestSphere(this.scale * 3, worldMatrix.Translation);
		}
	}
}
