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
    class Game_Lighting : Game_Base
    {
        /// <summary>
        /// Shader variables
        /// </summary>
        Effect effect_Unlit;
        Effect effect_VertexLit;
        Effect effect_PerPixelLit;

        /// <summary>
        /// Example 1.1: Effect objects used for this example
        /// </summary>
        #region Effect Fields
        private EffectParameter projectionParameter;
        private EffectParameter viewParameter;
        private EffectParameter worldParameter;
        private EffectParameter lightColorParameter;
        private EffectParameter lightDirectionParameter;
        private EffectParameter ambientColorParameter;
        #endregion

        /// <summary>
        /// Example 1.2: Data fields corresponding to the effect paramters
        /// </summary>
        #region Uniform Data Fields
        private Matrix worldMatrix;
        private Vector3 diffuseLightDirection;
        private Vector4 diffuseLightColor;
        private Vector4 ambientLightColor;
        #endregion

        private SampleGrid grid;
                
        private Camera_Orbit myCamera;
        private bool bEnableAdvancedEffect = true;
        private Model testModel;
        private Vector2 preClickMousePosition = Vector2.Zero;

        protected override void LoadContent()
        {
            base.LoadContent();

            effect_Unlit = Content.Load<Effect>("Shaders\\BasicShader");
            effect_VertexLit = Content.Load<Effect>("Shaders\\VertexLighting");
            effect_PerPixelLit = Content.Load<Effect>("Shaders\\PerPixelLighting");
            testModel = Content.Load<Model>("Meshes\\WeirdObject");

            SetUpGrid();
            SetUpEffectParams();
            SetUpCamera();
            SetUpLights();
       }

        private void SetUpGrid()
        {
            //Set up the reference grid and sample camera
            grid = new SampleGrid();
            grid.GridColor = Color.LimeGreen;
            grid.GridScale = 1.0f;
            grid.GridSize = 32;
            grid.LoadContent(graphics.GraphicsDevice);
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
            worldParameter = effect_Unlit.Parameters["world"];
            viewParameter = effect_Unlit.Parameters["view"];
            projectionParameter = effect_Unlit.Parameters["projection"];


            //These effect parameters are only used by vertexLightingEffect
            //to indicate the lights' colors and direction
            lightColorParameter = effect_VertexLit.Parameters["lightColor"];
            lightDirectionParameter = effect_VertexLit.Parameters["lightDirection"];
            ambientColorParameter = effect_VertexLit.Parameters["ambientColor"];
        }

        protected void SetUpCamera()
        {
            myCamera = new Camera_Orbit(this, GraphicsDevice.Viewport, SampleArcBallCameraMode.RollConstrained);
            myCamera.Distance = 3;
            //orbit the camera so we're looking down the z=-1 axis
            //the acr-ball camera is traditionally oriented to look
            //at the "front" of an object
            myCamera.OrbitRight(MathHelper.Pi);
            //orbit up a bit for perspective
            myCamera.OrbitUp(.2f);

            Components.Add(myCamera);

            // create the default world matrix
            worldMatrix = Matrix.Identity;

            //grid requires a projection matrix to draw correctly
            grid.ProjectionMatrix = myCamera.ProjectionMatrix;

            //Set the grid to draw on the x/z plane around the origin
            grid.WorldMatrix = Matrix.Identity;
        }

        private void SetUpLights()
        {

            //Set the light direction to a fixed value.
            //This will place the light source behind, to the right, and above the user.
            diffuseLightDirection = new Vector3(-1, -1, -1);

            //ensure the light direction is normalized, or
            //the shader will give some weird results
            diffuseLightDirection.Normalize();

            //set the color of the diffuse light
            diffuseLightColor = Color.CornflowerBlue.ToVector4();

            //set the ambient lighting color
            ambientLightColor = Color.DarkSlateGray.ToVector4();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleInput(gameTime);
        }


        private void HandleInput(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float dx = 0.0f;
            float dy = 0.0f;
            float dxMouse = 0.0f;
            float dyMouse = 0.0f;
            float dxZoom = 0.0f;
            float dxBracket = 0.0f;

            myCamera.HandleDefaultKeyboardControls(
                Keyboard.GetState(), gameTime);

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
                    dy -= 1.0f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    dy += 1.0f;
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

                //apply mesh rotation to world matrix
                if (dx != 0)
                {
                    worldMatrix = worldMatrix * Matrix.CreateFromAxisAngle(myCamera.Up,
                        elapsedTime * dx);
                }
                if (dy != 0)
                {
                    worldMatrix = worldMatrix * Matrix.CreateFromAxisAngle(myCamera.Right,
                        elapsedTime * -dy);
                }

                //Vector4 prevLightDirection = effect_PerPixelLit.Parameters["lightDirection"].GetValueVector4();
                //Vector4 newPan = prevLightDirection + new Vector4(-elapsedTime * dx * 0.1f, -elapsedTime * dy * 0.1f,0.0f,0.0f);
                //effect_PerPixelLit.Parameters["lightDirection"].SetValue(newPan);     
     

                //Handle input for selecting the active effect
                if ((Keyboard.GetState().IsKeyDown(Keys.Space) && lastKeyboardState.IsKeyUp(Keys.Space)))
                {
                    //toggle the advanced effect
                    bEnableAdvancedEffect = !bEnableAdvancedEffect;
                }
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            graphics.GraphicsDevice.Clear(Color.Black);

            //always set the shared effects parameters
            SetSharedEffectParameters();

            //draw the reference grid so it's easier to get our bearings
            DrawGrid();

            // Draw our test model
            DrawMesh(testModel);
        }

        /// <summary>
        /// Example 1.4
        /// 
        /// The effect parameters set in this function
        /// are shared between all of the rendered elements in the scene.
        /// </summary>
        private void SetSharedEffectParameters()
        {
            projectionParameter.SetValue(myCamera.ProjectionMatrix);
            viewParameter.SetValue(myCamera.ViewMatrix);
            worldParameter.SetValue(worldMatrix);
            effect_PerPixelLit.Parameters["vEye"].SetValue(myCamera.Position.X);
        }

        public void DrawGrid()
        {
            //the reference grid requires a view matrix to draw correctly
            grid.ViewMatrix = myCamera.ViewMatrix;

            grid.Draw();
        }

        public void DrawMesh(Model inModel)
        {
            if (inModel == null)
                return;

            //our sample meshes only contain a single part, so we don't need to bother
            //looping over the ModelMesh and ModelMeshPart collections. If the meshes
            //were more complex, we would repeat all the following code for each part
            ModelMesh mesh = inModel.Meshes[0];
            ModelMeshPart meshPart = mesh.MeshParts[0];            
            graphics.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);

            //figure out which effect we're using currently
            Effect effect;
            if (bEnableAdvancedEffect)
            {
                effect = effect_PerPixelLit;
            }
            else
            {
                effect = effect_PerPixelLit;
            }


            //at this point' we're ready to begin drawing
            //To start using any effect, you must call Effect.Begin
            //to start using the current technique (set in LoadGraphicsContent)

            //now we loop through the passes in the teqnique, drawing each
            //one in order
            for (int i = 0; i < effect.CurrentTechnique.Passes.Count; i++)
            {                
                effect.Parameters["world"].SetValue(worldMatrix * Matrix.CreateScale(0.1f));
                //EffectPass.Begin will update the device to
                //begin using the state information defined in the current pass
                effect.CurrentTechnique.Passes[i].Apply();

                //inModel contains all of the information required to draw
                //the current mesh
                graphics.GraphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList, 0, 0,
                    meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
            }
        }
    }
}
