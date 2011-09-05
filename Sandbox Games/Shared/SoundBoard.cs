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
    public class SoundBoard : DrawableGameComponent
    {
        const int SOUND_BOARD_WIDTH = 16;
        const int SOUND_BOARD_HEIGHT = 12;
        float drawScale;
        public SoundBoardNode[,] soundBoardNodes;
        public string[] waveName = new string[SOUND_BOARD_HEIGHT];
        public Vector2 myPosition = new Vector2(140, 70);
        public float nodeSpacing = 80.0f;
        public float bpm = 120;
        double elapsedBeatTime = 0.0;
        int currentColumn = 0;

        Game_SoundBoard myGame;

        public SoundBoard(Game_SoundBoard inGame) :
            base(inGame)
        {
            soundBoardNodes = new SoundBoardNode[SOUND_BOARD_WIDTH, SOUND_BOARD_HEIGHT];
            myGame = inGame;
            drawScale = 8.0f / SOUND_BOARD_WIDTH;
        }

        public void AddBPM(float bpmValue)
        {
            bpm += bpmValue;
        }
        public void LoadSoundLibrary(string SoundBankDirectory)
        {
            //loop through your folder and load all of its contents into the sound effects array 
            //this assumes that all of your content is in that folder 
            ICollection<string> fileList = Directory.GetFiles(myGame.Content.RootDirectory + "\\Audio\\" + SoundBankDirectory);

            for (int i = 0; i < SOUND_BOARD_WIDTH; i++)
            {
                for (int j = 0; j < SOUND_BOARD_HEIGHT; j++)
                {
                    int idx = j;
                    SoundBoardNode newNode = null;

                    waveName[j] = string.Empty;

                    if (idx < fileList.Count)
                    {
                        newNode = new SoundBoardNode(myGame, fileList.ElementAt(idx));
                        string[] splitStrings = Path.GetFileNameWithoutExtension(fileList.ElementAt(idx)).Split('_');
                        waveName[j] = splitStrings[splitStrings.Count() - 1];
                    }
                    else
                    {
                        newNode = new SoundBoardNode(myGame, string.Empty);
                    }

                    newNode.myPosition = myPosition + new Vector2(i * nodeSpacing * drawScale, j * nodeSpacing * drawScale);
                    newNode.myDrawScale = drawScale;
                    soundBoardNodes[i, j] = newNode;
                }
            }
        }

        public void PlaySoundNode(Vector2 screenPos)
        {
            // Figure out whether this position intersects with any of our sprites
            for (int i = 0; i < SOUND_BOARD_WIDTH; i++)
            {
                for (int j = 0; j < SOUND_BOARD_HEIGHT; j++)
                {
                    if (soundBoardNodes[i, j].PointIsInsideSprite(screenPos))
                    {
                        soundBoardNodes[i, j].bEnabled = !soundBoardNodes[i, j].bEnabled;
                    }
                }
            }
        }

        public void PlaySoundNode(int i, int j)
        {
            soundBoardNodes[i, j].Play();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            elapsedBeatTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedBeatTime > (60.0 / bpm))
            {
                elapsedBeatTime -= (60.0 / bpm);

                for (int i = 0; i < SOUND_BOARD_HEIGHT; i++)
                {
                    soundBoardNodes[currentColumn, i].Play();
                }

                currentColumn++;

                if (currentColumn >= SOUND_BOARD_WIDTH)
                {
                    currentColumn = 0;
                }

            }

            for (int i = 0; i < SOUND_BOARD_WIDTH; i++)
            {
                for (int j = 0; j < SOUND_BOARD_HEIGHT; j++)
                {
                    soundBoardNodes[i, j].Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            myGame.spriteBatch.Begin();
            for (int i = 0; i < SOUND_BOARD_WIDTH; i++)
            {
                for (int j = 0; j < SOUND_BOARD_HEIGHT; j++)
                {
                    soundBoardNodes[i, j].Draw(gameTime);
                }
            }
            myGame.spriteBatch.DrawString(myGame.myFont, "BPM: " + bpm + " currentColumn: " + currentColumn, new Vector2(100, 10), Color.White);
            //myGame.spriteBatch.DrawString(myGame.myFont, "ElapsedBeatTime: "+elapsedBeatTime, new Vector2(100, 30), Color.White);

            for (int i = 0; i < SOUND_BOARD_HEIGHT; i++)
            {
                myGame.spriteBatch.DrawString(myGame.myFont, waveName[i], myPosition + new Vector2(-100, drawScale * nodeSpacing * i), Color.White);
            }
            myGame.spriteBatch.End();
        }
    }

    public class SoundBoardNode
    {
        public SoundEffect mySoundEffect;
        public Texture2D mySprite;
        Color[] spriteTextureData;
        public Vector2 myPosition;
        public float myDrawScale;
        public Game_SoundBoard myGame;
        public Color myColor;
        public double lastPlayTime = -1000.0;
        public double fadeColorTime = 1.0f;
        public bool bWantsToPlay = false;
        public bool bEnabled = false;

        public bool PointIsInsideSprite(Vector2 point)
        {

            // Get the bounding rectangle of this block
            Rectangle spriteRectangle =
                new Rectangle((int)myPosition.X, (int)myPosition.Y,
                (int)(mySprite.Width * myDrawScale), (int)(mySprite.Height * myDrawScale));

            // Find the bounds of the rectangle intersection
            int top = Math.Max(spriteRectangle.Top, (int)point.Y - 1);
            int bottom = Math.Min(spriteRectangle.Bottom, (int)point.Y + 1);
            int left = Math.Max(spriteRectangle.Left, (int)point.X - 1);
            int right = Math.Min(spriteRectangle.Right, (int)point.X + 1);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = spriteTextureData[(x - spriteRectangle.Left) +
                                         (y - spriteRectangle.Top) * spriteRectangle.Width];

                    // If the pixels are not completely transparent,
                    if (colorA.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            return false;

        }

        public SoundBoardNode(Game_SoundBoard inGame, string soundEffectFileName)
        {
            myGame = inGame;
            mySoundEffect = null;

            if (soundEffectFileName != string.Empty)
            {
                // Strip off the end.
                string[] splitFileName = soundEffectFileName.Split('.');
                mySoundEffect = myGame.Content.Load<SoundEffect>(splitFileName[0]);
            }
            myColor = Color.White;
            mySprite = myGame.Content.Load<Texture2D>("Sprites\\Circle");

            spriteTextureData = new Color[mySprite.Width * mySprite.Height];
            mySprite.GetData(spriteTextureData);
        }

        public void Update(GameTime gameTime)
        {
            if (bWantsToPlay)
            {
                lastPlayTime = gameTime.TotalGameTime.TotalSeconds;

                bWantsToPlay = false;

                if (mySoundEffect != null &&
                    bEnabled)
                {
                    mySoundEffect.Play();
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            double elapsedTime = gameTime.TotalGameTime.TotalSeconds - lastPlayTime;
            byte currentColorValue = 255;

            if (elapsedTime < fadeColorTime)
            {
                currentColorValue = (byte)(255 * Math.Min(1.0, (gameTime.TotalGameTime.TotalSeconds - lastPlayTime) / fadeColorTime));
            }

            myColor.R = currentColorValue;
            myColor.B = currentColorValue;
            myColor.B = bEnabled ? (byte)0 : (byte)255;
            myGame.spriteBatch.Draw(mySprite, myPosition, new Rectangle(0, 0, (int)mySprite.Width, (int)mySprite.Height), myColor, 0.0f, new Vector2(0.0f), myDrawScale, SpriteEffects.None, 0.0f);
        }

        public void Play()
        {
            bWantsToPlay = true;

        }
    }

}
