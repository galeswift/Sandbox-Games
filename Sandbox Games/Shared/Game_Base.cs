using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Sandbox_Games
{
    public class LightInfo
    {
        public LightArea lightArea;
        public Vector2 lightPosition;
        public Color lightColor;

        public LightInfo(GraphicsDevice inGraphics, Color inLightColor, Vector2 inLightPosition)
        {
            lightColor = inLightColor;
            lightPosition = inLightPosition;
            lightArea = new LightArea(inGraphics, ShadowmapSize.Size512);
        }
    }
    public class Game_Base : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteFont myFont;
        public Random randomizer;
        public List<ParticleGenerator> partGenList = new List<ParticleGenerator>();
        public List<ParticleModifier> partModList = new List<ParticleModifier>();

        public MouseState lastMouseState = new MouseState();
        public KeyboardState lastKeyboardState = new KeyboardState();
        public GamePadState lastGamePadState = new GamePadState();

        public KeyboardState currentKeyboardState = new KeyboardState();
        public GamePadState currentGamePadState = new GamePadState();
        public MouseState currentMouseState = new MouseState();

        public Color clearColor = Color.Black;
        public BloomComponent bloom;
        //int bloomSettingsIndex = 0;

        // Dynamic shadows        
        List<LightInfo> lights = new List<LightInfo>();

        RenderTarget2D screenShadows;
        ShadowmapResolver shadowmapResolver;
        QuadRenderComponent quadRender;        
        Texture2D tileTexture;

        // Motion blur
        /// <summary>
        /// Contains the current scene that should be drawn to the screen.
        /// </summary>
        RenderTarget2D currentSceneRenderTarget = null;

        /// <summary>
        /// Contains the results of all previous blends. Each time this is updated it is also faded out slightly
        /// using the blendAmount value. This way old positions eventually disappear.
        /// </summary>
        RenderTarget2D previousSceneRenderTarget = null;        

        public Game_Base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            randomizer = new Random((int)DateTime.UtcNow.Ticks);

            //Enable default sample behaviors.
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            
            //bloom = new BloomComponent(this);            
            //Components.Add(bloom);

            quadRender = new QuadRenderComponent(this);
            Components.Add(quadRender);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            myFont = Content.Load<SpriteFont>("MainFont");

            // Create the shadowmap resolver, and the light area
            shadowmapResolver = new ShadowmapResolver(GraphicsDevice, quadRender, ShadowmapSize.Size256, ShadowmapSize.Size1024);
            shadowmapResolver.LoadContent(Content);            
            screenShadows = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);                        
            tileTexture = Content.Load<Texture2D>("Sprites\\tile");
                       
            // Create the render targest for the motion blur
            currentSceneRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            previousSceneRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (lights.Count > 0)
            {
                lights[0].lightPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }

            HandleInput();
        }

        public void ClearLights()
        {
            lights.Clear();
        }
        public virtual void AddLight(Color inLightColor, Vector2 inLightPosition)
        {
            lights.Add(new LightInfo(GraphicsDevice, inLightColor, inLightPosition));
        }

        protected override void Draw(GameTime gameTime)
        {            
            DrawShadowCasters(gameTime);            
            DrawShadows(gameTime);
            DrawMotionBlur(gameTime);
            base.Draw(gameTime);            
            //DrawOverlayText();
        }

        // Overridden in child classes to draw the scene
        protected virtual void DrawScene(GameTime gameTime)
        {            
        }

        protected virtual void DrawBackground(GameTime gameTime)
        {
            //draw the tile texture tiles across the screen
            Rectangle source = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(tileTexture, Vector2.Zero, source, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.End();
        }

        protected virtual void DrawShadowCasters(GameTime gameTime)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                // Begin drawing the light area
                lights[i].lightArea.LightPosition = lights[i].lightPosition;
                lights[i].lightArea.BeginDrawingShadowCasters();
                Vector2 localLightPos = lights[i].lightArea.ToRelativePosition(new Vector2(0, 0));
                // Need to transform the objects so their positions are relative to the light
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateTranslation(new Vector3(localLightPos.X, localLightPos.Y, 1)));
                DrawScene(gameTime);
                spriteBatch.End();
                lights[i].lightArea.EndDrawingShadowCasters();
                shadowmapResolver.ResolveShadows(lights[i].lightArea.RenderTarget, lights[i].lightArea.RenderTarget, lights[i].lightPosition);
            }
        }

        protected virtual void DrawShadows(GameTime gameTime)
        {            
            GraphicsDevice.SetRenderTarget(screenShadows);
            GraphicsDevice.Clear(clearColor);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            for (int i = 0; i < lights.Count; i++)
            {
                spriteBatch.Draw(lights[i].lightArea.RenderTarget, lights[i].lightArea.LightPosition - lights[i].lightArea.LightAreaSize * 0.5f, lights[i].lightColor);
            }
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);            
            GraphicsDevice.Clear(clearColor);


            //bloom.BeginDraw();


            //bloom.EndDraw(gameTime);
        }

        public virtual void DrawMotionBlur(GameTime gameTime)
        {


            // Draw the blended scene to the current scene render target, using the previous scene partially faded out
            GraphicsDevice.SetRenderTarget(currentSceneRenderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            BlendState blendState = new BlendState();
            
            //blendState.ColorBlendFunction = BlendFunction.Max;
            //blendState.ColorSourceBlend = Blend.One;
            //blendState.ColorDestinationBlend= Blend.One;
            
            blendState.ColorBlendFunction = BlendFunction.Add;
            blendState.ColorSourceBlend = Blend.SourceAlpha;
            blendState.ColorDestinationBlend= Blend.InverseSourceAlpha;

            blendState.AlphaBlendFunction = BlendFunction.Add;
            blendState.AlphaSourceBlend = Blend.One;
            blendState.AlphaDestinationBlend = Blend.InverseSourceAlpha;
            
            spriteBatch.Begin(SpriteSortMode.Deferred, blendState);
            spriteBatch.Draw(previousSceneRenderTarget, Vector2.Zero, new Color(255, 255, 255, 210));
            DrawScene(gameTime);
            spriteBatch.End();

            // Draw the blended scene to the current scene render target, using the previous scene partially faded out            
            GraphicsDevice.SetRenderTarget(previousSceneRenderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.Draw(currentSceneRenderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();

            // Draw everything to the screen (including the background which we aren't blending)
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Transparent);

            DrawBackground(gameTime);

            blendState = new BlendState();
            blendState.ColorSourceBlend = Blend.DestinationColor;
            blendState.ColorDestinationBlend = Blend.SourceColor;

            // Draw the blended scene to the current scene render target, using the previous scene partially faded out
            spriteBatch.Begin(SpriteSortMode.Immediate, blendState);
            spriteBatch.Draw(screenShadows, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.Begin();            
            spriteBatch.Draw(currentSceneRenderTarget, Vector2.Zero, Color.White);            
            spriteBatch.End();
        }
        /// <summary>
        /// Displays an overlay showing what the controls are,
        /// and which settings are currently selected.
        /// </summary>
        void DrawOverlayText()
        {
            string text = "A = settings (" + bloom.Settings.Name + ")\n" +
                          "B = toggle bloom (" + (bloom.Visible ? "on" : "off") + ")\n" +
                          "X = show buffer (" + bloom.ShowBuffer.ToString() + ")";

            spriteBatch.Begin();

            // Draw the string twice to create a drop shadow, first colored black
            // and offset one pixel to the bottom right, then again in white at the
            // intended position. This makes text easier to read over the background.
            spriteBatch.DrawString(myFont, text, new Vector2(35, 205), Color.Black);
            spriteBatch.DrawString(myFont, text, new Vector2(34, 204), Color.White);

            spriteBatch.End();
        }


        public void DrawStringInternal(string inString, Vector2 inPosition)
        {
            spriteBatch.Begin();
            // Draw with drop Shadow
            spriteBatch.DrawString(myFont, inString, inPosition + new Vector2(1, 1), Color.Black);            
            spriteBatch.DrawString(myFont, inString, inPosition, Color.White);
            spriteBatch.End();
        }

        #region Handle Input


        /// <summary>
        /// Handles input for quitting or changing the bloom settings.
        /// </summary>
        private void HandleInput()
        {
            lastKeyboardState = currentKeyboardState;
            lastGamePadState = currentGamePadState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // Check for exit.
            if (currentKeyboardState.IsKeyDown(Keys.Escape) ||
                currentGamePadState.Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }

           
            //// Switch to the next bloom settings preset?
            //if ((currentGamePadState.Buttons.A == ButtonState.Pressed &&
            //     lastGamePadState.Buttons.A != ButtonState.Pressed) ||
            //    (currentKeyboardState.IsKeyDown(Keys.A) &&
            //     lastKeyboardState.IsKeyUp(Keys.A)))
            //{
            //    bloomSettingsIndex = (bloomSettingsIndex + 1) %
            //                         BloomSettings.PresetSettings.Length;

            //    bloom.Settings = BloomSettings.PresetSettings[bloomSettingsIndex];
            //    bloom.Visible = true;
            //}

            //// Toggle bloom on or off?
            //if ((currentGamePadState.Buttons.B == ButtonState.Pressed &&
            //     lastGamePadState.Buttons.B != ButtonState.Pressed) ||
            //    (currentKeyboardState.IsKeyDown(Keys.B) &&
            //     lastKeyboardState.IsKeyUp(Keys.B)))
            //{
            //    bloom.Visible = !bloom.Visible;
            //}

            //// Cycle through the intermediate buffer debug display modes?
            //if ((currentGamePadState.Buttons.X == ButtonState.Pressed &&
            //     lastGamePadState.Buttons.X != ButtonState.Pressed) ||
            //    (currentKeyboardState.IsKeyDown(Keys.X) &&
            //     lastKeyboardState.IsKeyUp(Keys.X)))
            //{
            //    bloom.Visible = true;
            //    bloom.ShowBuffer++;

            //    if (bloom.ShowBuffer > BloomComponent.IntermediateBuffer.FinalResult)
            //        bloom.ShowBuffer = 0;
            //}
        }


        #endregion

    }
}
