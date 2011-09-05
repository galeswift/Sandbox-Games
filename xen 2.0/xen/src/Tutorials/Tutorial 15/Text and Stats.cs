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
 * This sample demonstrates:
 * 
 * Displaying text
 * 
 * Displaying the debug DrawStatisticsDisplay overlay
 * 
 * Using the Xen.Ex first person free camera
 * 
 */
namespace Tutorials.Tutorial_15
{

	//this application will create a text rectangle,
	//and it will also create a DrawStatisticsDisplay overlay element

	[DisplayName(Name = "Tutorial 15: Text, DrawStatistics and Free Camera")]
	public class Tutorial : Application
	{
		private Camera3D camera;
		private DrawTargetScreen drawToScreen;

		//This is text element will display some custom text in a rectangle
		private TextElementRect yellowElement;
		//this element will display the position of the camera
		private TextElement positionDisplay;
		//a red box that is embedded within the yellow text
		private SolidColourElement embeddedElement;

		//this is a special object that displays a large number of debug graphs
		//this is very useful for debugging performance problems at runtime
		private Xen.Ex.Graphics2D.Statistics.DrawStatisticsDisplay statisticsOverlay;


		protected override void Initialise()
		{
			//Xen.Ex provides a very useful Camera3D called 'FirstPersonControlledCamera3D'.
			//This camera uses player input to act as a simple first-person style flythrough camera
			Xen.Camera.FirstPersonControlledCamera3D camera = null;

			//it uses player input, so the UpdateManager must be passed in
			camera = new Xen.Camera.FirstPersonControlledCamera3D(this.UpdateManager);

			//in this case, we want the z-axis to be the up/down axis (otherwise it's the Y-axis)
			camera.ZAxisUp = true;
			//also it's default is a bit too fast moving
			camera.MovementSensitivity *= 0.1f;
			camera.LookAt(new Vector3(1, 0, 0), new Vector3(), new Vector3(0, 0, 1));

			this.camera = camera;

			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);


			//create a large number of actor instance from tutorial 10..
			for (int n = 0; n <= 16; n++)
			{
				//create in a half circle
				float angle = (n / 16.0f) * MathHelper.Pi;
				var position = new Vector3((float)Math.Sin(angle), (float)Math.Cos(angle), 0);

				//not too close together
				position *= 10;

				drawToScreen.Add(new Tutorial_10.Actor(this.Content, position));
			}


			//this element will display the camera position
			positionDisplay = new TextElement();

			//TextElement (unlike other Elements) defaults to Top Left alignment
			//So, in order to bring it closer to the centre of the screen (due to potential overscan)
			//it's position needs to be set 'right' and 'down' from 'top left'
			//(this is just an example, see XNA docs for correct overscan compensation behaviour)
			positionDisplay.Position = new Vector2(40, -40); //offset from top left corner alignment

			//add it to the screen
			drawToScreen.Add(positionDisplay);



			var sizeInPixels = new Vector2(400, 200);

			//create the main block of yellow text
			this.yellowElement = new TextElementRect(sizeInPixels);
			this.yellowElement.Colour = Color.Yellow;

			//first line of text... this will have a flashing 2D element embedded
			string embeddedText = @"This is a text box with a large amount of custom text! It also includes an embedded 2D element: , which is a 16x16 SolidColourElement";
			uint insertAtIndex = 96; // Hard coded to insert a 2D element at character index 96               which is about here: ^

			//add a bunch of text...
			this.yellowElement.Text.AppendLine(embeddedText);
			this.yellowElement.Text.AppendLine();
			this.yellowElement.Text.AppendLine(@"This class is:");
			this.yellowElement.Text.AppendLine(this.GetType().FullName);
			this.yellowElement.Text.AppendLine(@"It is located in assembly:");
			this.yellowElement.Text.AppendLine(this.GetType().Assembly.FullName);
			this.yellowElement.Text.AppendLine();

