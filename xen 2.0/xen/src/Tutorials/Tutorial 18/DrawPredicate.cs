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


/*
 * This sample extends from Tutorial_02 (Draw a sphere)
 * This sample demonstrates:
 * 
 * Using a DrawPredicate
 * 
 */
namespace Tutorials.Tutorial_18
{
	//In this example, a large cube is drawn, covering most of the screen
	//behind it, a complex sphere is drawn. A second cube is also used, which represents a bounding box for the sphere.
	//This sphere and the bounding box is drawn using a DrawPredicate. 
	//The bounding box is the 'predicate', which is the low detail bounding box of the 'complex' sphere.
	//The DrawPredicate object uses an occlusion query to test how many pixels the bounding box draws.
	//If the bounding box is visible (pixels are drawn) then the complex sphere is also drawn.

	//Note the DrawPredicate handles drawing of the complex sphere and the bounding box.
	//When drawing the bounding box, the DrawPredicate will diable depth and colour writes (so the box will not be visible)

	//To make the culling process visible, an overlay of the wireframe of the sphere will also be shown

	//this class draws the geometry, at a given position.
	//it also renders the geometry in wireframe, with depth testing disabled (to show the sphere through the occluding cube)
	//the wireframe uses it's own shader
	class GeometryDrawer : IDraw
	{
		private IDraw geometry;
		private readonly IShader shader, wireframeShader;

		public Vector3 position;


		public GeometryDrawer(IDraw geometry, IShader shader, IShader wireframeShader)
		{
			this.geometry = geometry;
			this.shader = shader;
			this.wireframeShader = wireframeShader;
		}

		public void Draw(DrawState state)
		{
			using (state.WorldMatrix.PushTranslateMultiply(ref this.position))
			{
				if (geometry.CullTest(state))
				{
					//draw the geometry

					using (state + shader)
					{
						geometry.Draw(state);

						//now, if set, draw the wireframe too
						if (wireframeShader != null)
						{
							state.Shader.Set(wireframeShader);

							//show the wireframe, disabling depth testing
							state.RenderState.Push();
							state.RenderState.CurrentDepthState.DepthTestEnabled = false;
							//also set additive blending
							state.RenderState.CurrentBlendState = AlphaBlendState.Additive;
							//set wireframe
							state.RenderState.CurrentRasterState.FillMode = FillMode.WireFrame;

							//draw
							geometry.Draw(state);

							state.RenderState.Pop();
						}
					}
				}
			}
		}

		public bool CullTest(ICuller culler)
		{
			return true;
		}
	}



	//This class is an application that draws the sphere in the middle of the screen, moving left to right,
	//A cube is drawn in front of the sphere, potentially occluding it
	[DisplayName(Name = "Tutorial 18: DrawPredicate")]
	public class Tutorial : Application
	{
		DrawTargetScreen drawToScreen;

		//the complex sphere
		GeometryDrawer sphere;
		//it's bounding box
		GeometryDrawer sphereBoundingBox;

		//the occluding cube
		GeometryDrawer cube;

		//runtime stats (see tutorial 15)
		Xen.Ex.Graphics2D.Statistics.DrawStatisticsDisplay statOverlay;


		protected override void Initialise()
		{
			Camera3D camera = new Camera3D();
			camera.LookAt(Vector3.Zero, new Vector3(0, 0, 5), Vector3.UnitY);

			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = Color.CornflowerBlue;



			//create a shader to display the geometry (this is the same as tutorial 02)
			var lightDirection = new Vector3(1.0f, 0.5f, 0.5f);
			var material = new MaterialShader();
			material.SpecularColour = Color.LightYellow.ToVector3();				//give the material a nice sheen

			var lights = new MaterialLightCollection();

			lights.AmbientLightColour = Color.CornflowerBlue.ToVector3() * 0.5f;	//set the ambient
			lights.CreateDirectionalLight(lightDirection, Color.Gray);				//add the first of two light sources
			lights.CreateDirectionalLight(-lightDirection, Color.DarkSlateBlue);

			material.LightCollection = lights;

			//create a simpler shader to display the wireframe (and also used for the bounding cube)
			var simpleShader = new Xen.Ex.Shaders.FillSolidColour();
			simpleShader.FillColour = Vector4.One * 0.01f;


			var sphereSize = new Vector3(0.5f, 0.5f, 0.5f);

			//create the complex sphere, this will have ~100k triangles.
			//pass in a shader for wireframe rendering
			sphere = new GeometryDrawer(new Xen.Ex.Geometry.Sphere(sphereSize, 200), material, simpleShader);

			//create the bounding cube
			sphereBoundingBox = new GeometryDrawer(new Xen.Ex.Geometry.Cube(sphereSize), simpleShader, null);

			//create the occluding cube, and position it close to the camera
			cube = new GeometryDrawer(new Xen.Ex.Geometry.Cube(Vector3.One), material, null);
			cube.position = new Vector3(0, 0, 2.75f);


			//add the cube first (so it can draw first, potentially occluding the sphere)
			//if the cube was added second, it would have no effect, as it would draw after the sphere
			drawToScreen.Add(cube);


			//create the predicate, passing in the sphere and bounding box
			var predicate = new Xen.Ex.Scene.DrawPredicate(sphere, sphereBoundingBox);

			//add the DrawPredicate (the DrawPredicate draws it's children)
			drawToScreen.Add(predicate);


			//statistic overlay
			statOverlay = new Xen.Ex.Graphics2D.Statistics.DrawStatisticsDisplay(this.UpdateManager);
			drawToScreen.Add(statOverlay);
		}

		//main application draw method
		protected override void Frame(FrameState state)
		{
			//move the sphere from left to right
			sphere.position = new Vector3((float)Math.Sin(state.TotalTimeSeconds) * 6, 0, 0);
			//and make sure the bounding box matches it
			sphereBoundingBox.position = sphere.position;


			//draw to the screen.
			drawToScreen.Draw(state);
		}



		protected override void LoadContent(ContentState state)
		{
			statOverlay.Font = state.Load<SpriteFont>("Arial");
		}

		protected override void Update(UpdateState state)
		{
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}
	}
}
