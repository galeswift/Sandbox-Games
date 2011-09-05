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
 * This is a *large* tutorial. 
 * It contains many classes and interfaces.
 * 
 * 
 * This sample demonstrates:
 * 
 * Implementing a simple game state / manager system as an *example* for
 * using the Xen ThreadPool to perform background loading
 * 
 */
namespace Tutorials.Tutorial_26
{

	/*
	 * 
	 * In XNA, all content is loaded through a ContentManager.
	 * The Xen Equivalent to this class is a ContentRegister.
	 * Internally, a ContentRegister wraps a ContentManager.
	 * 
	 * The names are similar, because the classes perform a similar
	 * duty. However, Xen ContentRegister stores a register of objects
	 * that implement the IContentOwner interface. 
	 * (Whereas ContentManager simply loads the content asked).
	 * 
	 * IMPORTANT NOTE: 
	 *		ContentManager stores a weak-reference list of IContentOwners,
	 *		this means you do *not* need to remove items from a ContentManager
	 *		to prevent garbage buildup.
	 *		However, XNA ContentManager resources cannot be unloaded 
	 *		one-by-one, and will not naturally be garbage collected 
	 *		when not in use.
	 * 
	 * 
	 * The only way to correctly Unload XNA content is by
	 * unloading the entire XNA ContentManager.
	 * 
	 * This means disposing the entire Xen ContentRegister.
	 * Because of this, it can be limiting to have an application
	 * use only the application global ContentRegister.
	 * (And trying to dispose the application global ContentRegister
	 * mid-game would be very risky!)
	 * 
	 * To get around this, it is best to create a new ContentRegister
	 * for each level that is loaded.
	 * This way, the level data can be unloaded when moving to the 
	 * next level.
	 * 
	 * 
	 * This tutorial demonstrates this by creating a second 
	 * ContentRegister.
	 * It also demonstrates how to use the Xen ThreadPool to 
	 * perform this loading operation on a background thread.
	 * 
	 * This allows loading progress to be displayed while loading.
	 * 
	 * 
	 * ***IMPORTANT***
	 * It is best that a ContentRegister is used by ONLY the thread
	 * it was created on.
	 * ***************
	 * 
	 * 
	 */




	//A class to simulate slow loading content
	class ExpensiveContent : IContentOwner
	{
		public int FakeData;

		void IContentOwner.LoadContent(ContentState state)
		{
			//this is just making work...
			//and it's *really* slow on the xbox :D
#if XBOX360 || DEBUG
			for (int i = 0; i < 1000; i++)
#else
			for (int i = 0; i < 10000; i++)
#endif
			{
				if (Math.Sqrt(DateTime.Now.Ticks) > Math.Tan(DateTime.Today.Ticks))
					FakeData++;
			}
		}
	}





	//Here, a very simple game state management system is simulated.
	//It has a StartScreenState, MenuState, a LoadingState and a PlayingState

	//Here is an interface for a game state manager, 
	//which keeps track of what state is currently active
	interface IGameStateManager
	{
		Application Application { get; }
		PlayerIndex PlayerIndex { get; set; }

		//change to a different state
		void SetState(IGameState state);
	}

	//And here is an interface for a game state, (such as a menu)
	//This is *highly* simplified.
	interface IGameState
	{
		void Initalise(IGameStateManager stateManager);

		//simplified IDraw/IUpdate
		//NOTE:
		//For simplicity this only provides drawing directly to the screen.
		void DrawScreen(DrawState state);
		void Update(UpdateState state);
	}

	//This is an interface for a progressive loading state,
	//it inherits from IGameState.
	//(eg, this could be a level that loads in the background while a progress bar is displayed)
	interface IProgressGameState : IGameState, IDisposable
	{
		float LoadingPercentage { get; }
		bool LoadingComplete { get; }

		void BeginLoad();
	}




	//This class is used on the XBOX,
	//it demonstrates a simple way to choose which controller to use.
	class StartScreenState : IGameState, IContentOwner
	{
		private TextElementRect startText;
		private IGameStateManager stateManager;

