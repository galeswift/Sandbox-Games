using System;
using System.Collections.Generic;
using System.Text;



using Xen;
using Xen.Camera;
using Xen.Graphics;
using Xen.Ex.Geometry;
using Xen.Ex.Graphics2D;
using Xen.Ex.Material;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


/*
 * This sample extends from Tutorial_02 (Draw Sphere)
 * This sample demonstrates:
 * 
 * Changing the render state
 * Using shortcut operators on the DrawState
 * 
 */
namespace Tutorials.Tutorial_06
{
	//this class is identical to the SphereDrawer in Tutorial_02,
	//except the draw method now implements different ways to change the alpha blending render state
	class SphereDrawer : IDraw
	{
		private int renderStateMode = 0;


		//draw the sphere with alpha blending
		public void Draw(DrawState state)
		{
			//below are 4 variations on how to set the alpha blending render state
			switch (renderStateMode % 4)
			{
				case 0:
					DrawManual(state);
					break;
				case 1:
					DrawPushPopManual(state);
					break;
				case 2:
					DrawPushPopStatic(state);
					break;
				case 3:
					DrawPushPopStored(state);
					break;
			}

			//cycle the various methods
			renderStateMode++;
		}

		//each of these 4 methods is more efficient than the previous...

		//this method is the manual method where each state is setup peice by peice
		private void DrawManual(DrawState state)
		{
			//manual render state

			//First, take a copy of current render state..
			DeviceRenderState currentState = state.RenderState;
			
			//The DeviceRenderState structure stores common render state.
			//The majority of render state is stored, with most commonly used xbox-supported state included.
			//The entire structure is less than 16 bytes (the size of a Vector4)
			//
			//The DeviceRenderState structure stores four smaller structures:
			//
			//	StencilState			Stencil;		(8 bytes)
			//	AlphaBlendState			Blend;			(4 bytes)	
			//	RasterState				Raster;			(2 bytes)
			//	DepthState				Depth;		    (1 byte)
			//
			
			//Here the alpha blend state is changed manually...

			//reset the DrawState's alpha blending render state to default (no blending)
			state.RenderState.CurrentBlendState = new AlphaBlendState();

			//set blending states one by one...
			state.RenderState.CurrentBlendState.Enabled = true;
			state.RenderState.CurrentBlendState.SourceBlend = Blend.SourceAlpha;
			state.RenderState.CurrentBlendState.DestinationBlend = Blend.InverseSourceAlpha;


			//draw the sphere
			DrawGeometry(state);


			//reset the previous state back
			state.RenderState.Set(ref currentState);
		}


		private void DrawPushPopManual(DrawState state)
		{
			//manual render state, using Push/Pop render state

			//push the render state
			//pusing/popping the render state is very fast
			using (state.RenderState.Push())
			{


				//change the alpha blend state (and only the alpha blend state) manually...

				state.RenderState.CurrentBlendState = new AlphaBlendState();
				//set blending...
				state.RenderState.CurrentBlendState.Enabled = true;
				state.RenderState.CurrentBlendState.SourceBlend = Blend.SourceAlpha;
				state.RenderState.CurrentBlendState.DestinationBlend = Blend.InverseSourceAlpha;


				//draw the sphere
				DrawGeometry(state);


			}
			//at this point, the using statement will have reset the render state to what it was when 'Push' was called.
		}



		private void DrawPushPopStatic(DrawState state)
		{
			//use a static alpha blend render state, using Push/Pop render state

			//push the entire render state
			using (state.RenderState.Push())
			{

				//change the alpha blend state (and only the alpha blend state) using a static blend state...
				//AlphaBlendState.Alpha is a static already setup with alpha blending
				state.RenderState.CurrentBlendState = AlphaBlendState.Alpha;

				//draw the sphere
				DrawGeometry(state);

			}
		}


