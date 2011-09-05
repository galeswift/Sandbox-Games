using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Sandbox_Games.Shared;

namespace Sandbox_Games.Tetris
{
    public class LineClearEventArgs : EventArgs
    {
        public int numLines;

        public LineClearEventArgs(int inNumLines)
        {
            numLines = inNumLines;
        }
    }

    // Called whenever we could not place our block
    public delegate void FullFieldHandler(object sender, EventArgs e);

    // Called whenever we clear a line
    public delegate void LineClearHandler(object sender, LineClearEventArgs e);

    // Called whenever we drop a block
    public delegate void DropBlockHandler(object sender, EventArgs e);

    // Called whenever an animation has finished playing for an object
    public delegate void FlxAnimationCallback(string Name, uint Frame, int FrameIndex);

    public class GameObject
    {
        /**
        * Whether the current animation has finished its first (or only) loop.
        */
        public bool bAnimFinished;

        // Whether or not this sprite has animation
        public bool bHasAnimations;

        /**
         * The width of the actual graphic or image being displayed (not necessarily the game object/bounding box).
         * NOTE: Edit at your own risk!!  This is intended to be read-only.
         */
        public int frameWidth;
        /**
         * The height of the actual graphic or image being displayed (not necessarily the game object/bounding box).
         * NOTE: Edit at your own risk!!  This is intended to be read-only.
         */
        public int frameHeight;
        /**
         * The total number of frames in this image (assumes each row is full).
         */
        public int frames;

        private List<SB_Anim> animations;
        //private uint flipped;
        protected SB_Anim curAnim;
        protected int curFrame;
        protected int currentAnimFrame;
        private float frameTimer;
        private FlxAnimationCallback animFinishedCallback;
        protected Rectangle animatedRect;

        public List<SB_Anim> Animations
        {
            get { return animations; }
        }

        // Depth from 0-1.  0 = in front, 1 = in the back
        public float SpriteDepth { get; set; }
        private Game_TetrisAI myGame;

        public Game_TetrisAI MyGame
        {
            get { return myGame; }
            set { myGame = value; }
        }

        private float mySize;

        public float Size
        {
            get { return mySize; }
            set { mySize = value; }
        }

        private Vector2 myPosition;
        public Vector2 Position
        {
            get { return myPosition; }
            set { myPosition = value; }
        }

        private Texture2D myTexture;

        public Texture2D Texture
        {
            get { return myTexture; }
            set { myTexture = value; }
        }

        private string myFilePath;

        public string FilePath
        {
            get { return myFilePath; }
            set { myFilePath = value; }
        }

        private Color myColor;

        public Color MyColor
        {
            get { return myColor; }
            set { myColor = value; }
        }
        private Vector2 myVelocity;


        public Vector2 Velocity
        {
            get { return myVelocity; }
            set { myVelocity = value; }
        }

        public GameObject(Game_TetrisAI inGame, Color inColor, string inFilePath = "sprites\\noImage", bool bIsAnimated = false, int inFrameWidth = 0, int inFrameHeight = 0)
        {
            myGame = inGame;
            FilePath = inFilePath;
            myColor = inColor;
            mySize = 32;

            LoadTexture(FilePath);

            if (bIsAnimated)
            {                
                InitAnimData(inFrameWidth, inFrameHeight);
            }
        }

        private void InitAnimData(int inFrameWidth, int inFrameHeight)
        {
            bHasAnimations = true;
            bAnimFinished = false;
            animations = new List<SB_Anim>();
            curAnim = null;
            curFrame = 0;
            currentAnimFrame = 0;
            frameTimer = 0;
            animFinishedCallback = null;
                        
            frameWidth = inFrameWidth;
            frameHeight = inFrameHeight;


            ResetAnimHelpers();
        }

        private void ResetAnimHelpers()
        {
            Rectangle texRect;
            texRect.X = 0;
            texRect.Y = 0;
            animatedRect.X = 0;
            animatedRect.Y = 0;
            animatedRect.Width = frameWidth;
            animatedRect.Height = frameHeight;

            texRect.Width = myTexture.Width;
            texRect.Height = myTexture.Height;
                      
            frames = (texRect.Width / animatedRect.Width) * (texRect.Height / animatedRect.Height);

            currentAnimFrame = 0;
        }

