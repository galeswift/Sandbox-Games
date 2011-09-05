using System;
using System.Collections.Generic;
using System.Text;



using Xen;
using Xen.Camera;
using Xen.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xen.Ex.Graphics;
using Xen.Ex.Graphics.Content;
using Xen.Ex.Graphics.Display;
using Xen.Ex.Graphics2D;
using Xen.Ex.Filters;


/*
 * This is the frontend menu for the Xbox
 * It lets you run a tutorial app
 * This isn't intended a tutorial.
 * 
 * It's also a bit insane.
 */
namespace Tutorials.XenMenu
{
	public class XenMenuApp : Application
	{
		private DrawTargetScreen drawToScreen;
		private ParticleSystem backgroundParticles;
		private VelocityBillboardParticles2DElement backroundDrawer;
		private DrawTargetTexture2D particlesTarget;
		private DrawTargetTexture2D particlesBlurred;
		private TextureDownsample quaterDownsample;
		private BlurFilter blurFilter;

		private ElementRect menuBlock;
		private float menuVerticalScroll;

		public const int ElementTextOffset = 10;
		public const int ElementHeight = 30;
		public const int ElementWidth = 750;
		public const int ElementYOffset = 200;
		public const int ElementXOffset = 300;
		public const int ElementSeparation = 35;
		public const int ArrowXOffset = 285;
		public const int MaxScrollDown = 500;
		public const float FadeOutStart = 550;
		public const float FadeOutEnd = 640;
		public const float FadeInStart = 190;
		public const float FadeInEnd = 150;

		private int selection;
		private Type selectedTutorial;
		private readonly List<TutorialSelection> buttons;
		private TexturedElement logo;
		private TextElementRect helperText;

		public Type SelectedTutorial { get { return selectedTutorial; } }
		public int SelectedIndex { get { return selection; } }

		public XenMenuApp(int selection)
		{
			this.selection = selection;
			this.buttons = new List<TutorialSelection>();
		}

		protected override void Initialise()
		{
			var tutorials = new Dictionary<string, Type>();
			Program.FindTutorials(tutorials);
			
			Camera3D camera = new Camera3D();

			drawToScreen = new DrawTargetScreen(camera);
			
			backgroundParticles = new ParticleSystem(this.UpdateManager);
			backgroundParticles.GlobalValues[0] = ArrowXOffset;
			backgroundParticles.GlobalValues[2] = (float)this.WindowWidth;
			backgroundParticles.GlobalValues[3] = (float)this.WindowHeight;

			particlesTarget = new DrawTargetTexture2D(camera, this.WindowWidth, this.WindowHeight, SurfaceFormat.Color, DepthFormat.None);
			particlesBlurred = new DrawTargetTexture2D(camera, this.WindowWidth / 2, this.WindowHeight / 2, SurfaceFormat.Color, DepthFormat.None);

			DrawTargetTexture2D inter0 = null, inter1 = null;
			quaterDownsample = new TextureDownsample(particlesTarget, particlesBlurred, ref inter0, ref inter1, particlesBlurred.Width, particlesBlurred.Height);

			inter0 = new DrawTargetTexture2D(camera, particlesBlurred.Width, particlesBlurred.Height, SurfaceFormat.Color, DepthFormat.None);
			blurFilter = new BlurFilter(BlurFilterFormat.SevenSampleBlur, 1, particlesBlurred, inter0);

			backroundDrawer = new VelocityBillboardParticles2DElement(backgroundParticles, false);
			particlesTarget.Add(backroundDrawer);

			//draw the resolved particles to the screen
			drawToScreen.Add(new TexturedElement(particlesTarget, new Vector2(1, 1), true));

			//background block other elements are inserted into. invisible
			var selectionBlock = new Xen.Ex.Graphics2D.SolidColourElement(new Color(0, 0, 0, 0), new Vector2(ElementWidth, tutorials.Count * ElementSeparation));
			selectionBlock.AlphaBlendState = AlphaBlendState.Alpha;
			selectionBlock.VerticalAlignment = VerticalAlignment.Top;

			this.menuBlock = selectionBlock;

			int y_pos = 0;
			foreach (var tutorial in tutorials)
			{
				var tut_item = new TutorialSelection(tutorial.Key, y_pos, this.Content, selectionBlock, this.UpdateManager, tutorial.Value);

				y_pos -= ElementSeparation;
				buttons.Add(tut_item);
			}

			drawToScreen.Add(selectionBlock);	

			var bloom = new TexturedElement(particlesBlurred, new Vector2(1, 1), true);
			bloom.AlphaBlendState = AlphaBlendState.AdditiveSaturate;
			drawToScreen.Add(bloom);

			this.logo = new TexturedElement(new Vector2(282,100));
			this.logo.VerticalAlignment = VerticalAlignment.Top;
			this.logo.HorizontalAlignment = HorizontalAlignment.Centre;
			this.logo.Position = new Vector2(0, -50);

			this.helperText = new TextElementRect(new Vector2(800,100),"Use the DPAD to select an item, press 'A' to run the example\nWhen running an example, press 'back' to return to this menu");
			this.helperText.VerticalAlignment = VerticalAlignment.Bottom;
			this.helperText.HorizontalAlignment = HorizontalAlignment.Centre;
			this.helperText.TextHorizontalAlignment = TextHorizontalAlignment.Centre;
			this.helperText.TextVerticalAlignment = VerticalAlignment.Centre;
			this.helperText.Colour = Color.Gray;

			drawToScreen.Add(logo);
			drawToScreen.Add(helperText);
		}

