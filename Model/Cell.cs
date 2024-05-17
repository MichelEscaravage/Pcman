using Interfaces;
using PcMan.Model;
using PcMan.Model.Characters;
using PcMan.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cells.Model
{
    internal class Cell : IViewable
    {
        private int left;
        private int top;
        private bool isEnterable = true;
        private List<IViewable> viewables;

        // Constructor for the Cell class.
        // Initializes the cell with the specified top and left positions.
        public Cell(int top, int left)
        {
            this.left = left;
            this.top = top;
            viewables = new List<IViewable>();
            Draw();
        }

        // Gets the symbol representing the wall.
        public char GetWallSymbol()
        {
            return '!';
        }

        // Gets the color of the first viewable or white if no viewables are present.
        public ConsoleColor GetColor()
        {
            if (viewables.Count > 0)
            {
                return viewables[0].GetColor();
            }
            else
            {
                return ConsoleColor.White;
            }
        }

        // Gets the left position of the cell.
        public int GetLeft()
        {
            return left;
        }

        // Gets the symbol of the first viewable or ' ' (empty) if no viewables are present.
        public char GetSymbol()
        {
            if (viewables.Count > 0)
            {
                return viewables[0].GetSymbol();
            }
            else
            {
                return ' ';
            }
        }

        // Gets the top position of the cell.
        public int GetTop()
        {
            return top;
        }

        // Gets the count of viewables in the cell.
        public int GetViewableCount()
        {
            return viewables.Count;
        }

        // Determines if the cell is enterable.
        public bool IsEnterable()
        {
            return isEnterable;
        }

        // Adds a viewable to the cell.
        public void EnterCell(IViewable viewable)
        {
            List<IViewable> copiedList = new List<IViewable>(viewables);
            viewables.Add(viewable);
            Draw();
        }

        // Checks if the cell contains a coin.
        public bool ContainsCoin()
        {
            foreach (IViewable viewable in viewables)
            {
                if (viewable is Coin)
                {
                    return true;
                }
            }
            return false;
        }

        // Checks if the cell contains a life.
        public bool ContainsLife()
        {
            foreach (IViewable viewable in viewables)
            {
                if (viewable is Life)
                {
                    return true;
                }
            }
            return false;
        }

        // Checks if the cell contains a portal.
        public bool ContainsPortal()
        {
            foreach (IViewable viewable in viewables)
            {
                if (viewable is Portal)
                {
                    return true;
                }
            }
            return false;
        }

        // Gets the value of the coin in the cell and removes it from the cell.
        public int GetCoin()
        {
            Coin coin = viewables.OfType<Coin>().FirstOrDefault();
            if (coin != null)
            {
                LeaveCell(coin);
                return coin.Pickup();
            }
            return 0;
        }

        // Gets the value of the life in the cell and removes it from the cell.
        public int GetLife()
        {
            Life life = viewables.OfType<Life>().FirstOrDefault();
            if (life != null)
            {
                LeaveCell(life);
                return life.Pickup();
            }
            return 0;
        }

        // Gets the portal in the cell.
        public Portal GetPortal()
        {
            foreach (IViewable viewable in viewables)
            {
                if (viewable is Portal)
                {
                    return (Portal)viewable;
                }
            }
            return null;
        }

        // Removes a viewable from the cell.
        public void LeaveCell(IViewable viewable)
        {
            viewables.Remove(viewable);
            Draw();
        }

        // Makes the cell a wall by setting IsEnterable to false.
        public void MakeWall()
        {
            isEnterable = false;
            Draw();
        }

        // Draws the cell, displaying the symbol and color of the first viewable,
        // or the cell's symbol and color if no viewables are present.
        public void Draw()
        {
            Console.SetCursorPosition(left, top);

            if (!isEnterable)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(GetWallSymbol());
            }
            else if (viewables.Count > 0)
            {
                Console.ForegroundColor = viewables[0].GetColor();
                Console.Write(viewables[0].GetSymbol());
            }
            else
            {
                Console.ForegroundColor = GetColor();
                Console.Write(GetSymbol());
            }
        }
    }
}
