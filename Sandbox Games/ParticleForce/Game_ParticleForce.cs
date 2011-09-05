using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
   
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game_ParticleForce : Game_Base
    {     
        public bool bLeftButtonPressed = false;
        public bool bRightButtonPressed = false;
        public bool bSpacebarPressed = false;
        public bool bAddingRepulsor = true;
        public float timeHeldKey = 0.0f;
        private ParticleModifier pendingMod = null;

        public Game_ParticleForce()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                bLeftButtonPressed = true;
            }

            if( Mouse.GetState().LeftButton == ButtonState.Released &&
                bLeftButtonPressed )
            {
                bLeftButtonPressed = false;
                ParticleGenerator partGen = new ParticleGenerator( this, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));                
                partGen.SpawnParticles(20);
                Components.Add(partGen);

                partGenList.Add(partGen);
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {                
                bRightButtonPressed = true;
                timeHeldKey += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (pendingMod == null)
                {
                    if (bAddingRepulsor)
                    {
                        pendingMod = new ParticleRepulsor(this, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                    }
                    else
                    {
                        pendingMod = new ParticleAttractor(this, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                    }
                    
                    Components.Add(pendingMod);
                    partModList.Add(pendingMod);
                }

                if (pendingMod != null)
                {
                    pendingMod.drawScale += (float)gameTime.ElapsedGameTime.TotalSeconds * 0.6f;                    
                }
                
            }
            
            if (Mouse.GetState().RightButton == ButtonState.Released &&
                bRightButtonPressed)
            {
                bRightButtonPressed = false;

                if (pendingMod != null)
                {
                    pendingMod.force = 50 + timeHeldKey * 300.0f;
                    pendingMod = null;
                }

                timeHeldKey = 0.0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                bSpacebarPressed = true;
            }

            if (!Keyboard.GetState().IsKeyDown(Keys.Space)&&
                bSpacebarPressed)
            {
                bSpacebarPressed = false;
                bAddingRepulsor = !bAddingRepulsor;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.DrawString(myFont, "Adding: " + (bAddingRepulsor ? "Repulsor" : "Attractor"), new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);
            spriteBatch.DrawString(myFont, "Spacebar: Toggle Repulsor/Attractor", new Vector2(40,40), Color.White);
            spriteBatch.DrawString(myFont, "Left Click: Spawn Particles", new Vector2(40, 70), Color.White);
            spriteBatch.DrawString(myFont, "Right Click: Spawn Repulsor/Attractor", new Vector2(40, 100), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
