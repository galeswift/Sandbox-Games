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
 * Drawing to an off-screen texture
 * Displaying the texture on a 2D Helper Element
 * 
 * 
 * see the 'NEW CODE' comments for code that has changed in this tutorial
 * 
 */
namespace Tutorials.Tutorial_07
{
	//The SphereDrawer class from Tutorial_03 is reused in this tutorial

	//a application that draws a sphere in the middle of an off screen texture, then draws the texture to the screen
	[DisplayName(Name = "Tutorial 07: Draw to a texture")]
	public class Tutorial : Application
	{
		//NEW CODE
		//A draw target that draws to an off screen texture
		//in this case it will draw a Sphere from tutorial 3
		private DrawTargetTexture2D drawToTexture;

		//Draw target that draws to the screen,
		//This will draw the off screen texture in the bottom left of the screen
		private DrawTargetScreen drawToScreen;


		//NEW CODE
		protected override void Initialise()
		{
			//draw targets usually need a camera.
			var camera = new Camera3D();
			//look at the sphere, which will be at 0,0,0
			camera.LookAt(Vector3.Zero, new Vector3(0, 0, 4), Vector3.UnitY);


			//create the draw target texture in the same way the DrawSphere sample created the draw target screen
			//creates a 128x128 texture (the pixelation should be visible)
			drawToTexture = new DrawTargetTexture2D(camera, 128, 128, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

			//make the texture clear colour different from the screen, so it's more obvious
			drawToTexture.ClearBuffer.ClearColour = Color.WhiteSmoke;

			//create the sphere (reused from Tutorial_03)
			var sphere = new Tutorial_03.SphereDrawer(Vector3.Zero);

			//Note, the sphere is added to the texture, not the screen
			drawToTexture.Add(sphere);


			//now, create the drawToScreen object..

			//The same camera is being used, although it doesn't have to be.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = Color.CornflowerBlue;


			var sizeInPixels = new Vector2(512,512);

			//Now create a 2D helper element that will display the texture on screen

			Xen.Ex.Graphics2D.TexturedElement displayTexture = null;

			//this helper class can directly take a DrawTargetTexture2D as a constructor parameter.

			//drawToTexture's Texture2D can be accessed with drawToTexture.GetTexture(),

			//However, at this point, drawToTexture.GetTexture() will be null - as the draw
			//target has yet to be drawn to.

			//drawToTexture.Warm() can be called, which will create the resources now.
			//However calling Warm() doesn't totally solve the problem because the texture 
			//will change when content needs reloading (this happens after a device reset, etc)

			//The 2D helper element takes care of this itself.
			//Content loading/reloading and dealing with GetTexture() will be covered in the next example.
			//for now, the helper class will handle things.

			//create the 2D helper element, which will display the texture on the screen.
			displayTexture = new TexturedElement(drawToTexture, sizeInPixels);

			//add it to the screen
			drawToScreen.Add(displayTexture);
		}

		//main application draw method
		protected override void Frame(FrameState state)
		{
			//used by Tutorial03 shader
			state.ShaderGlobals.SetShaderGlobal("colour", new Vector4(1, 0, 0, 1));

			//NEW CODE
			//Draw the off screen texture (make sure it's drawn at least once before it's used!)
			//In this example, the texture is drawn every frame
			drawToTexture.Draw(state);

			//draw to the screen.
			//(drawing to the screen should always occur last, and only happen once per frame)
			drawToScreen.Draw(state);
		}

		protected override void Update(UpdateState state)
		{
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}
	}
}