		public void Initalise(IGameStateManager stateManager)
		{
			this.stateManager = stateManager;

			//put some text up...
			this.startText = new TextElementRect(new Vector2(400,100));
			this.startText.Text.SetText("Press the START button to begin");
			this.startText.VerticalAlignment = VerticalAlignment.Centre;
			this.startText.HorizontalAlignment = HorizontalAlignment.Centre;
			this.startText.TextHorizontalAlignment = TextHorizontalAlignment.Centre;
			this.startText.Colour = Color.Red;

			//set the text font
			stateManager.Application.Content.Add(this);
		}

		public void DrawScreen(DrawState state)
		{
			//display the text
			startText.Draw(state);
		}

		public void Update(UpdateState state)
		{
			bool controllerSelected = false;

			//wait for a controller to press a button
			for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
			{
				//test each controller...
				Xen.Input.State.InputState input = state.PlayerInput[index].InputState;

				//has a button been pressed?
				if (input.Buttons.A.OnReleased ||
					input.Buttons.Start.OnReleased)
				{
					//Controller selected!
					//this controller will be used from now on...
					this.stateManager.PlayerIndex = index;
					controllerSelected = true;
				}
			}

			if (controllerSelected)
			{
				//go to the menu state.
				this.stateManager.SetState(new MenuState());
			}
		}

		void IContentOwner.LoadContent(ContentState state)
		{
			//load the text font.
			this.startText.Font = state.Load<SpriteFont>("Arial");
		}
	}


	//Here is a class to present a highly complex main menu :-)
	class MenuState : IGameState, IContentOwner
	{
		private TextElementRect menuText;
		private IGameStateManager stateManager;


		public void Initalise(IGameStateManager stateManager)
		{
			this.stateManager = stateManager;

			//display an incredibly complex line of text
			this.menuText = new TextElementRect(new Vector2(400, 100));
			this.menuText.Text.SetText("Press a button to start a new game!");
			this.menuText.VerticalAlignment = VerticalAlignment.Centre;
			this.menuText.HorizontalAlignment = HorizontalAlignment.Centre;
			this.menuText.TextHorizontalAlignment = TextHorizontalAlignment.Centre;

			//set the text font (using global content)
			stateManager.Application.Content.Add(this);
		}

		public void DrawScreen(DrawState state)
		{
			//display the 'menu' :-)
			menuText.Draw(state);
		}

		public void Update(UpdateState state)
		{
			//when a button is pressed, load the game..
			//Note the player index selected in the startup screen is used here...
			Xen.Input.State.InputState input = state.PlayerInput[this.stateManager.PlayerIndex].InputState;

			if (input.Buttons.A.OnReleased ||
				input.Buttons.B.OnReleased ||
				input.Buttons.X.OnReleased ||
				input.Buttons.Y.OnReleased)
			{
				//we want to start playing the game!

				//create a new game to play.
				IProgressGameState gameState = new PlayingState(this.stateManager.Application);

				//load the game, through a LoadingState
				IGameState loadingState = new LoadingState(gameState);

				//go to the loading state.
				this.stateManager.SetState(loadingState);
				return;
			}

			if (input.Buttons.Back.OnPressed)
			{
				//quit when back is pressed in the menu
				this.stateManager.Application.Shutdown();
				return;
			}
		}

		void IContentOwner.LoadContent(ContentState state)
		{
			//load the text font.
			this.menuText.Font = state.Load<SpriteFont>("Arial");
		}
	}


	//This state shows the loading progress of the state to be loaded.
	class LoadingState : IGameState
	{
		//store the progress state which will be loaded.
		private readonly IProgressGameState stateToLoad;

		//store the state manager
		private IGameStateManager stateManager;
		private SolidColourElement loadingBar, loadingBackground;


		public LoadingState(IProgressGameState stateToLoad)
		{
			this.stateToLoad = stateToLoad;
		}


		public void Initalise(IGameStateManager stateManager)
		{
			this.stateManager = stateManager;

			//Put up a beautiful and intricate loading bar (a green box on top of a black box)
			this.loadingBar = new SolidColourElement(Color.Lime, new Vector2(0, 0.15f), true);
			this.loadingBackground = new SolidColourElement(Color.Black, new Vector2(0.52f, 0.17f), true);

			this.loadingBar.Position = new Vector2(0.25f, 0.35f);
			this.loadingBackground.Position = new Vector2(0.24f, 0.34f);

			//tell the next state (in this case, it'll be the game) to begin loading
			this.stateToLoad.BeginLoad();
		}

