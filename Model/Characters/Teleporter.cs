using Cells.Model;
using Interfaces;
using PcMan.Model.Interfaces;
using System.Drawing;

namespace PcMan.Model.Characters
{
    internal class Teleporter : Character, IUpdatable
    {
        // Constructor for Teleporter class
        public Teleporter(int top, int left)
        {
            SetLeft(left);
            SetTop(top);

            // Set delay between updates and initialize elapsed time
            Delay = new TimeSpan(0, 0, 0, 2, 0);
            TimeElapsed = new TimeSpan(0, 0, 0, 0, 0);

            // Set the teleporter's symbol
            SetSymbol('%');

            // Set the teleporter's color
            SetColor(ConsoleColor.Magenta);

            // Move the teleporter to the initial position
            Move(0, 0);
        }

        // Updates the teleporter state based on the elapsed time, attempting to move in a random direction after a specified delay.
        public void Update(TimeSpan timeElapsed)
        {
            this.TimeElapsed += timeElapsed;

            if (this.TimeElapsed > Delay)
            {
                // Attempt to move in a random direction
                bool moved = TryMove(Game.GetGame().RandomBetween(-25, 25), Game.GetGame().RandomBetween(-70, 70));

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
