using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Sandbox_Games.Tetris
{
    public class SpriteDepthComparer : IComparer<GameObject>
    {
        public int Compare(GameObject x, GameObject y)
        {
            if (x.SpriteDepth > y.SpriteDepth)
            {
                return -1;
            }
            else if( x.SpriteDepth == y.SpriteDepth )
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }

    public class State
    {
        public bool bPaused = false;
        private Game_TetrisAI myGame;

        public Game_TetrisAI MyGame
        {
            get { return myGame; }
            set { myGame = value; }
        }

        List<GameObject> myGameObjects = new List<GameObject>();

        public State(Game_TetrisAI inGame)
        {
            myGame = inGame;
        }

        public void AddObject(GameObject inObject)
        {            
            myGameObjects.Add(inObject);

            myGameObjects.Sort(new SpriteDepthComparer());
        }

        public void RemoveObject(GameObject inObject)
        {
            myGameObjects.Remove(inObject);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!bPaused)
            {
                for (int i = 0; i < myGameObjects.Count; i++)
                {
                    myGameObjects[i].Update(gameTime);
                }
            }
        }
        
        public virtual void Draw(GameTime gameTime)
        {
            for (int i = 0; i < myGameObjects.Count; i++)
            {
                myGameObjects[i].Draw(gameTime);
            }
        }
    }

    class State_TetrisMain : State
    {
        public TField mField;
        public TField mField2;
        Keys[] currentKeys;

        double lastKeyRepeatTime = 0.0;
        double keyHeldTime = 0.0;
        const double keyRepeatRate = 0.03;        

        public SpriteFont gameFont;
        ExplosionParticleSystem ps_explosion;
        ExplosionSmokeParticleSystem ps_smoke;

        // Our rotating sound
        public SoundEffect sound_Rotate;
        public SoundEffect sound_Drop;
        public SoundEffect sound_Clear;        
        public List<Song> sound_SongCollection;

        TBlock GetCurrentBlock()
        {
            return mField.mCurrentBlock;
        }
        TBlock GetPreviewBlock()
        {
            return mField.mPreviewBlock;
        }

        public State_TetrisMain(Game_TetrisAI inGame)
            : base(inGame)
        {

            gameFont = inGame.Content.Load<SpriteFont>("KootenayFont");

            mField = new TField(this, inGame, Color.White,new Vector2(50, 50),10,20);            
            mField.SpriteDepth = 1.0f;            
            mField.FullFieldEvent += new FullFieldHandler(FieldIsFull);
            mField.LineClearEvent += new LineClearHandler(LineCleared);
            mField.DropBlockEvent += new DropBlockHandler(BlockDropped);
            AddObject(mField);
            
            mField2 = new TField(this, inGame, Color.White, new Vector2(700, 50), 10, 20);
            mField2.SpriteDepth = 1.0f;
            mField2.FullFieldEvent += new FullFieldHandler(FieldIsFull);
            mField2.LineClearEvent += new LineClearHandler(LineCleared);
            mField2.DropBlockEvent += new DropBlockHandler(BlockDropped);        
            AddObject(mField2);

            mField2.mCurrentAI = new AI_Basic(mField2);
            
            inGame.AddLight(Color.White, new Vector2(200, 300));

            inGame.AddLight(Color.Red, new Vector2(100, 75));            
            inGame.AddLight(Color.Navy, new Vector2(200, 300));
            inGame.AddLight(Color.White, new Vector2(150, 400));
            inGame.AddLight(Color.Green, new Vector2(500, 200));
            inGame.AddLight(Color.CornflowerBlue, new Vector2(20, 200));
            inGame.AddLight(Color.DarkSalmon, new Vector2(300, 400));

            inGame.AddLight(Color.Yellow, new Vector2(750, 75));            
            inGame.AddLight(Color.LawnGreen, new Vector2(850, 300));
            inGame.AddLight(Color.White, new Vector2(800, 400));

            for (int i = 0; i <= 4; i++)
            {
                inGame.AddLight(Color.White, new Vector2(50 + (inGame.graphics.PreferredBackBufferWidth * i) / 4, inGame.graphics.PreferredBackBufferHeight));
            }
            // create the particle systems and add them to the components list.
            // we should never see more than one explosion at once
            ps_explosion = new ExplosionParticleSystem(inGame, 1);
            inGame.Components.Add(ps_explosion);

            ps_smoke = new ExplosionSmokeParticleSystem(inGame, 1);
            inGame.Components.Add(ps_smoke);

            // Load sounds
            sound_Rotate = inGame.Content.Load<SoundEffect>("Audio\\Tetris\\Tetris_Rotate");
            sound_Drop = inGame.Content.Load<SoundEffect>("Audio\\Tetris\\Tetris_Drop");
            sound_Clear = inGame.Content.Load<SoundEffect>("Audio\\Tetris\\Tetris_Clear");
            sound_SongCollection = new List<Song>();
            sound_SongCollection.Add(inGame.Content.Load<Song>("Audio\\Tetris\\Tetris_MainTheme"));
            sound_SongCollection.Add(inGame.Content.Load<Song>("Audio\\Tetris\\Tetris_MainTheme2"));
            sound_SongCollection.Add(inGame.Content.Load<Song>("Audio\\Tetris\\Tetris_MainTheme3"));
            MediaPlayer.Play(GetRandomSong());
        }

        private Song GetRandomSong()
        {
            return sound_SongCollection[MyGame.randomizer.Next(0, sound_SongCollection.Count)];    
        }

        private void FieldIsFull(object sender, EventArgs e)
        {            
            MyGame.ResetGame();
        }

        private void LineCleared(object sender, LineClearEventArgs e)
        {

            if (sender == mField)
            {
                mField2.AddRows(e.numLines - 1);
            }
            else
            {
                mField.AddRows(e.numLines - 1);
            }

            
            sound_Clear.Play();
        }

        private void BlockDropped(object sender, EventArgs e)
        {
            sound_Drop.Play();
        }

        public void UpdateInput(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();

            Keys[] newKeys = (Keys[])kbState.GetPressedKeys();

            if (newKeys.GetLength(0) == 0)
            {
                keyHeldTime = 0.0;
                lastKeyRepeatTime = 0.0;
            }
            else if (keyHeldTime < 0.3)
            {
                keyHeldTime += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                lastKeyRepeatTime += gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (currentKeys != null)
            {
                foreach (Keys k in newKeys)
                {
                    bool bFound = false;

                    foreach (Keys k2 in currentKeys)
                    {
                        if (k == k2)
                        {
                            bFound = true;
                            break;
                        }
                    }

                    if (!bFound)
                    {
                        OnKeyPressed(k, gameTime);
                    }
                    else if (lastKeyRepeatTime >= keyRepeatRate)
                    {
                        lastKeyRepeatTime = 0.0;
                        OnKeyRepeat(k, gameTime);
                    }
                }

            }

            currentKeys = newKeys;
        }

        // Called from repeat or press
        void HandleKeyPress(Keys k, GameTime gameTime)
        {
            if (k == Keys.Right)
            {
                if (!mField.CheckCollision(GetCurrentBlock(), 1, 0))
                {
                    GetCurrentBlock().Move(1, 0);
                }
            }
            if (k == Keys.Left)
            {
                if (!mField.CheckCollision(GetCurrentBlock(), -1, 0))
                {
                    GetCurrentBlock().Move(-1, 0);
                }
            }
            if (k == Keys.Down)
            {
                float moveAmount = GetCurrentBlock().brickDropSpeed * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                GetCurrentBlock().Move(0, moveAmount);

                // if we're colliding, then correct the piece movement
                if (mField.CheckCollision(GetCurrentBlock(), 0, 1))
                {
                    // Correct the piece movement now
                    GetCurrentBlock().Move(0, -moveAmount);
                }                
            }
            // Toggles AI on/Off
            if (k == Keys.A)
            {
                if (mField.mCurrentAI == null)
                {
                    mField.mCurrentAI = new AI_Basic(mField);
                }
                else
                {
                    mField.mCurrentAI = null;
                }
            }
            if (k == Keys.P)
            {
                bPaused = !bPaused;
            }
            if (k == Keys.D)
            {
                mField.AddRows(2);
            }
            if (k == Keys.R)
            {
                FieldIsFull(null, EventArgs.Empty);
            }
            if (k == Keys.Enter)
            {
                mField.DropBlock();
            }
        }
        void OnKeyRepeat(Keys k, GameTime gameTime)
        {
            HandleKeyPress(k, gameTime);
        }

        void OnKeyPressed(Keys k, GameTime gameTime)
        {
            HandleKeyPress(k, gameTime);

            if (k == Keys.Up)
            {
                if (mField.TryRotate(GetCurrentBlock(), 1, 0) ||
                    mField.TryRotate(GetCurrentBlock(), -1, 0))
                {
                    sound_Rotate.Play();
                }
            }
            if (k == Keys.Space)
            {
                if (mField.mCurrentAI != null)
                {
                    mField.mCurrentAI.updateTimer = 1000;
                }
                else
                {
                    while (!mField.CheckCollision(GetCurrentBlock(), 0, 1))
                    {
                        GetCurrentBlock().Move(0, 1);
                    }

                    mField.DropBlock();
                    
                }
            }
            if (k == Keys.Z)
            {
                // create the particle systems and add them to the components list.
                // we should never see more than one explosion at once                
                ps_explosion.AddParticles();                                
                ps_smoke.AddParticles();

            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateInput(gameTime);

            //ps_explosion.AddParticles();
            //ps_smoke.AddParticles();

            //ps_explosion.position = new Vector2(Mouse.GetState().X,Mouse.GetState().Y);
            //ps_smoke.position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            //ps_explosion.position = GetCurrentBlock().GetWorldPos();
            //ps_smoke.position = GetCurrentBlock().GetWorldPos();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Vector2 previewBlockPos = new Vector2(mField.mPreviewBlock.GetBrickPos(1, 1).X, mField.mPreviewBlock.GetBrickPos(1, 1).Y);

            DrawStringInternal("Score " + mField.score, previewBlockPos + new Vector2(-25, 100));
            DrawStringInternal("Level " + mField.level, previewBlockPos + new Vector2(-25, 125));
            DrawStringInternal("Lines Remaining " + mField.linesRemainingOnLevel, previewBlockPos + new Vector2(-25, 150));
            DrawStringInternal("Lines Cleared " + mField.totalLinesCleared, previewBlockPos + new Vector2(-25, 175));
            DrawStringInternal("==========================", previewBlockPos + new Vector2(-25, 200));
            DrawStringInternal("[A]: Toggle AI", previewBlockPos + new Vector2(-25, 225));
            DrawStringInternal("[P]: Pause", previewBlockPos + new Vector2(-25, 250));
            DrawStringInternal("[R]: Restart", previewBlockPos + new Vector2(-25, 275));
            DrawStringInternal("[M]: Mute", previewBlockPos + new Vector2(-25, 300));

        }

        private void DrawStringInternal(string inString, Vector2 inPosition)
        {            
            MyGame.SpriteBatch.DrawString(gameFont, inString, inPosition - new Vector2(-1, -1), Color.Black);
            MyGame.SpriteBatch.DrawString(gameFont, inString, inPosition, Color.White);         
        }
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game_TetrisAI : Game_Base
    {        
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }

        State currentState;

        public Game_TetrisAI()
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
            // TODO: use this.Content to load your game content here
            ResetGame();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);            

            base.LoadContent();
        }

        public void ResetGame()
        {
            ClearLights();
            currentState = new State_TetrisMain(this);            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            currentState.Update(gameTime);
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void DrawScene(GameTime gameTime)
        {
            base.DrawScene(gameTime);            
            currentState.Draw(gameTime);        
        }

        public void SetCurrentState(State newState)
        {
            currentState = newState;            
        }
    }
}
