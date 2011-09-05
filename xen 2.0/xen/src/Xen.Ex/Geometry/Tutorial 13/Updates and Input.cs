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
using Xen.Input.State;

/*
 * This sample extends from Tutorial_10 (Model Instance)
 * This sample demonstrates:
 * 
 * Using the IUpdate interface to perform update logic
 * Responding PlayerInput
 * 
 * 
 * In this tutorial, the camera is moved with either
 * the gamepad, or the mouse if no gamepad is found
 * 
 */
namespace Tutorials.Tutorial_13
{
	//This example tutorial uses the Actor class from Tutorial_10 to draw a model

	//The class is just an example of how to use IUpdate and PlayerInput.
	//This class rotates the camera around the origin (0,0,0) based on
	//user input

	//NEW CODE
	class CameraOrbit : IUpdate
	{
		private Camera3D camera;
		private Vector2 cameraRotation;

		//The class is constructed with a camera and a reference to the Applications
		//UpdateManager
		public CameraOrbit(Camera3D camera, UpdateManager manager)
		{
			//keep a reference to the camera
			this.camera = camera;


			//Add this object to the update manager
			manager.Add(this);
		}

		//This is the core of game logic in xen, the Update() method
		//The Update method returns an UpdateFrequency,
		//This tells the update manager how often this object
		//wishes to be updated. In this example, 'FullUpdate60hz' will
		//be used, which indicates that Update() should be called 60
		//times per second.
		//NOTE: All player input is also updated at 60hz, so any class
		//that deals with player input should also run at 60hz.
		//
		//There is a special value, UpdateFrequency.Terminate which
		//can be returned. When used, the UpdateManager will remove
		//the object from its internal list.
		public UpdateFrequency Update(UpdateState state)
		{
			//Here, a reference to the state of the first players input is retrieved
			InputState playerInput = state.PlayerInput[PlayerIndex.One].InputState;

			//The InputState object stores the state of a virtual gamepad.
			//It stores all the values of a gamepad, such as
			//playerInput.ThumbSticks.LeftStick. However, under the hood
			//these values can be rewired. This can be done with the classes
			//within the Input namespace.
			
			//One example, is that the keyboard and mouse on the PC
			//are emulated as a gamepad in xen, using the mouse and WASD.
			//While the raw keyboard and mouse state are also available:
			//state.KeyboardState
			//state.MouseState
			//state.GetGamePadState(...)

			//NOTE: Using the XNA inbuilt 'GetState' methods such as
			//GamePad.GetState() is *very* slow. Avoid them at all costs!

			//With that said, the defaults control methods are: (For player one)
			//
			//Xbox360: No change, acts just like a gamepad
			//Windows with a Gamepad: No change
			//Windows without a Gamepad: Keyboard & mouse emulation

			//This can be changed. Example, to force player one to use a gamepad:
			//state.PlayerInput[PlayerIndex.One].ControlInput = Xen.Input.ControlInput.GamePad1;

			//With a lot of mouse driven games, it is desirable to keep the mouse
			//in the centre of the window, allowing greater freedom of movement.
			//This allows the player to rotate as much as they want, without the mouse
			//stopping at the edge of the screen:

			//state.PlayerInput[PlayerIndex.One].InputMapper.CentreMouseToWindow = true;
			

			//phew!

			//now. the camera logic.

			//rotation speed...
			float sensitivity = 5;

			//make sure it's scaled by the time step (in this case, it'll be 1/60)
			sensitivity *= state.DeltaTimeSeconds;

			this.cameraRotation.X += playerInput.ThumbSticks.RightStick.X * sensitivity;
			this.cameraRotation.Y += playerInput.ThumbSticks.RightStick.Y * sensitivity;

			//clamp the rotation up/down
			if (this.cameraRotation.Y > MathHelper.PiOver2)
				this.cameraRotation.Y = MathHelper.PiOver2;
			if (this.cameraRotation.Y < -MathHelper.PiOver2)
				this.cameraRotation.Y = -MathHelper.PiOver2;

			float viewDistance = 8;

			//when the 'A' button is held down, bring the view closer.
			//Note: with mouse/keyboard, button 'a' becomes the space bar (the default)
			if (playerInput.Buttons.A.IsDown)
				viewDistance = 6;

			//example, to change the mapping from the space bar to the enter key:
			//
			//state.PlayerInput[PlayerIndex.One].KeyboardMouseControlMapping.A =
			//			Microsoft.Xna.Framework.Input.Keys.Enter;

			//this is a fairly nasty way to generate the camera matrix :-)
			Matrix cameraMatrix =
				Matrix.CreateTranslation(0, 0, viewDistance) * //move +viewDistance on the z-axis (negative z-axis is into the screen, so positive moves away)
				Matrix.CreateRotationX(this.cameraRotation.Y + MathHelper.PiOver2) * // rotate up/down around x-axis (the model is rotated 90 deg, hence the PiOver2)
				Matrix.CreateRotationZ(this.cameraRotation.X); //then finally rotate around the z-axis left/right

			//update the camera matrix
			this.camera.SetCameraMatrix(ref cameraMatrix);

			//request that this object have Update called 60 times
			//per second. Note that this value can be changed at will,
			//So objects could update at different rates depending on
			//how close they are to the player, etc.
			return UpdateFrequency.FullUpdate60hz;
		}

		//New UpdateManagers can be created, and they can even be
		//added to other UpdateManagers. They can also be 
		//Enabled/Disabled with ease, creating a simple way to
		//control large portions of application logic

	}


	[DisplayName(Name = "Tutorial 13: IUpdate and Player Input")]
	public class Tutorial : Application
	{
		//screen draw target
		private DrawTargetScreen drawToScreen;
		private CameraOrbit cameraOrbit;

		protected override void Initialise()
		{
			Camera3D camera = new Camera3D();

			//create the draw target.
			drawToScreen = new DrawTargetScreen(this, camera);
			drawToScreen.ClearBuffer.ClearColour = Color.CornflowerBlue;

			//NEW CODE
			//create the CameraOrbit object..
			//pass in the application update manager
			cameraOrbit = new CameraOrbit(camera, this.UpdateManager);

			//create the actor instance from Tutorial_10
			drawToScreen.Add(new Tutorial_10.Actor(this.Content, Vector3.Zero));
		}

		//NEW CODE
		//Finally, the method 'InitialisePlayerInput' can be overridden to change
		//player input settings when the application starts up.
		//This method is called right after Initialise

		protected override void InitialisePlayerInput(Xen.Input.PlayerInputCollection playerInput)
		{
			//if using the mouse, then make it centre to the window
			 
			if (playerInput[PlayerIndex.One].ControlInput == Xen.Input.ControlInput.KeyboardMouse)
				playerInput[PlayerIndex.One].InputMapper.CentreMouseToWindow = true;
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
