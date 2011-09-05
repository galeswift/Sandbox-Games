using System;
using System.Collections.Generic;
using System.Text;



using Xen;
using Xen.Camera;
using Xen.Graphics;
using Xen.Ex.Graphics2D;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Xen.Ex.Material;
using Xen.Ex.Graphics;
using Xen.Ex.Graphics.Content;


/*
 * This sample builds on Tutorial 14
 * 
 * This sample demonstrates large scale animation of multiple actors
 * This sample demonstrates how to optimize draw order to avoid overdraw
 * 
 * NOTE: On the xbox, this sample runs significantly faster with a xen release build, without the debugger attached!
 * 
 */
namespace Tutorials.Tutorial_20
{

	//this class draws the Tiny model,
	//it also walks it randomly across the ground plane from Tutorial 14
	//When moving, a walk/run animation is played, when still, the 'Loiter'
	//animation is played.
	//
	//The movement logic is not very scientific! :-)
	//
	class Actor : IDraw, IContentOwner, IUpdate
	{
		//static random number generator
		private static Random random = new Random();

		private readonly float groundRadius;
		private readonly ModelInstance model;

		//animation controller, with two animations (move and idle)
		private AnimationController control;
		private AnimationInstance move, idle;

		//current position of the model, 'desired' target position, and velocity
		private Vector3 position, target, velocity;
		//the timer value that works out when to move to a new target
		//the current look angle (rotation of the model)
		//and the target angle, which is the angle of the next target
		private float moveTargetTimer, lookAngle, targetAngle;
		//move speed of the model
		private readonly float moveSpeed;

		//the world matrix of the model.
		private Matrix worldMatrix;
		private bool worldMatrixDirty = true; // set to true if worldMatrix is no longer valid, see 'UpdateWorldMatrix()'

		//create the actor
		public Actor(ContentRegister content, UpdateManager updateManager, MaterialLightCollection lights, float groundRadius)
		{
			this.groundRadius = groundRadius;

			model = new ModelInstance();
			model.LightCollection = lights;

			//force the model to render using spherical harmonic lighting
			//this will significantly improve performance on some hardware when GPU limited
			model.ShaderProvider = new Xen.Ex.Graphics.Provider.LightingDisplayShaderProvider(LightingDisplayModel.ForceSphericalHarmonic, model.ShaderProvider);

			//random starting position
			position = GetRandomPosition();

			//initially target the current position
			//the 'move to next target' timer will wind down and then the actor will move
			target = position;

			//randomize a bit
			lookAngle = (float)random.NextDouble() * MathHelper.TwoPi;
			moveSpeed = (float)random.NextDouble() * 0.9f + 0.1f;

			content.Add(this);
			updateManager.Add(this);


			InitaliseAnimations(updateManager);
		}

		private void InitaliseAnimations(UpdateManager updateManager)
		{
			//create the controller as an asynchronous controller.
			//this will process animations as a thread task
			//This occurs between the update loop and the draw loop,
			//which is why the UpdateManager must be provided.
			control = model.GetAsyncAnimationController(updateManager);

			//these perform a linear search to find the animation index
			int idleAnimationIndex	= control.AnimationIndex("Loiter");
			int jogAnimationIndex	= control.AnimationIndex("Jog");
			int walkAnimationIndex	= control.AnimationIndex("Walk");

			//create the idle animation
			idle = control.PlayLoopingAnimation(idleAnimationIndex);

			//give it a random speed
			idle.PlaybackSpeed = (float)random.NextDouble() * 0.5f + 0.6f;


			if (moveSpeed > 0.75)
			{
				//run animation
				move = control.PlayLoopingAnimation(jogAnimationIndex); // play a jogging animation
				move.PlaybackSpeed = 0.5f;
			}
			else
			{
				//walk animation
				move = control.PlayLoopingAnimation(walkAnimationIndex); // play a walking animation
			}
			//initially don't want the move animation being visible
			move.Weighting = 0;
		}


		private void UpdateWorldMatrix()
		{
			if (worldMatrixDirty)
			{
				//recalculate the world matrix


				//create the world matrix based on the current rotation and position...
				Matrix.CreateRotationZ(lookAngle - MathHelper.PiOver2, out this.worldMatrix);
				this.worldMatrix.Translation = position;

				//no longer dirty
				worldMatrixDirty = false;
			}
		}


