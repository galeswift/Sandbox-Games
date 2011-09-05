using System;
using System.Text;
using System.Collections.Generic;



using Xen;
using Xen.Camera;
using Xen.Graphics;
using Xen.Ex.Graphics;
using Xen.Ex.Graphics2D;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


/*
 * This sample demonstrates displaying the Xen Logo
 * 
 * The Xen Logo is external, in the XenLogo.dll file. These .dlls must be included in the project.
 * It is an animated logo that can be displayed within an application, if desired.
 * 
 * Although this sample demonstrates it being drawn full screen, it can also be drawn to a DrawTarget,
 * and then placed somewhere (such as a credit roll :-)
 * 
 */
namespace Tutorials.XenLogo
{

	[DisplayName(Name = "Xen Logo")]
	public class Tutorial : Application
	{
		//screen draw target
		private DrawTargetScreen drawToScreen;

		//This is the logo displaying class
		private Xen.Logo.XenLogo xenLogo;

		protected override void Initialise()
		{
			//create the draw target.
			drawToScreen = new DrawTargetScreen(new Camera2D());

			//create the logo
			//it automatically adds itself into the update list and will start processing in the next frame
			this.xenLogo = new Xen.Logo.XenLogo(this.UpdateManager);
			
			//test xenLogo.EffectFinished to determine if the effect has completed.
			
			//add it to the screen
			this.drawToScreen.Add(xenLogo);
		}

		protected override void Frame(FrameState state)
		{
			//draw the screen / effect
			drawToScreen.Draw(state);
		}

		protected override void Update(UpdateState state)
		{
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
				this.Shutdown();
		}

		protected override void SetupGraphicsDeviceManager(GraphicsDeviceManager graphics, ref RenderTargetUsage presentation)
		{
			if (graphics != null)
			{
				graphics.PreferredBackBufferWidth = 1280;
				graphics.PreferredBackBufferHeight = 720;
			}
		}
	}
}
