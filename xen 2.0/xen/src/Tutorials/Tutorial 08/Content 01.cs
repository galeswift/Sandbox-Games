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
 * This sample extends from Tutorial_07 (Draw to texture)
 * This sample demonstrates:
 * 
 * How to load content (part 1)
 * 
 */
namespace Tutorials.Tutorial_08
{
	//The SphereDrawer class from Tutorial_03 is reused in this tutorial

	//This tutorial does exactly the same thing as Tutorial 07,
	//In tutorial 7, a sphere is drawn to a texture,
	//and this texture is displayed on the screen.
	//However, the previous tutorial used a helper class to display the texture.
	//This helper class handled content loading the texture.
	//
	//When a DrawTargetTexture object is created, the actual texture resource
	//is not created. This does not occur until either it's first use, or the Warm()
	//method is called.
	//
	//This is by design, and has many advantages in more complex projects.
	//It also means you have to treat DrawTargetTextures in a similar way to
	//actual content resources: for example, all resource references must be
	//updated during a device reset.
	//
	//In this tutorial, the use of the IContentOwner interface is shown indirectly.
	//The Application class already implements IContentOwner, and is registered as a
	//content owner directly after the call to Initialise().
	//
	//IContentOwner has one method, LoadContent. The LoadContent method is the only
	//place where access to an XNA ContentManager is given. This is to enforce
	//that loading content only happens in LoadContent!. 
	//(So content is always correctly reloaded)
	//
	//In Xen, all Content Owners are added to a 'ContentRegister' class.
	//The ContentRegister class keeps a weak-referenced list of IContentOwners.
	//It calls LoadContent when needed. 
	//
	//ContentRegister can be created at runtime. For convenience, the base Application
	//class has it's own instance of ContentRegister; Application.Content. 
	//
	//IContentOwners can be registered by calling Content.Add(), instances need not be
	//unregistered, due to weak referencing.
	//
	//Registering an IContentLoader with a ContentRegister guarantees that LoadContent
	//will be called at least once - where possible it gets called immediately.
	//
	//Forcing all content loading to happen in one way simplifies larger projects.
	//It prevents the case where content is loaded in unexpected ways.
	//
	//
	//Vertices<>, Indices<>, DrawTargets and some other xen classes automatically deal
	//with device resets (including buffers storing dynamic vertex data).
	//
	//The next tutorial will implement a IContentLoader instance.
	//For now, the Application's LoadContent method will be overridden
	[DisplayName(Name = "Tutorial 08: Content 1")]
	public class Tutorial : Application
	{
		//Draw targets...
		private DrawTargetTexture2D drawToTexture;
		private DrawTargetScreen drawToScreen;

		//NEW CODE
		//The textured element is stored
		private Xen.Ex.Graphics2D.TexturedElement displayElement;

		protected override void Initialise()
		{
			//draw target camera.
			var camera = new Camera3D();
			camera.LookAt(Vector3.Zero, new Vector3(0, 0, 4), Vector3.UnitY);


			//create the draw target texture
			//actual graphics resources are not created yet...
			drawToTexture = new DrawTargetTexture2D(camera, 128, 128, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

			//make the texture clear colour different from the screen, so it's more obvious
			drawToTexture.ClearBuffer.ClearColour = Color.WhiteSmoke;

			//add a sphere from tutorial 03 to the texture
			drawToTexture.Add(new Tutorial_03.SphereDrawer(Vector3.Zero));



			var sizeInPixels = new Vector2(512,512);

			//NEW CODE
			//create the helper element, but don't give it a texture yet..
			displayElement = new TexturedElement(sizeInPixels);



			//create the drawToScreen object..
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = Color.CornflowerBlue;

			//add the helper element to the screen
			drawToScreen.Add(displayElement);
		}

		//NEW CODE
		//override the LoadContent method (as declared in IContentOwner, which the Application class implements)
		//This is called directly after Initialise() and whenever the device has been reset
		protected override void LoadContent(ContentState state)
		{
			//set the display element's texture to that of the off screen drawtarget
			//At this point, the resource for the off-screen texture probably hasn't been created
			//
			//calling drawToTexture.GetTexture() will return null if the resource hasn't been created.
			//calling drawToTexture.Warm(IState) will force the resource to be created now.
			//
			//The shortcut, drawToTexture.GetTexture(IState), will call Warm() if required,
			//before returning the texture.
			//
			//Whenever the device is reset (which can happen fairly easily), any off screen textures
			//will become invalid.
			//As will many resource loaded through a ContentManager.
			
			displayElement.Texture = drawToTexture.GetTexture(state);

			//An easy way to test a device reset (in Windows Vista) is to bring up a UAC dialog
		}

		//main application draw method
		protected override void Frame(FrameState state)
		{
			//required by Tutorial03 shader
			state.ShaderGlobals.SetShaderGlobal("colour", new Vector4(1, 0, 0, 1));

			//Draw the off screen texture (the texture cannot be used until after the first time it's drawn!)
			//In this tutorial, the texture is drawn every frame
			drawToTexture.Draw(state);

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