		public void Draw(DrawState state)
		{
			//make sure the world matrix is up to date
			UpdateWorldMatrix();

			//draw the model
			using (state.WorldMatrix.PushMultiply(ref this.worldMatrix))
			{
				model.Draw(state);
			}
		}

		public bool CullTest(ICuller culler)
		{
			//make sure the world matrix is up to date first
			UpdateWorldMatrix();

			//the model implements ICullableInstance interface, which allows a matrix to be passed in
			//the DepthDrawSorter requires a cull test be performed to sort this actor into front-back rendering order

			return model.CullTest(culler, ref this.worldMatrix);
		}

		public void LoadContent(ContentState state)
		{
			//load the model
			model.ModelData = state.Load<ModelData>(@"tiny_4anim");
		}

		#region movement logic

		//this is the complex, and very hacky bit. :-)
		//this shouldn't be studied, it's just to look nice on screen
		public UpdateFrequency Update(UpdateState state)
		{
			//if the move timer is above 0, then the actor is stationary
			//slowly reduce the timer
			if (moveTargetTimer > 0)
				moveTargetTimer -= state.DeltaTimeSeconds;

			//vector between the position and target
			Vector3 vectorToTarget = (target - position);
			float distance = vectorToTarget.Length();

			//if the model is really close to the target, and the timer
			//isn't counting down..
			if (distance < 0.5f && moveTargetTimer == 0)
			{
				//then wait 5 seconds to find a new target
				moveTargetTimer = 5;
			}

			//timer has gone below zero, then it's time to find a new target
			if (moveTargetTimer < 0)
			{
				moveTargetTimer = 0;

				//new random target
				target = GetRandomPosition();

				//and a new angle to that target
				Vector3 difference = (target - position);
				if (difference != Vector3.Zero)
					targetAngle = (float)Math.Atan2(difference.Y, difference.X);
			}

			//over time, reduce movement speed with a magic number
			//move speed is stored in the animation weighting (a bit of a hack..)
			move.Weighting *= 0.975f;


			if (velocity != Vector3.Zero)
			{
				//and increment the position with another magic number
				position += velocity * 0.1f;

				//position has changed, so mark the world matrix as dirty (needs updating)
				worldMatrixDirty = true;
			}

			//the target is a good distance away...
			if (distance > 0.25f)
			{
				//and the actor isn't looking at the target
				if (targetAngle != this.lookAngle)
				{
					//slowly rotate to look at the new target
					//(this often rotates the wrong way.. :-)
					if (lookAngle > targetAngle)
						lookAngle -= 0.05f;
					if (lookAngle < targetAngle)
						lookAngle += 0.05f;

					//if really close to the right angle, then snap to it.
					if (Math.Abs(lookAngle - targetAngle) <= 0.05f)
						lookAngle = targetAngle;

					//look angle has changed, so world matrix will need updating
					worldMatrixDirty = true;
				}
				else
				{
					//increase the move speed.. with a lot more magic numbers!
					move.Weighting = moveSpeed * 0.1f + (move.Weighting * (1 - moveSpeed * 0.1f));
					velocity += vectorToTarget * 0.1f / distance * move.Weighting;
				}
			}
			//dampen the velocity too... with another magic number
			velocity *= 0.9f;

			//idle weighting and move weighting should add up to one
			idle.Weighting = 1 - move.Weighting;

			//don't both processing the animation if it's weighting is zero
			idle.Enabled = idle.Weighting != 0;
			move.Enabled = move.Weighting != 0;

			//finally, return the update frequency.
			//In this case, it's 60hz Async, which indicates
			//that this update method is thread safe and can be run in parallel
			//to other instances.

			//Note that Async has an overhead, so it may actually slow down for
			//very simple update methods.

			//perf boost here is fairly minor for using async,
			//as this method isn't all that heavy on the cpu.
			return UpdateFrequency.FullUpdate60hzAsync;
		}


