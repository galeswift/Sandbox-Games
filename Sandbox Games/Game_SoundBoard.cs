using System;
using System.IO;
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
    public class Game_SoundBoard : Game_Base
    {     
        public bool bLeftButtonPressed = false;
        public bool bRightButtonPressed = false;
        public bool bSpacebarPressed = false;        
        public SoundBoard mySoundBoard;
        int currentBank = 1;
         
        public Game_SoundBoard()
        {
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            mySoundBoard = new SoundBoard(this);
            mySoundBoard.LoadSoundLibrary("Bank1");
            Components.Add(mySoundBoard);           
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
                bLeftButtonPressed)
            {
                bLeftButtonPressed = false;
                // Play the sound board at the world position.
                mySoundBoard.PlaySoundNode(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                bSpacebarPressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Space)&&
                bSpacebarPressed)
            {
                bSpacebarPressed = false;
                currentBank++;

                if( currentBank > 3 )
                {
                    currentBank = 1;
                }

                mySoundBoard.LoadSoundLibrary("Bank" + currentBank);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                mySoundBoard.AddBPM((float)gameTime.ElapsedGameTime.TotalSeconds * 50);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                mySoundBoard.AddBPM((float)-gameTime.ElapsedGameTime.TotalSeconds * 50);
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
        
            base.Draw(gameTime);
        }
    }
}
