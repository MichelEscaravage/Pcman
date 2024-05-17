using Cells.Model;
using Interfaces;
using PcMan.Model.Interfaces;
using System;
using System.Security.Cryptography;

namespace PcMan.Model.Characters
{
    internal class Portal : Character, IViewable
    {
        // Private fields to store the position, symbol, and color of the Portal object
        private int Left;
        private int Top;
        private char Symbol;
        private ConsoleColor color;

        // Constructor for the Portal class
        public Portal()
        {
            // Set default values for symbol and color, and place the Portal object on the game board
            Symbol = 'V';
            color = ConsoleColor.Magenta;
            Place();
        }

        // Method to place the Portal object on a random enterable cell on the game board
        public void Place()
        {
            // Get the current game instance
            Game game = Game.GetGame();

            // Get the cell at the current position
            Cell currentCell = Game.GetGame().GetCell(GetTop(), GetLeft());

            // Flag to check if the Portal object is successfully placed
            bool isPlaced = false;

            // Loop until the Portal object is placed
            while (!isPlaced)
            {
                // Generate random top and left coordinates
                int setTop = RandomNumberGenerator.GetInt32(0, game.GetHeight());
                int setLeft = RandomNumberGenerator.GetInt32(0, game.GetWidth());

                // Get the cell at the new coordinates
                Cell newCell = game.GetCell(setTop, setLeft);

                // Check if the new cell is enterable
                if (Game.GetGame().GetCell(setTop, setLeft).IsEnterable())
                {
                    // Leave the current cell and enter the new cell
                    Game.GetGame().GetCell(GetTop(), GetLeft()).LeaveCell(this);
                    Left = setLeft;
                    Top = setTop;
                    isPlaced = true;
                    Game.GetGame().GetCell(setTop, setLeft).EnterCell(this);
                }
            }
        }

        // Method to refresh the display of the Portal object on the console
        public void Refresh()
        {
            Console.SetCursorPosition(Left, Top);
            Console.ForegroundColor = color;
            Console.Write(Symbol);
        }
    }
}