		public void DrawScreen(DrawState state)
		{
			//set the loading block size to the loading progress..
			this.loadingBar.Size = new Vector2(0.5f * stateToLoad.LoadingPercentage / 100F, 0.15f);

			//draw the loading bar
			this.loadingBackground.Draw(state);
			this.loadingBar.Draw(state);

			//single core PC? (No task threads were created)
			if (this.stateManager.Application.ThreadPool.ThreadCount == 0)
			{
				//Sleep the CPU for a bit.
				//this will allow some breathing room for the loading thread
				System.Threading.Thread.Sleep(30);
			}
		}

		public void Update(UpdateState state)
		{
			//once loading is complete...
			if (this.stateToLoad.LoadingComplete)
			{
				//move to the next state (ie, the state being loaded)
				this.stateManager.SetState(this.stateToLoad);
				return;
			}
		}
	}





	//This state stores the game objects, it's the actual state where the game is active.
	//It will draw a bunch of sprites, which bounce around.
	//The sprites will be partially updated in a task thread.
	//loading will be simulated by creating 'ExpensiveContent' objects (which do absolutely nothing)
	class PlayingState : IProgressGameState, Xen.Threading.IAction
	{
		//the instance of the background threaded content register.
		private readonly ThreadedContentRegister contentRegister;
		private IGameStateManager stateManager;

		//a bunch of sprites that will bounce around the screen
		private SpriteElement spriteElement;
		private readonly Vector2[] spritePosition;
		private readonly Vector2[] spriteVelocity;

		private int updateIndex;
		private Random rand;

#if DEBUG
		private const int SpriteCount = 8000;
#else
		private const int SpriteCount = 25000;
#endif

		//input position to control the particles
		private Vector2 inputPosition;
		//buffered position, for the task thread which will process the particles
		//Note: For thread safety, it's always a very good idea to buffer data
		//when you know the two threads won't conflict
		private Vector2 inputPositionBuffer;

		//when true, part of the particle processing will be performed as a thread task
		private bool runSingleThreaded;

		//This is a wait callback handle to the particle processing thread task
		private Xen.Threading.WaitCallback updateTask;

		private Vector2 windowSize;


		//create the state...
		//Note: this is done before the loading state is displayed.
		public PlayingState(Application application)
		{
			//create the threaded content register
			this.contentRegister = new ThreadedContentRegister(application);

			this.rand = new Random();

			this.spritePosition = new Vector2[SpriteCount];
			this.spriteVelocity = new Vector2[SpriteCount];
		}

		//these properties are required by the IProgressGameState interface
		public float LoadingPercentage { get { return this.contentRegister.LoadingPercentage; } }
		public bool LoadingComplete { get { return this.contentRegister.LoadingComplete; } }


		//this is called by the loading screen state to trigger loading to begin
		public void BeginLoad()
		{
			//add a bunch of complex objects to be loaded...
			for (int i = 0; i < 100; i++)
				contentRegister.Add(new ExpensiveContent());

			//trigger the register to begin loading
			contentRegister.BeginLoad();
		}


		//this will be called once this game state is up and running.
		public void Initalise(IGameStateManager stateManager)
		{
			this.stateManager = stateManager;

			//create the sprites, which will bounce around the screen

			//sprite drawer
			this.spriteElement = new SpriteElement();

			//the sprite positions / velocities
			for (int i = 0; i < SpriteCount; i++)
			{
				this.spritePosition[i] = new Vector2((float)rand.NextDouble() * stateManager.Application.WindowWidth * 0.25f, (float)rand.NextDouble() * stateManager.Application.WindowHeight * 0.25f);
				this.spriteVelocity[i] = new Vector2((float)rand.NextDouble() * 2000 - 1000, (float)rand.NextDouble() * 2000 - 1000);

				//add a sprite for each to the sprite drawer
				this.spriteElement.AddSprite(spritePosition[i], new Vector2(1.5f, 1.5f));
			}
		}

		//draw the sprites!
		public void DrawScreen(DrawState state)
		{
			//(for all but the first frame)
			//at this point, sprites may be being processed on the sprite thread


			//This method call updates the sprite effect.
			//Note, it does this by waiting for the sprite update task to finish,
			//copying the sprite positions, then starting the sprite update again.
			//The sprite update runs while the drawing/app loop is running
			UpdateEffect(state);

			//at this point, the sprite task has probably started on another thread...
			//it will be calculating sprite positions for the *next* frame.

			//draw the sprites
			this.spriteElement.Draw(state);
		}

