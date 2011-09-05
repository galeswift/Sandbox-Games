using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Sandbox_Games.Tetris
{
    class AI_Basic
    {
        TField mField;
        public double updateTimer = 0.0;
        // How often the AI makes a move, per second
        public double updateFrequency = 0.75;
        double debug_lastBestScore = 0.0;
        int debug_lastNumEdges = 0;
        int debug_lastNumHoles = 0;
        int debug_bestGridX;
        int debug_bestGridY;
        int debug_bestRotation;
        float debug_heightPenalty;

        TBlock GetCurrentBlock()
        {
            return mField.mCurrentBlock;
        }

        public AI_Basic(TField inField)
        {
            mField = inField;
        }

        public void Draw(GameTime inGameTime)
        {          
        }

        // Returns how many holes are in the specified column
        public int GetNumHoles(int inXPos)
        {
            int result = 0;
            bool bFoundBlock = false;
            for (int y = 0; y < mField.GRIDHEIGHT; y++)
            {
                if (mField.GridMap[inXPos, y].bFilled == true)
                {
                    bFoundBlock = true;
                }
                else if (bFoundBlock)
                {
                    result++;
                }
            }

            return result;
        }

        // Returns the average height of all blocks in the current playfield
        float GetAverageBlockHeight()
        {
            float result = 0;
            int numBlocks = 0;
            for (int x = 0; x < mField.GRIDWIDTH; x++)
            {
                for (int y = 0; y < mField.GRIDHEIGHT; y++)
                {
                    if (mField.GridMap[x, y].bFilled == true)
                    {
                        result += y;
                        numBlocks++;
                    }
                }
            }

            result /= numBlocks;

            return result;
        }

        //AI Update
        public void Update(GameTime gameTime)
        {
            // Update how much time has passed
            updateTimer += gameTime.ElapsedGameTime.TotalSeconds;

            // If we've already placed the block, or it's not time to update yet, bail
            if (GetCurrentBlock().bPlaced ||
                updateTimer < updateFrequency)
            {
                return;
            }

            updateTimer = 0.0;

            float bestScore = -10000;
            int bestGridX = 0;
            int bestGridY = 0;
            int bestRotation = 0;

            // Set up the block for testing
            GetCurrentBlock().Reset();

            for (int i = 0; i < 4; i++)
            {

                // First move it all the way left
                while (!mField.CheckCollision(GetCurrentBlock(), -1, 0))
                {
                    GetCurrentBlock().Move(-1, 0);
                }

                do
                {
                    // Now test it on the ground
                    while (!mField.CheckCollision(GetCurrentBlock(), 0, 1))
                    {
                        GetCurrentBlock().Move(0, 1);
                    }
                    // It's at the bottom, so see how many edges are touching
                    int numEdges = mField.GetNumTouchingEdges(GetCurrentBlock());
                    float currentScore = numEdges;

                    // Set up the field as if we placed the block
                    mField.DropBlock_Test(true);

                    // Count how many holes are in the field
                    int totalHoles = 0;

                    for (int x = 0; x < mField.GRIDWIDTH; x++)
                    {
                        totalHoles += GetNumHoles(x);
                    }

                    currentScore -= totalHoles;

                    // Get the average height of the blocks in the field
                    float heightPenalty = mField.GRIDHEIGHT - GetAverageBlockHeight();
                    currentScore -= heightPenalty;

                    // Reset the field as if we had not placed the block.
                    mField.DropBlock_Test(false);

                    // See if we haev a new best score
                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        bestGridX = GetCurrentBlock().XGridPos;
                        bestGridY = GetCurrentBlock().YGridPos;
                        bestRotation = i;

                        debug_lastBestScore = bestScore;
                        debug_lastNumEdges = numEdges;
                        debug_lastNumHoles = totalHoles;
                        debug_bestGridX = bestGridX;
                        debug_bestGridY = bestGridY;
                        debug_bestRotation = bestRotation;
                        debug_heightPenalty = heightPenalty;
                        //                        Console.WriteLine("Testing: bestScore {0} bestGridX {1} bestGrixY {2} bestRotation {3} ", bestScore, bestGridX, bestGridY, bestRotation);
                    }

                    // Move it back to the top.
                    GetCurrentBlock().YGridPos = 0;

                    // Move it one slot over
                    GetCurrentBlock().Move(1, 0);
                } while (!mField.CheckCollision(GetCurrentBlock(), 0, 0));

                // Collided, so we're done all the way to the right, reset our position
                GetCurrentBlock().Reset();

                // Rotate it once to the right
                mField.RotateBlock(GetCurrentBlock(), true);
            }

            // Console.WriteLine("bestScore {0} bestGridX {1} bestGrixY {2} bestRotation {3} ", bestScore, bestGridX, bestGridY, bestRotation);
            for (int i = 0; i < bestRotation; i++)
            {
                mField.RotateBlock(GetCurrentBlock(), true);
            }

            GetCurrentBlock().XGridPos = bestGridX;
            GetCurrentBlock().YGridPos = bestGridY;
            GetCurrentBlock().bPlaced = true;

            mField.DropBlock();

        }
    }
}
