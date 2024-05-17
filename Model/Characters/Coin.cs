using Cells.Model;
using PcMan.Model.interfaces;
using PcMan.Model.Interfaces;
using System;
using System.Security.Cryptography;

namespace PcMan.Model.Characters
{
    internal class Coin : ICollectable, IViewable
    {
        // Private fields to store the position, symbol, color, and coin value of the Coin object
        private int top;
        private int left;
        private char symbol;
        private ConsoleColor color;
        private int coinValue = 1;

        // Constructor for the Coin class
        public Coin()
        {
            // Set default values for symbol and color, and place the Coin object on the game board
            symbol = '$';
            color = ConsoleColor.DarkGreen;
            Place();
        }

        // Method to place the Coin object on a random enterable cell on the game board
        public void Place()
        {
            // Get the current game instance
            Game game = Game.GetGame();

            // Get the cell at the current position
            Cell currentCell = Game.GetGame().GetCell(GetTop(), GetLeft());

            // Flag to check if the Coin object is successfully placed
            bool isPlaced = false;

            // Loop until the Coin object is placed
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
                    Game.GetGame().GetCell(top, left).LeaveCell(this);
                    left = setLeft;
                    top = setTop;
                    isPlaced = true;
                    Game.GetGame().GetCell(setTop, setLeft).EnterCell(this);
                }
            }
        }

        // Method to simulate picking up the Coin object and placing it in a new location
        public int Pickup()
        {
            Place();
            return coinValue;
        }

        // Method to get the top position of the Coin object
        public int GetTop()
        {
            return top;
        }

        // Method to get the left position of the Coin object
        public int GetLeft()
        {
            return left;
        }

        // Method to get the symbol representing the Coin object
        public char GetSymbol()
        {
            return symbol;
        }

        // Method to get the color of the Coin object
        public ConsoleColor GetColor()
        {
            return color;
        }
    }
}