		private void DrawPushPopStored(DrawState state)
		{
			//This method demonstrates using the '+' operator shortcut,
			//internally, this is equivalent to calling:
			//state.RenderState.Push(AlphaBlendState.Alpha)
			//All Push() methods in all state stack classes include these shortcuts
			
			using (state + AlphaBlendState.Alpha)
			{

				//draw the sphere
				DrawGeometry(state);

			}
		}



		// most of the rest of the code in this file is identical to tutorial 02
		// (expect the lighting shader outputs an alpha value that changes, and a background rect is drawn for contrast)


		//geometry of the sphere
		private Xen.Ex.Geometry.Sphere sphereGeometry;
		//world matrix (position and rotation) of the sphere
		private Matrix worldMatrix;
		//shader used to display the sphere
		private MaterialShader shader;

		//constructor
		public SphereDrawer(Vector3 position)
		{
			//setup the sphere
			var size = new Vector3(1,1,1);
			//use a prebuilt sphere geometry class
			sphereGeometry = new Sphere(size, 32);

			//setup the world matrix
			worldMatrix = Matrix.CreateTranslation(position);

			//create a lighting shader with some nice looking lighting
			var material = new MaterialShader();
			material.SpecularColour = Color.LightYellow.ToVector3();//with a nice sheen

			var lightDirection = new Vector3(0.5f,1,-0.5f); //a dramatic direction

			var lights = new MaterialLightCollection();
			lights.AmbientLightColour = Color.CornflowerBlue.ToVector3() * 0.5f;
			lights.CreateDirectionalLight(lightDirection, Color.Gray);//two light sources
			lights.CreateDirectionalLight(-lightDirection, Color.DarkSlateBlue);

			material.LightCollection = lights;

			this.shader = material;
		}

		//draw the sphere
		private void DrawGeometry(DrawState state)
		{
			//push the world matrix, multiplying by the current matrix if there is one
			//Note, this is using the '*' operator shortcut to call:
			//state.WorldMatrix.PushMultiply(this.worldMatrix)

			using (state * this.worldMatrix)
			{
				//cull test the sphere
				if (sphereGeometry.CullTest(state))
				{
					//animate the material alpha.. (in a sin wave between 0 and 1)
					shader.Alpha = (float)Math.Sin(state.TotalTimeSeconds * 2) * 0.5f + 0.5f;
					
					//bind the shader
					using (state + shader)
					{
						//draw the sphere geometry
						sphereGeometry.Draw(state);
					}
				}
			}
		}

		public bool CullTest(ICuller culler)
		{
			//is the sphere on screen?
			return sphereGeometry.CullTest(culler, ref this.worldMatrix);
		}
	}


	//a application that draws a sphere in the middle of the screen
	[DisplayName(Name = "Tutorial 06: Render State")]
	public class Tutorial : Application
	{
		//a DrawTargetScreen is a draw target that draws items directly to the screen.
		//in this case it will only draw a SphereDrawer
		private DrawTargetScreen drawToScreen;

		protected override void Initialise()
		{
			//draw targets usually need a camera.
			Camera3D camera = new Camera3D();
			//look at the sphere, which will be at 0,0,0
			camera.LookAt(Vector3.Zero, new Vector3(0, 0, 4), Vector3.UnitY);

			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = Color.CornflowerBlue;

			//create the sphere
			SphereDrawer sphere = new SphereDrawer(Vector3.Zero);


			//before adding the sphere, add a rect over half the background to show blending is active

			//element covers half the screen
			SolidColourElement element = new SolidColourElement(Color.DarkGray, new Vector2(0.5f, 1), true);
			//element is added before the sphere (so it draws first)
			drawToScreen.Add(element);

			//add it to be drawn to the screen
			drawToScreen.Add(sphere);
		}

		//main application draw method
		protected override void Frame(FrameState state)
		{
			//draw to the screen.
			drawToScreen.Draw(state);
		}

		protected override void Update(UpdateState state)
		{
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}
	}
}