		//read the user input position, which is used to control the sprites.
		public void Update(UpdateState state)
		{
			Xen.Input.State.InputState input = state.PlayerInput[this.stateManager.PlayerIndex].InputState;

			//compute the new input position (thumbstick location of the magnet)
			//note, this isn't used for the calculations directly, inputPositionBuffer is...
			this.inputPosition = input.ThumbSticks.RightStick + input.ThumbSticks.LeftStick;
			this.inputPosition.X = this.inputPosition.X * 0.25f + 0.5f;
			this.inputPosition.Y = this.inputPosition.Y * 0.25f + 0.5f;
			this.inputPosition *= this.windowSize;

			//if A is held down, the sprites are processed without the thread task
			runSingleThreaded = input.Buttons.A;

			//quit back to the menu?
			if (input.Buttons.Back.OnPressed)
			{
				//wait for the sprite processing to finish (if it's still running)
				updateTask.WaitForCompletion();

				//go back to the main menu when back is pressed
				this.stateManager.SetState(new MenuState());
				return;
			}
		}

		//This method call updates the sprite effect.
		//Note, it does this by waiting for the sprite update task to finish,
		//copying the sprite positions, then starting the sprite update again.
		//The sprite update runs while the drawing/app loop is running
		public void UpdateEffect(IState state)
		{
			//wait for the previous frame update to finish (it may still be running)...
			updateTask.WaitForCompletion();


			//Note: at this point, the thread task is *not* active, so it is safe to
			//read/write the data used by the thread. Doing so before WaitForCompletion
			//would potentially be unsafe.


			//store the inputPosition into the buffer, so the task can access it.
			inputPositionBuffer = inputPosition;


			//copy out the updated position data... 
			//note, the update task isn't active when this is called
			//otherwise the data may be still being written to by the thread task
			//(The thread task writes directly to 'spritePosition')
			this.spriteElement.SetSpritePositions(spritePosition);


			this.windowSize = new Vector2(state.Application.WindowWidth, state.Application.WindowHeight);
			
			//now, start the process up again.

			//being a new update task...
			//(this task could be split up, to help the xbox out even more)

			if (!runSingleThreaded)
			{
				//make sure the WaitCallback is assigned!
				updateTask = state.Application.ThreadPool.QueueTask(this, null);

				//now, at some point, the PerformAction method below will be called,
				//usually from another thread.

				//It's quite possible that the method has already been called and
				//has finished already by this point.
			}
			else
				UpdateSprites(); //single thread update
		}

		//this will be called by the thread pool,
		public void PerformAction(object data)
		{
			//animate the sprites...
			UpdateSprites();
		}

		//performs a basic 'magnet' effect on a large number of randomly moving sprites.
		private void UpdateSprites()
		{
			//note, this method accesses members of this class. It's very important that
			//these values are not read/written while this process is active.
			
			//animate the sprites...
			float speed = 0.01f;

			for (int i = 0; i < SpriteCount; i++)
			{
				Vector2 pos = spritePosition[i];
				Vector2 vel = spriteVelocity[i];

				//gravity
				vel.Y -= 12;

				//distance to the magnet
				float dx = pos.X - inputPositionBuffer.X;
				float dy = pos.Y - inputPositionBuffer.Y;

				pos.X += vel.X * speed;
				pos.Y += vel.Y * speed;
				
				float attraction = 25 / (float)Math.Sqrt(Math.Max(dx * dx + dy * dy, 0.001f));

				//attact more when moving towards the magnet.

				vel.X -= dx * attraction;
				vel.Y -= dy * attraction;
				vel.X *= 0.99f;
				vel.Y *= 0.99f;

				//bounces off the edges
				if (pos.X > windowSize.X && vel.X > 0) vel.X = vel.X * -0.85f;
				if (pos.Y > windowSize.Y && vel.Y > 0) vel.Y = vel.Y * -0.85f;
				if (pos.X < 0 && vel.X < 0) vel.X = vel.X * -0.85f;
				if (pos.Y < 0 && vel.Y < 0) vel.Y = vel.Y * -0.85f;

				spritePosition[i] = pos;
				spriteVelocity[i] = vel;
			}

			//reset 0.5% of the sprite per frame
			for (int i = 0; i <= (SpriteCount/500); i++)
			{
				this.spritePosition[updateIndex % this.spritePosition.Length] =
					new Vector2((float)rand.NextDouble() * windowSize.X * 0.25f, (float)rand.NextDouble() * windowSize.Y * 0.25f);
				
				this.spriteVelocity[updateIndex % this.spriteVelocity.Length] =
					new Vector2((float)rand.NextDouble() * 2000 - 1000, (float)rand.NextDouble() * 2000 - 1000);

				updateIndex++;
			}

			//Done!
		}