			//add an embedded 2D element within the text
			//create it..
			this.embeddedElement = new SolidColourElement(Color.Red, new Vector2(16, 16)); // quite small
			this.embeddedElement.AlphaBlendState = AlphaBlendState.Alpha;
			//add it.
			this.yellowElement.AddInline(this.embeddedElement, insertAtIndex);


#if XBOX360
			this.yellowElement.Text.AppendLine(@"Press and hold both thumbsticks to show the debug overlay");
#else
			this.yellowElement.Text.AppendLine(@"Press F12 to show the debug overlay");
#endif


			//align the element rectangle to the bottom centre of the screen
			this.yellowElement.VerticalAlignment = VerticalAlignment.Bottom;
			this.yellowElement.HorizontalAlignment = HorizontalAlignment.Centre;

			//centre align the text
			this.yellowElement.TextHorizontalAlignment = TextHorizontalAlignment.Centre;
			//centre the text in the middle of the 400x200 area of the element rectangle
			this.yellowElement.TextVerticalAlignment = VerticalAlignment.Centre;

			//add it to the screen
			drawToScreen.Add(yellowElement);




			//create the statistics display
			//this class will query the DrawState for the previous frames DrawStatistics structure.
			//this structure provides a large number of statistics for the drawn frame.
			//The DrawStatisticsDisplay displays some of the more important statistics. It will also
			//display thread activity on the xbox.

			//DrawStatistics are only available in DEBUG xen builds
			//They can be accessed at runtime with DrawState GetPreviousFrameStatistics()

			//at runtime, pressing 'F12' will toggle the overlay (or holding both thumbsticks on x360)
			this.statisticsOverlay = new Xen.Ex.Graphics2D.Statistics.DrawStatisticsDisplay(this.UpdateManager);

			//then add it to the screen
			drawToScreen.Add(statisticsOverlay);
		}

		protected override void InitialisePlayerInput(Xen.Input.PlayerInputCollection playerInput)
		{
			this.yellowElement.Text.AppendLine();

			//if using keyboard/mouse, then centre the mouse each frame
			if (playerInput[PlayerIndex.One].ControlInput == Xen.Input.ControlInput.KeyboardMouse)
			{
				playerInput[PlayerIndex.One].InputMapper.CentreMouseToWindow = true;
				this.yellowElement.Text.AppendLine("Use the mouse and WASD to move the camera");
			}
			else
				this.yellowElement.Text.AppendLine("Use the gamepad to move the camera");

		}

		//load the font used by the text and overlay
		protected override void LoadContent(ContentState state)
		{
			//Load a normal XNA sprite font
			var xnaSpriteFont = state.Load<SpriteFont>("Arial");

			//both elements require the font to be set before they are drawn
			this.yellowElement.Font = xnaSpriteFont;
			this.positionDisplay.Font = xnaSpriteFont;
			//the statistics overlay also requires the font is set
			this.statisticsOverlay.Font = xnaSpriteFont;
		}


		protected override void Frame(FrameState state)
		{
			//update some of the text before drawing..

			//get the camera position
			Vector3 cameraPosition;
			camera.GetCameraPosition(out cameraPosition);

			//Set the position text to the camera position
			positionDisplay.Text.Clear();

			positionDisplay.Text.Append(cameraPosition.X);
			positionDisplay.Text.Append(", ");
			positionDisplay.Text.Append(cameraPosition.Y);
			positionDisplay.Text.Append(", ");
			positionDisplay.Text.Append(cameraPosition.Z);

			//fade the embedded element in and out with a Sine pattern
			Color colour = embeddedElement.Colour;
			colour.A = (byte)(Math.Sin(state.TotalTimeSeconds * 4) * 128 + 127);
			embeddedElement.Colour = colour;



			//draw everything
			drawToScreen.Draw(state);
		}


		//Override this method to setup the graphics device before the application starts.
		//This method is called before Initialise()
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
	}
}
