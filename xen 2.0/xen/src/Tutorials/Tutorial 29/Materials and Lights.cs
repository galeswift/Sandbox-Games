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

using Tutorials.Tutorial_14;
using System.Reflection;

/*
 * This sample demonstrates:
 * 
 * Using many lights on a single character.
 * 
 * This demonstrates how the MaterialShader will dynamically choose the closest lights to draw per-pixel,
 * While collecting the remaining lights as an ambient term.
 * 
 * Internally, the MaterialShader is single-pass. In shader model 2.0, the maximum number of lights that
 * can be drawn per-pixel is 2. This is due to limitations on how complex a shader can be.
 * Also, it makes sense from a performance point of view. More than two lights typically is going to be
 * too expensive.
 * 
 * As such, the MaterialShader will smartly choose which lights are going to have the most significant
 * impact on the object being drawn. This is based on proximity, size, colour intensity and a user defined
 * 'PriorityMultiplier' value (which is kept at 1.0 in this example, the default value)
 * 
 * This sample demonstrates where the capabilities of MaterialShader are best used - on a character. 
 * It is very hard to tell that the character in this demo is only using two per pixel lights.
 * However, the ground plane also uses the same system - and it's quite easy to see lights fade in and 
 * out as their priorities change.
 * 
 * 
 * Advanced use:
 * 
 * The lighting model used by MaterialShader can be infulenced by changing two properties:
 * 
 * ms.LightingDisplayModel can set how to display the model lighting, 
 * You can force per pixel or per vertex lighting, single light rendering, or ambient SH lighting only. (slowest to fastest)
 * 
 * Also, 
 * ms.LightingDisplayModelRadius allows setting the approximate radius of the model.
 * This value will be used in determining priority of nearby lights. All lights within the radius will be treated
 * as the same distance (so only intensity will factor into the priority calculation).
 * 
 * 
 * Apologies if this sample makes you dizzy :-)
 * 
 */
namespace Tutorials.Tutorial_29
{
	[DisplayName(Name = "Tutorial 29: MaterialShader Multi-light priority")]
	public class Tutorial : Application
	{
		private const float diskRadius = 50;

		private DrawTargetScreen drawToScreen;
		private Camera3D camera;
		private MaterialLightCollection lights;
		private Tutorials.Tutorial_11.Actor actor;
			
		protected override void Initialise()
		{
			camera = new Camera3D();

			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			//clear to dark blue
			drawToScreen.ClearBuffer.ClearColour = new Color(20, 20, 40);
			
			//create the light collection
			lights = new MaterialLightCollection();

			//set a dark blue ambient colour
			lights.AmbientLightColour = new Color(40, 40, 80).ToVector3();

			//get a list of predifined colours in the 'Color' structure using reflection
			//avoid doing this sort of thing at runtime!
			PropertyInfo[] colours = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public); // get all the static properties
			Random random = new Random();
			
			int lightCount = 12;

			//geometry for a light (shared for each light)
			IDraw lightGeometry = null;

			for (int i = 0; i < lightCount; i++)
			{
				//start with white.
				Color colour = Color.White;

				//try and pick a random colour from the list, using reflection to get the value of the property
				try
				{
					//pick a random field info object (a reflected colour property)
					var randomColourField = colours[random.Next(colours.Length)];

					//try and get it's value
					object colourObject = randomColourField.GetValue(null, null);

					if (colourObject is Color)
						colour = (Color)colourObject;
				}
				catch
				{
					//this shouldn't fail, but always be careful with reflection...
					//typically this would be handled correctly, but here, just stick with white.
				}


				float angle = (float)i / (float)lightCount * (float)Math.PI * 2;
				Vector3 position = new Vector3((float)Math.Sin(angle) * (diskRadius + 1), (float)Math.Cos(angle) * (diskRadius + 1), 4);

				float intensity = 1;

				//interface to the light about to be created
				IMaterialPointLight light = null;

				//create the point light
				light = lights.CreatePointLight(position, intensity, colour, colour);
				
				light.SourceRadius = 5;

				//create the light geometry (a sphere)
				if (lightGeometry == null)
					lightGeometry = new Xen.Ex.Geometry.Sphere(Vector3.One, 8, true, false, false);

				//visually show the light with a light drawer
				IDraw lightSourceDrawer = new LightSourceDrawer(position, lightGeometry, colour);

				//add the light geometry to the screen
				drawToScreen.Add(lightSourceDrawer);
			}

			//add the actor
			actor = new Tutorials.Tutorial_11.Actor(this.Content, this.lights);

			drawToScreen.Add(actor);


			//create the ground disk
			GroundDisk ground = new GroundDisk(this.Content, lights, diskRadius);

			//then add it to the screen
			drawToScreen.Add(ground);
		}

		protected override void Frame(FrameState state)
		{
			//rotate the camera around the ground plane
			RotateCamera(state);

			//draw everything
			drawToScreen.Draw(state);
		}

		//Override this method to setup the graphics device before the application starts.
		//This method is called before Initalise()
		protected override void SetupGraphicsDeviceManager(GraphicsDeviceManager graphics, ref RenderTargetUsage presentation)
		{
			if (graphics != null) // graphics is null when starting within a WinForms host
			{
				graphics.PreferredBackBufferWidth = 1280;
				graphics.PreferredBackBufferHeight = 720;
			}
		}


		private void RotateCamera(IState state)
		{
			//ignore most of this. it's just a bunch of magic numbers. Used to position the actor and camera
			float angle = state.TotalTimeSeconds * 0.15f - 0.1f;

			Vector3 lookAt = new Vector3((float)Math.Sin(angle) * diskRadius * 0.9f, (float)Math.Cos(angle) * diskRadius * 0.9f, 0);
			actor.WorldMatrix = Matrix.CreateRotationZ(-angle - MathHelper.PiOver2);
			actor.WorldMatrix.Translation = lookAt;

			lookAt.Z = 5;
			angle = state.TotalTimeSeconds * 0.15f;

			Vector3 lookFrom = new Vector3((float)Math.Sin(angle) * diskRadius * 0.8f, (float)Math.Cos(angle) * diskRadius * 0.8f, 6);
			lookFrom.Z += (float)Math.Sin(angle) * 3;

			camera.LookAt(lookAt, lookFrom, new Vector3(0, 0, 1));
		}

		protected override void Update(UpdateState state)
		{
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}

	}


	//this class simply draws the sphere representing the lights
	class LightSourceDrawer : IDraw
	{
		private IDraw geometry;
		private Vector3 position;
		private Color lightColour;

		public LightSourceDrawer(Vector3 position, IDraw geometry, Color lightColour)
		{
			this.position = position;
			this.geometry = geometry;
			this.lightColour = lightColour;
		}

		public void Draw(DrawState state)
		{
			using (state * position) // this multiply operator is a shortcut for 'state.WorldMatrix.PushTranslateMultiply(Vector3)'
			{
				DrawSphere(state);
			}
		}

		private void DrawSphere(DrawState state)
		{
			//draw the geometry with a solid colour shader
			if (geometry.CullTest(state))
			{
				var shader = state.GetShader<Xen.Ex.Shaders.FillSolidColour>();
				shader.FillColour = lightColour.ToVector4();
				
				using (state + shader) // this add operator is a shortcut for 'state.Shader.Push(IShader)'
				{
					geometry.Draw(state);
				}
			}
		}

		public bool CullTest(ICuller culler)
		{
			return true;
		}
	}
}