		private Vector3 GetRandomPosition()
		{
			float rndAngle = (float)random.NextDouble() * MathHelper.TwoPi;
			//non linear random, keeps away from the centre
			float rndDist = (float)(1 - random.NextDouble() * random.NextDouble()) * groundRadius;

			Vector3 pos = new Vector3((float)Math.Sin(rndAngle) * rndDist, (float)Math.Cos(rndAngle) * rndDist, 0);
			return pos;
		}

		#endregion
	}

	[DisplayName(Name = "Tutorial 20: Animation stress test!")]
	public class Tutorial : Application
	{
		private const float diskRadius = 50;

		private DrawTargetScreen drawToScreen;
		private Camera3D camera;
		private MaterialLightCollection lights;

		private Xen.Ex.Graphics2D.Statistics.DrawStatisticsDisplay stats;

		protected override void Initialise()
		{
			camera = new Camera3D();
			camera.Projection.FarClip = 300;
			camera.Projection.NearClip = 10;
			camera.Projection.FieldOfView *= 0.55f;
			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);

			//no need to clear the colour buffer, as a special background will be drawn
			drawToScreen.ClearBuffer.ClearColourEnabled = false;

			//create the light collection first
			lights = new MaterialLightCollection();

			// In this example, the rendering order has been manually optimized to reduce the number of pixels drawn
			// 
			// In xen, rendering is usually explicit. This means, when a call to Draw() is made, the draw order is
			// respected, and internally the object will be drawn using the graphics API.
			// Objects added to the screen will have Draw() called in the order they were added.
			//
			// However, the draw order can also cause performance problems.
			// In general, it's best to draw front to back, this means draw the objects closest to the screen first.
			//
			// This way, the objects at the back will be drawing behind the objects already drawn.
			// Modern video cards can quickly discard pixels if they are 'behind' what is already drawn.
			// Without front-to-back, the objects at the front could be drawing *over* objects already drawn.
			//
			// This is known as overdraw, a case where an object is drawn, only to be 'overdrawn' later in the frame.
			// Reducing overdraw can help performance, especially when complex shaders are used.
			//
			// In this example, the sample is usually vertex-limited (that is, the bottleneck is vertex processing)
			// However, it can demonstrate how optimizing for overdraw can significantly reduce the number of pixels
			// that are shaded.
			//
			// In debug builds, the DrawStatisticsDisplay class will show the number of pixels drawn in the frame.
			// 
			// With overdraw optimized draw order, ~1,000,000 pixels are drawn per frame. Without, upto 2,100,000
			// pixels are drawn per frame (usually ~1,800,000). (A 1280x720 display has 921,600 pixels)
			// 
			// This means that without an overdraw optimized draw order, on average, each pixel is being drawn
			// twice. With an optimized draw order, this number is closer to 1.1, which is very close to the 
			// optimal value of 1.0 (where each pixel is only drawn once).
			//
			// Note that the number of pixels reported by the DrawStatisticsDisplay is for the entire frame, including
			// every render target. Some PCs may not support this value, and display -1.
			//
			// One last point....
			// This sample is an extreme test of a GPU's ability to push triangles onto the screen (vertex/triangle rate).
			// However, observation will show the number of triangles drawn is often over 3,300,000!
			// Assuming half the triangles are back-face culled (an accurate approximation), this still means
			// there are around 1,650,000 triangles that are visible at any time.
			// (But remember, the vertex shader still runs for back facing triangles!)
			//
			// Assuming approximatly half of these triangles are depth occluded (very approximate), still
			// results in a huge number of visible triangles.
			// This all means that the average triangle is drawing a *very* small number of pixels, in this case,
			// the average for the actors is probably *less than 1 pixel per triangle!*.
			//
			// Triangles averaging less than 1 pixel are known as subpixel triangles.
			// For a number of reasons, subpixel triangles are very inefficent.
			// For example, if a single pixel is drawn, due to the way a video card works, the pixel shader will always
			// run in multiples of 4 pixels, so a single pixel triangle will still run the pixel shader 4 times.
			//
			// As an approximate rule:
			// Typically drawing a 1 pixel triangle will be as no faster than drawing a 16 pixel triangle.
			//
			// This makes this sample a perfect candidate for level of detail optimization, where a lower resolution
			// model is used as an actor gets further away from the screen. (Eg, two modelInstances, sharing a controller)
			//
			// The vertex shader is also very expensive, and for each triangle, it will be run upto 3 times.
			// This means the vertex shader is running more often than the pixel shader!
			// This hypothesis can be confirmed; setting the lights to per-vertex, instead of per-pixel, results
			// in a significantly *lower* frame rate!
			//
			//
			//