		protected override void Update(UpdateState state)
		{
			menuBlock.Position = new Vector2(ElementXOffset, -(ElementYOffset + (int)menuVerticalScroll));

			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.DpadDown.IsPressedRepeats(state, 0.1f, 0.15f))
				selection++;
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.DpadUp.IsPressedRepeats(state, 0.1f, 0.15f))
				selection--;

			if (selection < 0) selection = 0;
			if (selection >= buttons.Count) selection = buttons.Count - 1;

			float selection_y = (ElementYOffset + ElementHeight * 0.5f + ElementSeparation * selection);

			float y_offset = selection_y - MaxScrollDown;
			if (y_offset < 0) y_offset = 0;
			menuVerticalScroll = y_offset * -0.2f + menuVerticalScroll * 0.8f;


			foreach (var item in buttons)
			{
				float y = item.YPos - menuBlock.Position.Y;
				
				item.Highlight = false;
				item.Fade = 1;

				if (y > FadeOutStart) item.Fade = 1 - (y - FadeOutStart) / (FadeOutEnd - FadeOutStart);
				if (y < FadeInStart) 
					item.Fade = 1 - (y - FadeInStart) / (FadeInEnd - FadeInStart);

				if (item.Fade < 0) item.Fade = 0;
			}

			if (buttons.Count > 0)
				buttons[selection].Highlight = true;

			this.backgroundParticles.GlobalValues[1] = WindowHeight - selection_y - menuVerticalScroll;

			if (buttons.Count > 0 && state.PlayerInput[PlayerIndex.One].InputState.Buttons.A.OnPressed)
			{
				selectedTutorial = buttons[selection].Value;
				this.Shutdown();
			}
			else
			if (state.PlayerInput[PlayerIndex.One].InputState.Buttons.Back.OnPressed)
			{
				selectedTutorial = null;
				this.Shutdown();
			}
		}

		protected override void Frame(FrameState state)
		{
			particlesTarget.Draw(state);
			
			quaterDownsample.Draw(state);
			blurFilter.Draw(state);
						
			drawToScreen.Draw(state);
		}

		protected override void LoadContent(ContentState state)
		{
			backgroundParticles.ParticleSystemData = state.Load<ParticleSystemData>("Particles/XboxMenu");
			helperText.Font = state.Load<SpriteFont>("Arial");
			logo.Texture = state.Load<Texture2D>("xen");
		}

		protected override void SetupGraphicsDeviceManager(GraphicsDeviceManager graphics, ref RenderTargetUsage presentation)
		{
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
		}
	}

	class TutorialSelection : IUpdate, IContentOwner
	{
		private readonly SolidColourElement[] background;
		private readonly TextElementRect text;
		private Vector3 Colour;
		public bool Highlight;
		public float Fade;
		public readonly float YPos;
		public readonly Type Value;

		public TutorialSelection(string text, int y_pos, IContentRegister content, ElementRect parent, UpdateManager update, Type value)
		{
			this.Value = value;
			this.YPos = XenMenuApp.ElementHeight * 0.5f - y_pos;

			background = new SolidColourElement[3];
			for (int i = 0; i < background.Length; i++)
			{
				background[i] = new SolidColourElement(Color.Black, new Vector2(parent.Size.X, XenMenuApp.ElementHeight));
				background[i].VerticalAlignment = VerticalAlignment.Top;
				background[i].HorizontalAlignment = HorizontalAlignment.Left;
				background[i].Position = new Vector2(0, (float)y_pos);

				parent.Add(background[i]);
			}
			//offset the elements a bit, to give a 3D effect.

			background[1].AlphaBlendState = AlphaBlendState.ModulateX2;
			background[2].AlphaBlendState = AlphaBlendState.ModulateX2;

			background[1].Size -= new Vector2(1, 1);
			background[1].Position += new Vector2(1, 0);
			background[2].Size -= new Vector2(1, 1);
			background[2].Position -= new Vector2(0, 1);


			this.text = new TextElementRect(new Vector2(parent.Size.X - XenMenuApp.ElementTextOffset, XenMenuApp.ElementHeight), text);
			this.text.VerticalAlignment = VerticalAlignment.Top;
			this.text.HorizontalAlignment = HorizontalAlignment.Left;
			this.text.Position = new Vector2(XenMenuApp.ElementTextOffset, (float)y_pos);

			parent.Add(this.text);

			content.Add(this);
			update.Add(this);
		}

		public void LoadContent(ContentState state)
		{
			text.Font = state.Load<SpriteFont>("CourierNew");
		}

		public UpdateFrequency Update(UpdateState state)
		{
			Vector3 color = new Vector3(0.08f, 0.08f, 0.08f);
			if (Highlight)
				color = new Vector3(0.15f, 0.5f, 0.05f);

			this.Colour = this.Colour * 0.75f + color * 0.25f;
			Vector3 half = Vector3.One * 0.5f;

			background[0].Colour = new Color(this.Colour * Fade);
			background[2].Colour = new Color(new Vector3(0.25f, 0.25f, 0.25f) * Fade + half * (1 - Fade));
			background[1].Colour = new Color(new Vector3(1, 1, 1) * Fade + half * (1-Fade));
			text.ColourFloat = new Vector4(this.Colour + Vector3.One * 0.5f, Fade);

			return UpdateFrequency.OncePerFrame;
		}
	}
}