        public virtual void LoadTexture(string inFilePath)
        {
            myTexture = MyGame.Content.Load<Texture2D>(FilePath);
        }
        public virtual void Draw(GameTime gameTime)
        {
            myGame.SpriteBatch.Draw(myTexture, GetRectangle(), myColor);
        }

        public virtual Rectangle GetRectangle()
        {
            if (bHasAnimations)
            {
                return animatedRect;
            }
            else
            {
                return new Rectangle((int)myPosition.X, (int)myPosition.Y, (int)mySize, (int)mySize);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            Vector2 newPosition = Position + myVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position = newPosition;
            UpdateAnimation(gameTime);
        }


        /**
         * Internal function for updating the sprite's animation.
         * Useful for cases when you need to update this but are buried down in too many supers.
         * This function is called automatically by <code>FlxSprite.update()</code>.
         */
        protected void UpdateAnimation(GameTime gameTime)
        {
            if ((curAnim != null) && (curAnim.delay > 0) && (curAnim.looped || !bAnimFinished))
            {
                frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                while (frameTimer > curAnim.delay)
                {
                    frameTimer = frameTimer - curAnim.delay;
                    if (curFrame == curAnim.frames.Length - 1)
                    {
                        if (curAnim.looped)
                        {
                            curFrame = 0;
                        }
                        bAnimFinished = true;
                    }
                    else
                    {
                        bAnimFinished = false;
                        curFrame++;
                    }
                    currentAnimFrame = curAnim.frames[curFrame];
                    CalcFrame();
                }
            }
        }

        /**
         * Internal function to update the current animation frame.
         */
        protected void CalcFrame()
        {
            uint rx = (uint)(currentAnimFrame * frameWidth);
            uint ry = 0;

            //Handle sprite sheets
            uint w = (uint)myTexture.Width;
            if (rx >= w)
            {
                ry = (uint)(rx / w) * (uint)frameHeight;
                rx %= w;
            }

            animatedRect = new Rectangle((int)rx + (int)myPosition.X, (int)ry + (int)myPosition.Y, frameWidth, frameHeight);

            if (animFinishedCallback != null && curAnim != null && bAnimFinished)
            {
                animFinishedCallback(curAnim.name, (uint)curFrame, currentAnimFrame);
            }

        }

        /**
         * Adds a new animation to the sprite.
         * 
         * @param	Name		What this animation should be called (e.g. "run").
         * @param	Frames		An array of numbers indicating what frames to play in what order (e.g. 1, 2, 3).
         * @param	FrameRate	The speed in frames per second that the animation should play at (e.g. 40 fps).
         * @param	Looped		Whether or not the animation is looped or just plays once.
         */
        public void AddAnimation(string Name, int[] Frames)
        {
            try
            {
                animations.Add(new SB_Anim(Name, Frames, 0, true));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} This object is not an animated object!",e.Message);
            }
        }
        public void AddAnimation(string Name, int[] Frames, int FrameRate)
        {
            try
            {
                animations.Add(new SB_Anim(Name, Frames, FrameRate, true));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} This object is not an animated object!", e.Message);
            }
        }
        public void AddAnimation(string Name, int[] Frames, int FrameRate, bool Looped)
        {
            try
            {
                animations.Add(new SB_Anim(Name, Frames, FrameRate, Looped));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} This object is not an animated object!", e.Message);
            }
        }

        /**
         * Pass in a function to be called whenever this sprite's animation changes.
         * 
         * @param	AnimationCallback		A function that has 3 parameters: a string name, a uint frame number, and a uint frame index.
         */
        public void AddAnimationCallback(FlxAnimationCallback ac)
        {
            animFinishedCallback = ac;
        }
    }

    struct GridSlot
    {
        public bool bFilled;
        public Color gridColor;
    }

    class TBlock : GameObject
    {
        public new bool[,] Size;

        // Made the x and y positions floats so that we have smooth movement
        private float xGridPos;
        public int XGridPos
        {
            get { return (int)(xGridPos); }
            set { xGridPos = value; }
        }

        private float yGridPos;
        public int YGridPos
        {
            get { return (int)(yGridPos); }
            set { yGridPos = value; }
        }

        // How big each of the invidiual bricks are
        public static int BrickSize = 20;
        public TField myField;

        // if true, then we show a faded out version of our block on the field
        public bool bShowGhost = true;

        // how many total bricks make up this block
        public const int DIMENSIONS = 4;
        public bool bPlaced = false;

        // How many grid slots per second we drop this brick.  Can change to increase the speed based on level, etc.
        public float brickDropSpeed = 2;

        public TBlock(TField inField, Game_TetrisAI inGame, Color inColor)
            :
            base(inGame, inColor, "Sprites\\Square")
        {
            myField = inField;
            Size = new bool[DIMENSIONS, DIMENSIONS];
            for (int i = 0; i < DIMENSIONS; i++)
            {
                for (int j = 0; j < DIMENSIONS; j++)
                {
                    Size[i, j] = false;
                }
            }
        }

        public void Reset()
        {
            Position = new Vector2(0, 0);
            bShowGhost = true;
            xGridPos = 3;
            yGridPos = 0;
        }

        public void Move(float X, float Y)
        {
            xGridPos += X;
            yGridPos += Y;
        }

        public override void Draw(GameTime gameTime)
        {
            DrawBlock(MyColor);

            if (bShowGhost)
            {
                int moveCounter = 0;
                while (!myField.CheckCollision(this, 0, 1))
                {
                    moveCounter++;
                    Move(0, 1);
                }
                Color ghostColor = MyColor;
                ghostColor.A = 50;
                DrawBlock(ghostColor, true);

                while (moveCounter > 0)
                {
                    moveCounter--;
                    Move(0, -1);
                }
            }
        }

        private void DrawBlock(Color inColor, bool bIsGhost = false)
        {
            for (int i = 0; i < DIMENSIONS; i++)
            {
                for (int j = 0; j < DIMENSIONS; j++)
                {
                    if (Size[i, j] == true)
                    {
                        MyGame.SpriteBatch.Draw(Texture, GetBrickPos(i, j, bIsGhost), inColor);
                    }
                }
            }
        }

        // returns the world position of a specific brick of a piece.  (There are 4 bricks in each piece)
        public Rectangle GetBrickPos(int X, int Y, bool bGetSnappedPos = false)
        {
            Vector2 myWorldPos = Position + myField.GetWorldPosition(X + (bGetSnappedPos ? XGridPos : xGridPos), Y + (bGetSnappedPos ? YGridPos : yGridPos));

            return new Rectangle((int)myWorldPos.X, (int)myWorldPos.Y, (int)BrickSize, (int)BrickSize);
        }

        public Vector2 GetWorldPos()
        {
            return Position + myField.GetWorldPosition(XGridPos, YGridPos);
        }

        public void SnapBlockToGrid()
        {
            // removes any floating point component of the grid positions.
            xGridPos = (float)Math.Truncate(xGridPos);
            yGridPos = (float)Math.Truncate(yGridPos);
        }
    }

    class TField : GameObject
    {
        public int GRIDWIDTH;
        public int GRIDHEIGHT;

        public TBlock mCurrentBlock;
        public TBlock mPreviewBlock;

        // if not null, this AI will control the movement of the blocks in the game 
        public AI_Basic mCurrentAI = null;

        // The state we're currently living in
        public State mState = null;

        // How fast blocks drop in the field.
        double dropRate = 1.0;

        // Where the preview block is spawned
        public Vector2 previewBlockPosition;

        // Current level for this field
        public int level = 1;

        // Current score for this level
        public int score = 0;

        // Total lines cleared for this level
        public int totalLinesCleared = 0;

        // How many more lines before we level up
        public int linesRemainingOnLevel = 10;

        // The texture used for an active grid slot
        public Texture2D gridSlotTexture;

        // The grid slots for our entire field
        public GridSlot[,] GridMap;

        // A temporary grid slot used by the AI
        public GridSlot[,] tempGridMap;

        // General randomizer for this field
        Random mRandomizer = new Random();

        // How much time the player has to move their block after it's been dropped.  Set to the droprate once we detect a collision
        double timeLeftToMoveAfterDrop = 0.0;

        // Border image for our field texture
        public Texture2D borderTexture;

        // Border image for our filled border texture
        public Texture2D borderTextureFilled;

        // Constructor
        public TField(State inState, Game_TetrisAI inGame, Color inColor, Vector2 inPosition, int gridWidth = 10, int gridHeight = 20)
            : base(inGame, inColor, "Sprites\\field")
        {
            GRIDWIDTH = gridWidth;
            GRIDHEIGHT = gridHeight;

            GridMap = new GridSlot[GRIDWIDTH, GRIDHEIGHT];

            mState = inState;

            mCurrentBlock = GetRandomBlock();
            mCurrentBlock.Reset();
            mState.AddObject(mCurrentBlock);
            Position = inPosition;

            ResetPreviewBlock();

            MyColor = new Color(255, 255, 255, 50);
            gridSlotTexture = MyGame.Content.Load<Texture2D>("Sprites\\Square");
            borderTexture = MyGame.Content.Load<Texture2D>("Sprites\\fieldBorder");
            borderTextureFilled = MyGame.Content.Load<Texture2D>("Sprites\\fieldBorderFilled");
        }

        private void ResetPreviewBlock()
        {
            previewBlockPosition = new Vector2(Position.X, 0) + new Vector2(GRIDWIDTH * TBlock.BrickSize, 0);

            mPreviewBlock = GetRandomBlock();
            mPreviewBlock.bShowGhost = false;
            mPreviewBlock.Position = previewBlockPosition;
            mState.AddObject(mPreviewBlock);
        }

        public event FullFieldHandler FullFieldEvent;
        public event LineClearHandler LineClearEvent;
        public event DropBlockHandler DropBlockEvent;

        // Keeps trying to rotate + move the block until a suitable position is found.
        // Allows rotating near an edge
        public bool TryRotate(TBlock inBlock, int xDir, int tryCount)
        {
            // Magic # of moves to try before aborting
            if (tryCount > 3)
            {
                return false;
            }
            else if (!RotateBlock(inBlock, true))
            {
                inBlock.Move(xDir, 0);
                bool bTryResult = TryRotate(inBlock, xDir, ++tryCount);

                if (!bTryResult)
                {
                    inBlock.Move(-xDir, 0);
                }

                return bTryResult;
            }
            else
            {
                return true;
            }
        }

        // Rotates a block around a pivot point.
        public bool RotateBlock(TBlock inBlock, bool bRotateRight)
        {
            bool result = true;

            bool[,] newSize = new bool[TBlock.DIMENSIONS, TBlock.DIMENSIONS];
            bool[,] oldSize = inBlock.Size;

            for (int i = 0; i < TBlock.DIMENSIONS; i++)
            {
                for (int j = 0; j < TBlock.DIMENSIONS; j++)
                {
                    if (bRotateRight)
                    {
                        newSize[3 - j, i] = inBlock.Size[i, j];
                    }
                    else
                    {
                        newSize[j, 3 - i] = inBlock.Size[i, j];
                    }
                }
            }

            inBlock.Size = newSize;

            if (CheckCollision(inBlock, 0, 0))
            {
                inBlock.Size = oldSize;
                result = false;
            }
            else
            {
                inBlock.SnapBlockToGrid();
            }

            return result;
        }

        // Add rows filled with random junk from the bottom
        public void AddRows(int count)
        {
            if (count <= 0)
            {
                return;
            }

            bool bLostGame = false;
            // see if this makes us lose
            for (int x = 0; x < GRIDWIDTH; x++)
            {
                for (int y = 0; y < count; y++)
                {
                    if (GridMap[x, y].bFilled)
                    {
                        bLostGame = true;
                        break;
                    }
                }

                if (bLostGame)
                {
                    break;
                }
            }

            if (bLostGame)
            {
                if (FullFieldEvent != null)
                {
                    FullFieldEvent(this, EventArgs.Empty);
                }
            }
            else
            {
                for (int x = 0; x < GRIDWIDTH; x++)
                {
                    for (int y = 0; y < GRIDHEIGHT - count; y++)
                    {
                        // Move everything up
                        GridMap[x, y] = GridMap[x, y + count];
                    }
                }

                for (int y = GRIDHEIGHT - count; y < GRIDHEIGHT; y++)
                {
                    int randHole = MyGame.randomizer.Next(GRIDWIDTH);
                    for (int x = 0; x < GRIDWIDTH; x++)
                    {
                        GridMap[x, y].bFilled = false;

                        if (x != randHole)
                        {
                            GridMap[x, y].bFilled = true;
                            GridMap[x, y].gridColor = Color.DarkGray;
                        }
                    }
                }
            }
        }

        public void RemoveRow(int row, bool bTest)
        {
            if (!bTest)
            {
                linesRemainingOnLevel--;
                totalLinesCleared++;
                if (linesRemainingOnLevel <= 0)
                {
                    ChangeLevel(1);
                }
            }

            for (int x = 0; x < GRIDWIDTH; x++)
            {
                for (int y = row; y > 0; y--)
                {
                    GridMap[x, y] = GridMap[x, y - 1];
                }
            }
        }

        void ChangeLevel(int amount)
        {
            dropRate = Math.Max(0.05, dropRate - 0.1);
            level += amount;
            linesRemainingOnLevel = 10;
        }

        // Gets the number of edges touching the block
        public int GetNumTouchingEdges(TBlock inBlock)
        {
            int result = 0;
            int curX = inBlock.XGridPos;
            int curY = inBlock.YGridPos;
            if (inBlock != null)
            {
                for (int i = 0; i < TBlock.DIMENSIONS; i++)
                {
                    for (int j = 0; j < TBlock.DIMENSIONS; j++)
                    {
                        if (inBlock.Size[i, j] == true)
                        {
                            if (curX + i <= 0 ||
                                curX + i >= GRIDWIDTH - 1)
                            {
                                result++;
                            }

                            if (curY + j >= GRIDHEIGHT - 1)
                            {
                                result++;
                            }

                            // Check on all four sides, and see if any edges touch
                            if ((i + curX - 1) >= 0 &&
                                (i + curX - 1) < GRIDWIDTH - 1 &&
                                GridMap[i + curX - 1, j + curY].bFilled == true)
                            {
                                result++;
                            }

                            // Check on all four sides, and see if any edges touch
                            if ((i + curX + 1) >= 0 &&
                                (i + curX + 1) <= GRIDWIDTH - 1 &&
                                GridMap[i + curX + 1, j + curY].bFilled == true)
                            {
                                result++;
                            }

                            // Check on all four sides, and see if any edges touch
                            if ((j + curY - 1) >= 0 &&
                                (j + curY - 1) <= GRIDHEIGHT - 1 &&
                                GridMap[i + curX, j + curY - 1].bFilled == true)
                            {
                                result++;
                            }

                            // Check on all four sides, and see if any edges touch
                            if ((j + curY + 1) >= 0 &&
                                (j + curY + 1) <= GRIDHEIGHT - 1 &&
                                GridMap[i + curX, j + curY + 1].bFilled == true)
                            {
                                result++;
                            }
                        }
                    }
                }
            }

            return result;
        }

        // Returns true if collision detected
        public bool CheckCollision(TBlock inBlock, int X, int Y)
        {
            int newX = inBlock.XGridPos + X;
            int newY = inBlock.YGridPos + Y;

            // Check for collision against the walls
            if (inBlock != null)
            {
                for (int i = 0; i < TBlock.DIMENSIONS; i++)
                {
                    for (int j = 0; j < TBlock.DIMENSIONS; j++)
                    {
                        if (inBlock.Size[i, j] == true)
                        {
                            if (newX + i < 0 ||
                                newX + i > GRIDWIDTH - 1 ||
                                newY + j < 0 ||
                                newY + j > GRIDHEIGHT - 1)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            // Check for collision against other blocks
            for (int x = 0; x < GRIDWIDTH; x++)
            {
                for (int y = 0; y < GRIDHEIGHT; y++)
                {
                    if (x >= newX && x < newX + 4)
                    {
                        if (y >= newY && y < newY + 4)
                        {
                            if (GridMap[x, y].bFilled == true &&
                                inBlock.Size[x - newX, y - newY] == true)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public Vector2 GetWorldPosition(float X, float Y)
        {
            return new Vector2(Position.X + TBlock.BrickSize * X, Position.Y + TBlock.BrickSize * Y);
        }

        public override Rectangle GetRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, GRIDWIDTH * TBlock.BrickSize, GRIDHEIGHT * TBlock.BrickSize);
        }

        public override void Draw(GameTime gameTime)
        {
            // Border for field            
            MyGame.SpriteBatch.Draw(borderTexture, new Rectangle((int)(Position.X - 5), (int)(Position.Y - 5), GetRectangle().Width + 10, GetRectangle().Height + 10), Color.White);

            // Draw Preview             
            MyGame.SpriteBatch.Draw(borderTexture, new Rectangle((int)mPreviewBlock.GetWorldPos().X - TBlock.BrickSize, (int)mPreviewBlock.GetWorldPos().Y, TBlock.BrickSize * 5, TBlock.BrickSize * 4), Color.White);

            //base.Draw(gameTime);

            for (int x = 0; x < GRIDWIDTH; x++)
            {
                for (int y = 0; y < GRIDHEIGHT; y++)
                {
                    if (GridMap[x, y].bFilled)
                    {
                        Vector2 gridWorldPos = GetWorldPosition(x, y);
                        MyGame.SpriteBatch.Draw(gridSlotTexture, new Rectangle((int)gridWorldPos.X, (int)gridWorldPos.Y, TBlock.BrickSize, TBlock.BrickSize), GridMap[x, y].gridColor);
                    }
                }
            }

            if (mCurrentAI != null)
            {
                mCurrentAI.Draw(gameTime);
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (mCurrentAI != null)
            {
                mCurrentAI.Update(gameTime);
            }



            timeLeftToMoveAfterDrop -= gameTime.ElapsedGameTime.TotalSeconds;

            // see if we're in the playing field yet, and that we haven't collided with anything yet                
            if (mCurrentBlock.YGridPos >= 0 &&
                CheckCollision(mCurrentBlock, 0, 1))
            {
                if (timeLeftToMoveAfterDrop < 0)
                {
                    DropBlock();
                }
            }
            else
            {
                float moveAmount = (1.0f / (float)dropRate) * mCurrentBlock.brickDropSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                mCurrentBlock.Move(0, moveAmount);

                timeLeftToMoveAfterDrop = dropRate;
            }

        }

        public TBlock GetRandomBlock()
        {
            TBlock Result = null;

            int blockIndex = mRandomizer.Next(7);

            // 0 = Line
            // 1 = square
            // 2 = T
            // 3 = Z
            // 4 = S
            // 5 = L
            // 6 = J

            switch (blockIndex)
            {
                case 0:
                    Result = new TBlock(this, MyGame, Color.Red);
                    Result.Size[1, 0] = true;
                    Result.Size[1, 1] = true;
                    Result.Size[1, 2] = true;
                    Result.Size[1, 3] = true;
                    break;
                case 1:
                    Result = new TBlock(this, MyGame, Color.Blue);
                    Result.Size[1, 1] = true;
                    Result.Size[1, 2] = true;
                    Result.Size[2, 1] = true;
                    Result.Size[2, 2] = true;
                    break;
                case 2:
                    Result = new TBlock(this, MyGame, Color.SteelBlue);
                    Result.Size[1, 1] = true;
                    Result.Size[0, 2] = true;
                    Result.Size[1, 2] = true;
                    Result.Size[2, 2] = true;
                    break;
                case 3:
                    Result = new TBlock(this, MyGame, Color.Yellow);
                    Result.Size[0, 1] = true;
                    Result.Size[1, 1] = true;
                    Result.Size[1, 2] = true;
                    Result.Size[2, 2] = true;
                    break;
                case 4:
                    Result = new TBlock(this, MyGame, Color.Green);
                    Result.Size[2, 1] = true;
                    Result.Size[1, 1] = true;
                    Result.Size[1, 2] = true;
                    Result.Size[0, 2] = true;
                    break;
                case 5:
                    Result = new TBlock(this, MyGame, Color.Purple);
                    Result.Size[1, 1] = true;
                    Result.Size[2, 1] = true;
                    Result.Size[2, 2] = true;
                    Result.Size[2, 3] = true;
                    break;
                case 6:
                    Result = new TBlock(this, MyGame, Color.White);
                    Result.Size[2, 1] = true;
                    Result.Size[1, 1] = true;
                    Result.Size[1, 2] = true;
                    Result.Size[1, 3] = true;
                    break;
                default:
                    break;
            }

            return Result;
        }

        public void DropBlock_Test(bool bPlace)
        {
            if (bPlace)
            {
                // Copy the grid map over                
                tempGridMap = (GridSlot[,])GridMap.Clone();

                for (int i = 0; i < TBlock.DIMENSIONS; i++)
                {
                    for (int j = 0; j < TBlock.DIMENSIONS; j++)
                    {
                        if (mCurrentBlock.Size[i, j] == true)
                        {
                            GridMap[i + mCurrentBlock.XGridPos, j + mCurrentBlock.YGridPos].bFilled = true;
                            GridMap[i + mCurrentBlock.XGridPos, j + mCurrentBlock.YGridPos].gridColor = mCurrentBlock.MyColor;
                        }
                    }
                }

                int rowRemoveCount = 0;
                // Check for cleared lines
                for (int y = 0; y < GRIDHEIGHT; y++)
                {
                    bool bClearedLine = true;
                    for (int x = 0; x < GRIDWIDTH; x++)
                    {
                        if (GridMap[x, y].bFilled != true)
                        {
                            bClearedLine = false;
                            break;
                        }
                    }

                    if (bClearedLine)
                    {
                        rowRemoveCount++;
                        RemoveRow(y, true);
                    }
                }

                if (rowRemoveCount > 0)
                {
                    if (LineClearEvent != null)
                    {
                        LineClearEvent(this, new LineClearEventArgs(rowRemoveCount));
                    }
                }
            }
            else
            {
                GridMap = (GridSlot[,])tempGridMap.Clone();
            }
        }

        public void DropBlock()
        {
            for (int i = 0; i < TBlock.DIMENSIONS; i++)
            {
                for (int j = 0; j < TBlock.DIMENSIONS; j++)
                {
                    if (mCurrentBlock.Size[i, j] == true)
                    {
                        GridMap[i + mCurrentBlock.XGridPos, j + mCurrentBlock.YGridPos].bFilled = true;
                        GridMap[i + mCurrentBlock.XGridPos, j + mCurrentBlock.YGridPos].gridColor = mCurrentBlock.MyColor;
                    }
                }
            }

            int rowRemoveCount = 0;
            // Check for cleared lines
            for (int y = 0; y < GRIDHEIGHT; y++)
            {
                bool bClearedLine = true;
                for (int x = 0; x < GRIDWIDTH; x++)
                {
                    if (GridMap[x, y].bFilled != true)
                    {
                        bClearedLine = false;
                        break;
                    }
                }

                if (bClearedLine)
                {
                    rowRemoveCount++;
                    RemoveRow(y, false);
                }
            }

            if (rowRemoveCount > 0)
            {
                if (LineClearEvent != null)
                {
                    LineClearEvent(this, new LineClearEventArgs(rowRemoveCount));
                }

                score += Convert.ToInt32(100 * rowRemoveCount * rowRemoveCount * 0.25 * level);
            }

            mState.RemoveObject(mCurrentBlock);
            mCurrentBlock = mPreviewBlock;
            mCurrentBlock.Reset();

            if (DropBlockEvent != null)
            {
                DropBlockEvent(this, EventArgs.Empty);
            }

            // If we couldn't place this block, then end the game.
            if (CheckCollision(mCurrentBlock, 0, 1))
            {
                FullFieldEvent(this, EventArgs.Empty);
            }

            ResetPreviewBlock();
        }

    }
}