			bool optimizeForOverdraw = true;

			//create a list of actors to added to the screen
			var actors = new List<Actor>(500);

			//create 500 actors!
			for (int i = 0; i < 500; i++)
			{
				Actor actor = new Actor(this.Content, this.UpdateManager, lights, diskRadius);
				actors.Add(actor);
			}


			//create the lights, similar to Tutorial 14
			lights.AmbientLightColour = new Vector3(0.35f, 0.35f, 0.45f);

			Vector3[] lightPositions =
			{ 
				new Vector3(0, 30, 12), 
				new Vector3(0, -30, 12) 
			};

			//setup the two lights in the scene
			IDraw lightGeometry = null;
			IDraw lightPoleGeometry = null;
			//create geometry to display the lights
			var lightSourceGeometry = new List<IDraw>();

			//setup the lights, and create the light globe geometry
			for (int i = 0; i < lightPositions.Length; i++)
			{
				var colour = new Vector3(2, 2, 2);

				var light = lights.CreatePointLight(lightPositions[i], 1, colour, colour);

				light.SourceRadius = 6;

				if (lightGeometry == null)
				{
					lightGeometry = new Xen.Ex.Geometry.Sphere(Vector3.One, 8, true, false, false);
					lightPoleGeometry = new Xen.Ex.Geometry.Cube(new Vector3(0.4f, 0.4f, lightPositions[i].Z * 0.5f));
				}

				//visually show the light
				//create the light sphere geometry from tutorial 14.
				var position = lightPositions[i];
				lightSourceGeometry.Add(new Tutorial_14.LightSourceDrawer(position, lightGeometry, Color.LightYellow));
				position.Z *= 0.5f;
				lightSourceGeometry.Add(new Tutorial_14.LightSourceDrawer(position, lightPoleGeometry, new Color(40,40,70)));
			}

			//create the ground plane, also from tutorial 14
			var ground = new Tutorial_14.GroundDisk(this.Content, lights, diskRadius);


			//this is a special background element,
			//it draws a gradient over the entire screen, fading from dark at the bottom to light at the top.
			Color darkBlue = new Color(40, 40, 50);
			Color lightBlue = new Color(100, 100, 110);
			var background = new BackgroundGradient(lightBlue, darkBlue);


