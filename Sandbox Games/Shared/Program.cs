using System;
using Sandbox_Games.Tetris;

namespace Sandbox_Games
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game_TetrisAI game = new Game_TetrisAI())
            {
                game.Run();
            }
        }
    }
}

