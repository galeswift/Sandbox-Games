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
using Xen.Ex.Graphics.Content;
using Microsoft.Xna.Framework.GamerServices;


/*
 * This sample extends from Tutorial_11 (ModelAnimation)
 * This sample demonstrates:
 * 
 * Showing an Avatar, and playing a custom animation
 * 
 */
namespace Tutorials.Tutorial_27
{
	//this class represents the avatar...
	class Avatar : IDraw, IContentOwner, IUpdate
	{
		//The avatar rendering instance:
		//(This class is very similar to ModelInstance)
		private AvatarInstance avatar;

		//An avatar animation controller,
		private AvatarAnimationController animationController;
		//And two animation instance handles.
		private AnimationInstance cheerAnimation, walkAnimation;

		//load the avatar
		public Avatar(ContentRegister content, UpdateManager update, out bool contentLoaded)
		{
			//create a random avatar description...
			Microsoft.Xna.Framework.GamerServices.AvatarDescription description;
			description = Microsoft.Xna.Framework.GamerServices.AvatarDescription.CreateRandom();

			//Create the avatar instance
			avatar = new AvatarInstance(description, true);


			//Create the animation controller.
			animationController = avatar.GetAnimationController();

			//NOTE: Animations cannot be played until the avatar animation data has been loaded...
			update.Add(this);

			//to play animations from a file, the user must download additional content.
			//however, we don't want to crash if they haven't... so only crash if the debugger is present
			//this will allow the user to download the content.
			contentLoaded = false;

			try
			{
				content.Add(this);
				contentLoaded = true;
			}
			catch
			{
				content.Remove(this);	//failed! but don't crash
			}

			if (contentLoaded)
			{
				//At this point in this tutorial, the animation is now loaded.

				//get the index of the walk animation
				int animationIndex = animationController.AnimationIndex("Walk");

				//begin playing the animation, looping
				walkAnimation = animationController.PlayLoopingAnimation(animationIndex);
			}
		}

		//draw the avatar
		public void Draw(DrawState state)
		{
			avatar.Draw(state);
		}

		//cull test the avatar, in this case, the cull test is highly approximate.
		public bool CullTest(ICuller culler)
		{
			return avatar.CullTest(culler);
		}

		//load the avatar animation...
		public void LoadContent(ContentState state)
		{
			//load the avatar data into the avatar instance

			/********************************************************************************************/
			/********************************************************************************************/
			try
			{
				//Something gone wrong? read below....
				avatar.AddNamedAnimation(state.Load<AvatarAnimationData>(@"AvatarAnimations\walk"), "Walk");
			}
			catch 
			{
				#if XBOX
				//Something gone wrong? read below....
				if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
				#endif
				throw;
			}
			/********************************************************************************************/
			/********************************************************************************************/
			/*																							*/
			/*	Getting an exeption here? Here is how to fix it: (or press resume to continue)			*/
			/*																							*/
			/********************************************************************************************/
			/********************************************************************************************/

			/*/
			 * 
			 * This sample demonstrates two forms of avatar animation, 'preset' animations that are
			 * standard animations available to all XNA applications, and custom animations imported
			 * from an FBX file.
			 * 
			 * The fbx used in this tutorial is walk.fbx, from the XNA custom avatar animation sample.
			 * This sample is for premium members only (so can't be redistributed) and is also a quite
			 * large download. (Walk.fbx is 10MB!)
			 * 
			 * The sample can be downloaded here:
			 * http://creators.xna.com/en-US/sample/customavataranimation
			 * http://creators.xna.com/downloads/?id=387
			 * 
			 * Once downloaded and extracted, find the file Walk.fbx, and place it in the 'AvatarAnimations'
			 * directory in the Xen Tutorial Content project.
			 * Once added, set the content processor to 'Avatar Animation - Xen'.
			 * 
			 * Note:
			 * 
			 * The XNA sample assumes there is a single animation per FBX file.
			 * Use the 'AddNamedAnimation' method to load these types of animation files. For FBX
			 * files with multiple animations (that are already named) use the 'AddAnimationSource'
			 * method.
			 * 
			/*/
		}

		//update...
		public UpdateFrequency Update(UpdateState state)
		{
			//when pressing A, create a new avatar description
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.A.OnPressed)
			{
				this.avatar.AvatarDescription = Microsoft.Xna.Framework.GamerServices.AvatarDescription.CreateRandom();
			}

			//if pressing B, play the cheer animation
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.B.OnPressed)
			{
				//start or stop the cheer animation (which is a built in animation in XNA)
				//use a fadein / fadeout
				if (!cheerAnimation.AnimationFinished)
					cheerAnimation.StopAnimation(0.5f);
				else
					cheerAnimation = animationController.PlayPresetAnimation(AvatarAnimationPreset.Celebrate, true, 0.5f, 0.5f);
			}

			//when the cheer animation is playing, fade the walking animation out,
			//do this based on the opposite weighitng of the cheer animation (taking fading into account)
			this.walkAnimation.Weighting = 1 - this.cheerAnimation.GetFadeScaledWeighting(state);

			return UpdateFrequency.FullUpdate60hz;
		}
	}




	[DisplayName(Name = "Tutorial 27: Avatars (Requires Additional Download)")]
	public class Tutorial : Application
	{
		//screen draw target
		private DrawTargetScreen drawToScreen;
		private TextElement text;

		//NOTE:
		//In XNA, Avatars require the GamerServices Component to be initalised. 
		//However, XNA is *very* picky about where this component is initalised.
		//It also adds quite a bit to a games' startup time and will prevent an application
		//from running twice from the same process.
		//To enable the gamer services object to be created, override this method and return true:
		protected override bool ApplicationRequiresGamerServices
		{
			get { return true; }
		}

		protected override void Initialise()
		{
			Camera3D camera = new Camera3D();
			camera.LookAt(new Vector3(0, 1, 0), new Vector3(0.5f, 1, -1), new Vector3(0, 1, 0));
			camera.Projection.NearClip = 0.1f;

			//create the draw target.
			drawToScreen = new DrawTargetScreen(camera);
			drawToScreen.ClearBuffer.ClearColour = Color.CornflowerBlue;

			bool contentLoaded;

			//create the avatar instance
			drawToScreen.Add(new Avatar(this.Content, this.UpdateManager, out contentLoaded));

#if XBOX
			text = new TextElement(@"Press 'A' to display a new Avatar" + Environment.NewLine + "Press 'B' to play the 'Cheer' animation.");
#else
			text = new TextElement(@"Unfortunately, avatars are only supported on the XBOX...." + Environment.NewLine + "AvatarInstance will display a wireframe of an avatar skeleton, using any FBX imported animations." + Environment.NewLine + "XNA preset animations cannot be displayed in this wireframe mode.");
#endif
			if (!contentLoaded)
			{
				//could not load the animation... user needs to download it themselves
				text.Text.AppendLine();
				text.Text.AppendLine(@"ERROR: An additional download is required!");
				text.Text.AppendLine(@"Avatar walk animation could not be loaded! See code comments for details!");
				text.Colour = Color.Red;
			}

			text.Position = new Vector2(50, -50);
			drawToScreen.Add(text);
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

		protected override void LoadContent(ContentState state)
		{
			this.text.Font = state.Load<SpriteFont>(@"Arial");
		}
	}
}