			if (optimizeForOverdraw == false)
			{
				//add all the objects in a naive order

				//first add the background (fills the entire screen, draws to every pixel, but is very fast)
				drawToScreen.Add(background);

				//then add the ground plane (all the actors will appear on top of the ground plane, overdrawing it)
				drawToScreen.Add(ground);

				//then add the lights (which are on top of the ground, overdrawing it)
				foreach (IDraw geometry in lightSourceGeometry)
					drawToScreen.Add(geometry);

				//then finally add the actors, in the order they were created
				foreach (Actor actor in actors)
					drawToScreen.Add(actor);
			}
			else
			{
				//or, add the objects in a order optimized for overdraw

#if !XBOX360
				//first, add the actors. Because they are almost always closest to the screen
				//however, use a depth sorter so the actors are sorted into a front to back draw order,
				//this sorting is based on the centre point of the cull tests they perform.

				var sorter = new Xen.Ex.Scene.DepthDrawSorter(Xen.Ex.Scene.DepthSortMode.FrontToBack);

				//Remember, the objects placed in the sorter *must* perform a valid CullTest,
				//if the CullTest simply returns true/false, no sorting will occur.
				//(Note the Actor.CullTest method)

				//to ease the CPU load, have the sorter only sort the actors every few frames...
				sorter.SortDelayFrameCount = 5;

				foreach (Actor actor in actors)
					sorter.Add(actor); // add the actors to the sorter (not the screen)

				//the sorter itself must be added to the screen!
				drawToScreen.Add(sorter); // the sorter will then draw the actors in a sorted order
#else

				//In this case (on the Xbox), because the application is heavily vertex limited
				//and already heavily CPU stretched by the animation system, the cost of 
				//sorting the actors actually causes a larger performance hit on the CPU than 
				//the time saved on the GPU. This inballance causes a frame rate drop.
				//
				//However, the reason for this may be unexpected. 
				//The framerate drop is not caused by the overhead of sorting the actors.
				//
				//Any 3D API calls made are doubly expensive on the XBOX, so in order to 
				//maintain 20fps in this sample, the primary (rendering) thread must not 
				//block, or switch to task processing.
				//If it does so, valuable rendering time is lost.
				//
				//When using a sorter, the actors are drawn in an order that is constantly changing.
				//However, they always have Update() called in a consistent order.
				//
				//During Update() the actors animation controllers will spawn thread tasks to 
				//process their animation.
				//
				//These tasks are processed on the spare xbox hardware threads, they are
				//processed in the order they were added. 
				//Processing the animation usually completes before the rendering finishes.
				//(the rendering is not delayed waiting for the animation to finish).
				//
				//However, when sorting the actors get drawn in an unpredictable order, 
				//this means the last actor added could be the first actor to draw,
				//in such a case, the chances of it's animation processing having completed
				//is *very* low. When this happens, the rendering thread has to switch to
				//processing animations, delaying rendering.
				//
				//So, for the xbox, in this sample it's best just to draw in the update order.

				foreach (Actor actor in actors)
					drawToScreen.Add(actor);

#endif

				//add the light source geometry, as they are usually below the actors, but above the ground
				foreach (IDraw geometry in lightSourceGeometry)
					drawToScreen.Add(geometry);

				//then add the ground plane, which is usually below the actors and lights.
				drawToScreen.Add(ground);

				//finally, enable a special feature of ElementRect.
				//This makes the element draw at the maximum possible Z distance
				//(behind anything else that has been drawn)
				background.DrawAtMaxZDepth = true;
				
				//add it to the screen
				drawToScreen.Add(background);
			}

			//finally,
			//create the draw statistics display
			stats = new Xen.Ex.Graphics2D.Statistics.DrawStatisticsDisplay(this.UpdateManager);
			drawToScreen.Add(stats);
		}

		protected override void Frame(FrameState state)
		{
			//rotate the camera around the scene (similar to tutorial 14)
			RotateCamera(state);

			//draw the scene
			drawToScreen.Draw(state);
		}

		private void RotateCamera(IState state)
		{
			Vector3 lookAt = new Vector3(0, 0, 0);
			float angle = state.TotalTimeSeconds * 0.05f;
			Vector3 lookFrom = new Vector3((float)Math.Sin(angle) * 60, (float)Math.Cos(angle) * 80, 25);
			lookFrom.Z += (float)Math.Sin(angle) * 15;

			camera.LookAt(lookAt, lookFrom, new Vector3(0, 0, 1));
		}

		protected override void Update(UpdateState state)
		{
			if (state.PlayerInput[0].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}

		//setup the desired resolution
		protected override void SetupGraphicsDeviceManager(GraphicsDeviceManager graphics, ref RenderTargetUsage presentation)
		{
			if (graphics != null) // graphics is null when starting within a WinForms host
			{
				graphics.PreferredBackBufferWidth = 1280;
				graphics.PreferredBackBufferHeight = 720;
				graphics.SynchronizeWithVerticalRetrace = false;
			}
		}

		//set the font for the statistics overlay
		protected override void LoadContent(ContentState state)
		{
			stats.Font = state.Load<SpriteFont>(@"Arial");
		}

	}


	//a simple element that displays a gradient background
	public class BackgroundGradient : SolidColourElement
	{
		private Color topColour;

		public BackgroundGradient(Color top, Color bottom) : base(bottom,new Vector2(1,1),true)
		{
			this.topColour = top;
		}

		protected override void WriteColours(ref Color topLeft, ref Color topRight, ref Color bottomLeft, ref Color bottomRight)
		{
			topLeft = topColour;
			topRight = topColour;
			bottomLeft = this.Colour;
			bottomRight = this.Colour;
		}
	}
}
