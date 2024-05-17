using Cells.Model;
using Interfaces; // Importing necessary interface(s)
using PcMan.View;

namespace PcMan.Model.Characters
{
    internal class Bouncer : Character, IUpdatable // Class definition, inheriting from Character class and implementing IUpdatable interface
    {
        public Bouncer(int top, int left) // Constructor with initial position parameters
        {
            SetLeft(left); // Set the initial left position
            SetTop(top);   // Set the initial top position

            // Set the delay between updates and initialize elapsed time
            Delay = new TimeSpan(0, 0, 0, 0, 300);
            TimeElapsed = new TimeSpan(0, 0, 0, 0, 0);

            ChangeDirection(); // Initialize the movement direction

            // Check if the direction is valid, throw an exception if not
            if (GetDirectionTop() > 1 || GetDirectionLeft() > 1 || GetDirectionTop() < -1 || GetDirectionLeft() < -1 || (GetDirectionLeft() == 0 && GetDirectionTop() == 0))
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(GetDirectionTop() + "," + GetDirectionLeft());

                throw new Exception("Invalid direction");
            }

            SetSymbol('X'); // Set the character symbol
            SetColor(ConsoleColor.DarkRed); // Set the character color

            Move(0, 0); // Move the character to the initial position
        }

        // Updates the player state based on the elapsed time, attempting to move in the current direction after a specified delay.
        public void Update(TimeSpan timeElapsed)
        {
            this.TimeElapsed += timeElapsed; // Increment the elapsed time

            if (this.TimeElapsed > Delay) // Check if it's time to update
            {
                bool moved = TryMove(GetDirectionTop(), GetDirectionLeft()); // Try to move in the current direction

                if (moved)
                {

                    Cell cell = Game.GetGame().GetCell(GetTop(), GetLeft());
                    this.TimeElapsed -= Delay; // Subtract the delay from elapsed time if successfully moved
                    if (GetLeft() == Game.GetGame().GetPlayer().left && GetTop() == Game.GetGame().GetPlayer().top) 
                    {
                        Game.GetGame().DecreaseLife(1);
                    }
                    if (cell.ContainsPortal())
                    {
                        Portal portal = cell.GetPortal();
                        Teleport();
                        portal.Refresh();
                    }


                }
                else
                {
                    ChangeDirection(); // Change direction if movement was not successful
                }
            }
        }
    }
}