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
    class Game_Fractals : Game_Base
    {
        enum EFractalType
        {
            EFT_Julia,
            EFT_Mandelbrot,
            EFT_Poly,
            EFT_Max,
        };

        EFractalType currentFractalType = EFractalType.EFT_Mandelbrot;

        /// <summary>
        /// Shader variables
        /// </summary>
        Effect effect_Fractal;

        

        /// <summary>
        /// Example 1.1: Effect objects used for this example
        /// </summary>
        #region Effect Fields
        private EffectParameter projectionParameter;
        private EffectParameter viewParameter;
        private EffectParameter worldParameter;        
        #endregion

        /// <summary>
        /// Example 1.2: Data fields corresponding to the effect paramters
        /// </summary>
        #region Uniform Data Fields
        private Matrix worldMatrix;        
        #endregion

        private Camera_Base myCamera;
        private Vector2 preClickMousePosition;       
        private Texture2D renderTexture;                

        public Game_Fractals() 
            : base()
        {                     
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            IsMouseVisible = true;

            // Load the shader
            effect_Fractal = Content.Load<Effect>("Shaders\\Fractals");

            // Create a camera
            SetUpCamera();

            // Create the texture to render to
            SetUpRenderTexture();

            // Initialize all the shader params
            SetUpEffectParams();
        }


        protected void SetUpCamera()
        {
            myCamera = new Camera_Base(this, GraphicsDevice.Viewport);
            myCamera.Position = new Vector3(0, 0, 0.0f);
            myCamera.LookAt = new Vector3(0.0f, 0.0f, -1.0f);
            Components.Add(myCamera);

            worldMatrix = Matrix.Identity;
        }


        private void SetUpRenderTexture()
        {
            renderTexture = new Texture2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, true, SurfaceFormat.Color);

            int numPixels = renderTexture.Width * renderTexture.Height;
            Color[] textureData = new Color[numPixels];

            for (int i = 0; i < numPixels; i++)
            {
                textureData[i] = new Color(255, (byte)(255 * i / numPixels), (byte)(255 * i / numPixels), 255);
            }

            renderTexture.SetData(textureData);
        }

        /// <summary>
        /// Example 1.3
        /// This function obtains EffectParameter objects from the Effect objects.
        /// The EffectParameters are handles to the values in the shaders and are
        /// effectively how your C# code and your shader code communicate.
        /// </summary>
        private void SetUpEffectParams()
        {
            //These parameters are shared in the same EffectPool.
            //Shared parameters use the "shared" keyword in the effect file
            //to indicate that they are shared between multiple effects.  In
            //the source code then, only one parameter is required for multiple
            //effects that share a parameter of the same type and name.
            worldParameter = effect_Fractal.Parameters["world"];
            viewParameter = effect_Fractal.Parameters["view"];
            projectionParameter = effect_Fractal.Parameters["projection"];
        }
      
        protected override void Update(GameTime gameTime)
        {            
            HandleInput(gameTime);

            base.Update(gameTime);
        }

        private void HandleInput(GameTime gameTime)
        {
            
            float dx = 0.0f;
            float dy = 0.0f;
            float dxMouse = 0.0f;
            float dyMouse = 0.0f;
            float dxZoom = 0.0f;
            float dxBracket = 0.0f;

            if (IsActive)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (lastMouseState.LeftButton == ButtonState.Released)
                    {
                        preClickMousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                    }

                    dxMouse = lastMouseState.X - Mouse.GetState().X;
                    dyMouse = lastMouseState.Y - Mouse.GetState().Y;
                    Mouse.SetPosition((int)(GraphicsDevice.Viewport.Width / 2.0f), (int)(GraphicsDevice.Viewport.Height / 2.0f));
                    IsMouseVisible = false;
                }
                else if (Mouse.GetState().LeftButton == ButtonState.Released &&
                         lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    Mouse.SetPosition((int)(GraphicsDevice.Viewport.Width / 2.0f), (int)(GraphicsDevice.Viewport.Height / 2.0f));                   
                    IsMouseVisible = true;
                }
                else if (Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    IsMouseVisible = true;
                }

                dxZoom = Mouse.GetState().ScrollWheelValue - lastMouseState.ScrollWheelValue;
                
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    dx -= 1.0f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    dx += 1.0f;
                }
                //apply mesh rotation to world matrix
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    dx -= 1.0f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    dx += 1.0f;
                }

                //apply mesh rotation to world matrix
                if (Keyboard.GetState().IsKeyDown(Keys.OemOpenBrackets))
                {
                    dxBracket -= 1.0f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.OemCloseBrackets))
                {
                    dxBracket += 1.0f;
                }

                UpdateFractalParams(gameTime, dx, dy, dxMouse, dyMouse, dxZoom, dxBracket);

                //Handle input for selecting the active effect
                if ((Keyboard.GetState().IsKeyDown(Keys.Space) && lastKeyboardState.IsKeyUp(Keys.Space)))
                {
                    currentFractalType++;

                    if (currentFractalType >= EFractalType.EFT_Max)
                    {
                        currentFractalType = EFractalType.EFT_Julia;
                    }
                }
            }
        }

        private void UpdateFractalParams(GameTime gameTime, float dx, float dy, float dxMouse, float dyMouse, float dxZoom, float dxBracket)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float prevZoom = effect_Fractal.Parameters["Zoom"].GetValueSingle();
            float newZoom = prevZoom;
            if (dxZoom > 0)
            {
                newZoom *= 0.9f;
            }
            else if (dxZoom < 0)
            {
                newZoom *= 1.1f;
            }

            effect_Fractal.Parameters["Zoom"].SetValue(newZoom);

            Vector2 prevSeed = effect_Fractal.Parameters["JuliaSeed"].GetValueVector2();
            effect_Fractal.Parameters["JuliaSeed"].SetValue(prevSeed + new Vector2(elapsedTime * dx * 0.01f, elapsedTime * dy * 0.01f));

            // Randomly move the seed around
            Console.WriteLine(gameTime.TotalGameTime.TotalSeconds);
            effect_Fractal.Parameters["JuliaSeed"].SetValue(new Vector2((float)Math.Cos(gameTime.TotalGameTime.TotalSeconds / 10.0f+1) * 0.005f + 0.39f, (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds / 20.0f+1) * 0.005f- 0.2f));

            Vector2 prevPan = effect_Fractal.Parameters["Pan"].GetValueVector2();
            Vector2 newPan = prevPan + new Vector2(-elapsedTime * dxMouse * 0.1f * (newZoom / 3), -elapsedTime * dyMouse * 0.1f * (newZoom / 3));            
            effect_Fractal.Parameters["Pan"].SetValue(newPan);     
     
            effect_Fractal.Parameters["Aspect"].SetValue(1/GraphicsDevice.Viewport.AspectRatio);

            float prevIterations = effect_Fractal.Parameters["Iterations"].GetValueInt32();
            effect_Fractal.Parameters["Iterations"].SetValue(prevIterations + dxBracket);

            effect_Fractal.Parameters["ViewportSize"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            effect_Fractal.Parameters["TextureSize"].SetValue(new Vector2(renderTexture.Width, renderTexture.Height));

            Vector3 newColorScale = effect_Fractal.Parameters["ColorScale"].GetValueVector3();
            float colorScaleRate = 0.5f;
            // Adjust the color scale
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
            {
                newColorScale.X += elapsedTime * colorScaleRate;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
            {
                newColorScale.Y += elapsedTime * colorScaleRate;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
            {
                newColorScale.Z += elapsedTime * colorScaleRate;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
            {
                newColorScale.X -= elapsedTime * colorScaleRate;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad5))
            {
                newColorScale.Y -= elapsedTime * colorScaleRate;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
            {
                newColorScale.Z -= elapsedTime * colorScaleRate;
            }

            effect_Fractal.Parameters["ColorScale"].SetValue(newColorScale);
        }

        /// <summary>
        /// Example 1.4
        /// 
        /// The effect parameters set in this function
        /// are shared between all of the rendered elements in the scene.
        /// </summary>
     
        void UpdateEffectParams()
        {     
            projectionParameter.SetValue(myCamera.ProjectionMatrix);
            viewParameter.SetValue(myCamera.ViewMatrix);
            worldParameter.SetValue(worldMatrix);    
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            UpdateEffectParams();
           
            DrawFractal();

            // Draw other components (which includes the bloom).
            base.Draw(gameTime);

            string fractalName = "";
            switch( currentFractalType )
            {
                case EFractalType.EFT_Julia:
                    fractalName = "Julia";
                    break;
                case EFractalType.EFT_Mandelbrot:
                    fractalName = "Mandelbrot";
                    break;
                case EFractalType.EFT_Poly:
                    fractalName = "Custom";
                    break;                    
            }                        

            Vector3 currentColorScale = effect_Fractal.Parameters["ColorScale"].GetValueVector3();
            DrawStringInternal(String.Format("Spacebar: Change Fractal [{0}] ",fractalName), new Vector2(50, 30));
            DrawStringInternal(String.Format("Right/Left Bracket: Change # iterations [{0:F}]",effect_Fractal.Parameters["Iterations"].GetValueSingle()), new Vector2(50,50));
            DrawStringInternal(String.Format("Up/Down/Left/Right: Change Seed [{0:F}]",effect_Fractal.Parameters["JuliaSeed"].GetValueVector2()),new Vector2(50, 70));
            DrawStringInternal("Left Mouse + Drag: Pan", new Vector2(50, 90));
            DrawStringInternal("Mousewheel: zoom", new Vector2(50, 110));
            DrawStringInternal(String.Format("1/2/3/4/5/6: Change Color [{0:F},{1:F},{2:F} ]", currentColorScale.X ,currentColorScale.Y ,currentColorScale.Z), new Vector2(50, 130));
            
        }

        void DrawFractal()
        {            
            if (currentFractalType == EFractalType.EFT_Julia)
            {
                effect_Fractal.CurrentTechnique = effect_Fractal.Techniques["Julia"];
            }
            else if( currentFractalType == EFractalType.EFT_Mandelbrot )
            {
                effect_Fractal.CurrentTechnique = effect_Fractal.Techniques["Mandelbrot"];
            }
            else 
            {
                effect_Fractal.CurrentTechnique = effect_Fractal.Techniques["Poly"];
            }
            spriteBatch.Begin();

            effect_Fractal.CurrentTechnique.Passes[0].Apply();             
            spriteBatch.Draw( renderTexture,new Vector2(0,0), Color.Red);
            spriteBatch.End();            
            
        }

    }
}
