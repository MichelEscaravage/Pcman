using Cells.Model;
using Interfaces;
using PcMan.View;
using System.Security.Cryptography;

namespace PcMan.Model.Characters
{
    internal class Player2 : Character, IUpdatable
    {
        // Declare keyboard controller variable
        private KeyboardController2 keyboardController2;

        // Constructor for Player class
        public Player2(int top, int left, KeyboardController2 keyboardController2)
        {
            // Set initial positions
            SetLeft(left);
            SetTop(top);

            // Initialize time-related variables
            TimeElapsed = new TimeSpan(0, 0, 0, 0, 0);
            Delay = new TimeSpan(0, 0, 0, 0, 100);

            // Set the keyboard controller
            this.keyboardController2 = keyboardController2;

            // Set the player's symbol
            SetSymbol('T');

            // Set the player's color
            SetColor(ConsoleColor.Yellow);

            // Move the player to the initial position
            Move(0, 0);
        }

        /// <summary>
        /// Update method, called continuously while the game is playing
        /// </summary>
        /// <param name="timeElapsed">The time elapsed since the last update</param>
        public void Update(TimeSpan timeElapsed)
        {
            this.TimeElapsed += timeElapsed;

            if (this.TimeElapsed > Delay)
            {
                bool moved = false;

                // Get the last pressed key from KeyboardController and reset it to null
                ConsoleKey lastPressed = keyboardController2.ReadKey(true);

                // Check the last pressed key and attempt to move accordingly
                if (lastPressed == ConsoleKey.W)
                {
                    moved = TryMove(-1, 0); // Move up
                }
                else if (lastPressed == ConsoleKey.S)
                {
                    moved = TryMove(1, 0); // Move down
                }
                else if (lastPressed == ConsoleKey.D)
                {
                    moved = TryMove(0, 1); // Move right
                }
                else if (lastPressed == ConsoleKey.A)
                {
                    moved = TryMove(0, -1); // Move left
                }

                if (moved)
                {
                    Cell cell = Game.GetGame().GetCell(GetTop(), GetLeft());

                    // Check for coin in the cell and update score
                    if (cell.ContainsCoin())
                    {
                        Game.GetGame().IncreaseScore2(cell.GetCoin());
                    }

                    // Check for life in the cell and update life count
                    if (cell.ContainsLife())
                    {
                        Game.GetGame().IncreaseLife(cell.GetLife());
                    }

                    // Check for collision with obstacles and decrease life count
                    if (Game.GetGame().CheckCollision())
                    {
                        Game.GetGame().DecreaseLife(1);
                    }

                    // Check for portal in the cell and teleport
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
