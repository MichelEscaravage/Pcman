using Cells.Model;
using Interfaces;

namespace PcMan.Model.Characters
{
    internal class RandomEnemy : Character, IUpdatable
    {
        // Constructor for RandomEnemy class
        public RandomEnemy(int top, int left)
        {
            SetLeft(left);
            SetLeft(top);

            // Set delay between updates and initialize elapsed time
            Delay = new TimeSpan(0, 0, 0, 0, 100);
            TimeElapsed = new TimeSpan(0, 0, 0, 0, 0);

            // Initialize the movement direction randomly
            ChangeDirection();

            // Set the enemy's symbol
            SetSymbol('#');

            // Set the enemy's color
            SetColor(ConsoleColor.Yellow);

            // Move the enemy to the initial position
            Move(0, 0);
        }

        // Updates the enemy state based on the elapsed time, attempting to move in the current direction after a specified delay and changing direction regardless.
        public void Update(TimeSpan timeElapsed)
        {
            this.TimeElapsed += timeElapsed;

            if (this.TimeElapsed > Delay)
            {
                // Attempt to move in the current direction
                bool moved = TryMove(GetDirectionTop(), GetDirectionLeft());

                // Change direction regardless of whether the enemy moved or not
                ChangeDirection();

                if (moved)
                {

                    Cell cell = Game.GetGame().GetCell(GetTop(), GetLeft());
                    // Decrease timeElapsed by the delay
                    this.TimeElapsed -= Delay;
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
            }
        }
    }
}