		public void Dispose()
		{
			this.contentRegister.Dispose();
		}
	}





	//Finally, the gamestate manager.
	//This implements the interface and stores the current state object.
	class GameStateManager : IGameStateManager, IDraw, IUpdate
	{
		private readonly Application application;
		private PlayerIndex playerIndex;

		//the current state object
		private IGameState currentGameState;



		public GameStateManager(Application application)
		{
			this.playerIndex = PlayerIndex.One;
			this.application = application;
			
#if XBOX360
			//to begin with, have a 'StartScreenState' on the xbox
			this.SetState(new StartScreenState());
#else
			//otherwise jump straight to the menu
			this.SetState(new MenuState());
#endif
		}

		//interface members
		Application IGameStateManager.Application
		{
			get { return application; }
		}
		PlayerIndex IGameStateManager.PlayerIndex
		{
			get { return playerIndex; }
			set { playerIndex = value; }
		}

		//change to a new state.
		public void SetState(IGameState state)
		{
			//dispose the old state first (otherwise it's resources might stick around!)
			if (this.currentGameState is IDisposable)
			   (this.currentGameState as IDisposable).Dispose();

			this.currentGameState = state;

			//call Initalise() on the new state
			state.Initalise(this);
		}

		public void Draw(DrawState state)
		{
			//draw the state
			this.currentGameState.DrawScreen(state);
		}

		bool ICullable.CullTest(ICuller culler)
		{
			return true;
		}

		public UpdateFrequency Update(UpdateState state)
		{
			//update the current state
			this.currentGameState.Update(state);

			return UpdateFrequency.FullUpdate60hz;
		}
	}






	//and finally, the application.
	[DisplayName(Name = "Tutorial 26: Game State and Threading")]
	public class Tutorial : Application
	{
		//screen draw target
		private DrawTargetScreen drawToScreen;

		//Bonus: On the PC, pressing F1 toggles fullscreen.
		//to do this, the GraphicsDeviceManager needs to be stored.
		private GraphicsDeviceManager graphicsManager;
		private Xen.Ex.Graphics2D.Statistics.DrawStatisticsDisplay stats;

		protected override void Initialise()
		{
			//create the draw target.
			drawToScreen = new DrawTargetScreen(new Camera2D());
			drawToScreen.ClearBuffer.ClearColour = Color.CornflowerBlue;
			
			//create the GameStateManager
			GameStateManager manager = new GameStateManager(this);

			//add it to the screen, and to be updated
			this.drawToScreen.Add(manager);
			this.UpdateManager.Add(manager);

			stats = new Xen.Ex.Graphics2D.Statistics.DrawStatisticsDisplay(this.UpdateManager);
			this.drawToScreen.Add(stats);
		}

		protected override void Frame(FrameState state)
		{
			drawToScreen.Draw(state);
		}

		protected override void Update(UpdateState state)
		{
			//if F1 is pressed, fullscreen mode is toggled.
#if !XBOX360
			if (graphicsManager != null &&
				state.KeyboardState[Microsoft.Xna.Framework.Input.Keys.F1].OnReleased)
			{
				graphicsManager.IsFullScreen = !graphicsManager.IsFullScreen;
				graphicsManager.ApplyChanges();
			}
#endif
		}

		protected override void SetupGraphicsDeviceManager(GraphicsDeviceManager graphics, ref RenderTargetUsage presentation)
		{
			//store the device manager (note it'll be null in WinForms)
			graphicsManager = graphics;
		}

		protected override void LoadContent(ContentState state)
		{
			stats.Font = state.Load<SpriteFont>("Arial");
		}
	}
}
