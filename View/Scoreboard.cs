using PcMan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcMan.View
{
    internal class Scoreboard
    {
        private int Left;
        private int Top;

        ConsoleColor color;

        public Scoreboard(int left, int top)
        {
            color = ConsoleColor.Red;
            Left = left;
            Top = top;
            Counters();
        }

        /// <summary>
        /// Displays the game counters on the scoreboard.
        /// </summary>
        public void Counters()
        {
            Game game = Game.GetGame();

            // Clear the previous content on the scoreboard
            Console.SetCursorPosition(Left, Top);
            Console.Write(new string(' ', 30));

            // Display the updated content on the scoreboard
            Console.SetCursorPosition(Left, Top);
            Console.Write($"Total Lives: {game.GetLife()}");

            Console.SetCursorPosition(Left, Top + 1);
            Console.Write($"Player 1: Score:{game.GetScore()}");

            // Move to the next line on the scoreboard
            Console.SetCursorPosition(Left, Top + 2);
            Console.Write($"Player 2: Score:{game.GetScore2()}");

            Console.SetCursorPosition(Left, Top + 3);
            Console.Write($"Current level:{game.GetCurrentLevel()}| Score to win:{game.GetScoreToWin()}");
        }

        /// <summary>
        /// Gets the instance of the Scoreboard.
        /// </summary>
        /// <param name="scoreboard">The Scoreboard instance to get.</param>
        /// <returns>The same Scoreboard instance.</returns>
        public Scoreboard GetScoreboard(Scoreboard scoreboard)
        {
            return scoreboard;
        }
    }
}
