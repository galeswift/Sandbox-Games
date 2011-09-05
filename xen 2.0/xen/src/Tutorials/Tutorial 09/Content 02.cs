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


/*
 * This sample extends from Tutorial_01 (Application class)
 * This sample demonstrates:
 * 
 * How to load content using a class that implements IContentOwner (part 2)
 * It also demonstrates sampling a texture in a shader.
 * 
 */
namespace Tutorials.Tutorial_09
{
	//This class simply draws an image loaded through the XNA content pipeline.
	//The image is drawn to the centre of the screen.
	//A custom shader is used to display this texture (see 'shader.fx')

	//This class implements the IContentOwner interface to load the image
	class ImageDisplayer : IDraw, IContentOwner
	{
		//Helper element that will display the image on screen, using the custom shader
		private ShaderElement element;
		//Store an instance of the custom shader
		private Shader.Tutorial09Technique shader;
		
		//construct the displayer.
		//Because this class needs to load content, a ContentRegister is a required
		//constructor parameter. This makes sure it will always load content
		public ImageDisplayer(IContentRegister contentRegister)
		{
			if (contentRegister == null)
				throw new ArgumentNullException();

			//create the element that will display the texture
			var sizeInPixels = new Vector2(768,384);

			//create an instance of the shader
			//The texture will be assigned to the shader in LoadContent
			this.shader = new Shader.Tutorial09Technique();

			//create the element which will display the shader
			this.element = new ShaderElement(this.shader, sizeInPixels);

			//place the element in the centre of the screen
			this.element.HorizontalAlignment = HorizontalAlignment.Centre;
			this.element.VerticalAlignment = VerticalAlignment.Centre;

			//add this object to the content register
			//items added to a content register do not need to be removed, as they
			//are tracked by a weak reference.
			//LoadContent() will be called whenever the device is created/reset.
			contentRegister.Add(this);

			//At this point, since the device is being created, LoadContent() will have
			//been called already. If the device hadn't been created yet, then
			//LoadContent() would be called at a later time.

			//LoadContent() will always be called before the first time Draw() is called.
		}

		//Load content
		public void LoadContent(ContentState state)
		{
			//load and assign the texture:
			//Note the texture is assigned to the shader, not the visual element
			var texture = state.Load<Texture2D>(@"skyline");

			this.shader.DisplayTexture = texture;

			//The 2D element can adjust it's texture coordinates to make sure the texture
			//is exactly aligned to on screen pixels, however, to do this, the size of the
			//texture must be set (this is assuming the shader displays a texture!)
			this.element.SetTextureSize(texture.Width, texture.Height);
		}

		//draw the element on screen
		public void Draw(DrawState state)
		{
			//the ShaderElement class automatically calls the shader Bind() method
			element.Draw(state);
		}

		//always return true, so Draw() is always called (the element is always on screen)
		public bool CullTest(ICuller culler)
		{
			return true;
		}
	}




	//This application class simply creates the image displayer and adds it to the screen
	//The Application class stores its own ContentRegister (this.Content)
	//(Extra content registers can be created as well)
	[DisplayName(Name = "Tutorial 09: Content 02 and Shader Texture Sampling")]
	public class Tutorial : Application
	{
		//screen draw target
		private DrawTargetScreen drawToScreen;

		protected override void Initialise()
		{
			//create the draw target.
			drawToScreen = new DrawTargetScreen(new Camera3D());
			
			//create the image displayer, passing in a reference
			//to the ContentRegister for this Application instance
			var imageDisplayer = new ImageDisplayer(this.Content);

			//add it to the screen
			drawToScreen.Add(imageDisplayer);
		}

		protected override void Frame(FrameState state)
		{
			drawToScreen.Draw(state);
		}

		protected override void Update(UpdateState state)
		{
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}
	}
}
