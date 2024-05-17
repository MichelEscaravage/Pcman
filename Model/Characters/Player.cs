using Cells.Model;
using Interfaces;
using PcMan.View;
using System.Security.Cryptography;

namespace PcMan.Model.Characters
{
    /// <summary>
    /// Player class, derived from Character class
    /// </summary>
    internal class Player : Character, IUpdatable
    {
        // Declare keyboard controller variable
        private KeyboardController keyboardController;

        // Constructor for Player class
        public Player(int top, int left, KeyboardController keyboardController)
        {
            // Set initial positions
            SetLeft(left);
            SetTop(top);

            // Initialize time-related variables
            TimeElapsed = new TimeSpan(0, 0, 0, 0, 0);
            Delay = new TimeSpan(0, 0, 0, 0, 100);

            // Set the keyboard controller
            this.keyboardController = keyboardController;

            // Set the player's symbol
            SetSymbol('P');

            // Set the player's color
            SetColor(ConsoleColor.Blue);

            // Move the player to the initial position
            Move(0, 0);
        }

        /// <summary>
        /// Update method, called continuously while the game is playing
        /// </summary>
        public void Update(TimeSpan timeElapsed)
        {
            this.TimeElapsed += timeElapsed;

            if (this.TimeElapsed > Delay)
            {
                bool moved = false;

                // Get the last pressed key from KeyboardController and reset it to null
                ConsoleKey lastPressed = keyboardController.ReadKey(true);

                // Check the last pressed key and attempt to move accordingly
                if (lastPressed == ConsoleKey.UpArrow)
                {
                    moved = TryMove(-1, 0); // Move up
                }
                else if (lastPressed == ConsoleKey.DownArrow)
                {
                    moved = TryMove(1, 0); // Move down
                }
                else if (lastPressed == ConsoleKey.RightArrow)
                {
                    moved = TryMove(0, 1); // Move right
                }
                else if (lastPressed == ConsoleKey.LeftArrow)
                {
                    moved = TryMove(0, -1); // Move left
                }

                if (moved)
                {
                    Cell cell = Game.GetGame().GetCell(GetTop(), GetLeft());
                    if (cell.ContainsCoin())
                    {
                        Game.GetGame().IncreaseScore(cell.GetCoin());
                    }
                    if (cell.ContainsLife())
                    {
                        Game.GetGame().IncreaseLife(cell.GetLife());
                    }
                    if (Game.GetGame().CheckCollision())
                    {
                        Game.GetGame().DecreaseLife(1);
                    }
                    if (cell.ContainsPortal())
                    {
                        Portal portal = cell.GetPortal();
                        Teleport();
                        portal.Refresh();
                    }
                    this.TimeElapsed -= Delay;
                }
            }
        }
    }
}
