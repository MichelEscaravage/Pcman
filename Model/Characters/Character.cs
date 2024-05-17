using PcMan.Model.Interfaces;
using System.Security.Cryptography;

namespace PcMan.Model.Characters
{
    internal class Character : IViewable
    {
        private int directionTop;
        private int directionLeft;
        private int Left;
        private int Top;
        private char Symbol;
        private ConsoleColor Color;
        public TimeSpan Delay;
        public TimeSpan TimeElapsed;

        // Gets the current top direction of the character.
        public int GetDirectionTop()
        {
            return directionTop;
        }

        // Sets the top direction of the character.
        public void SetDirectionTop(int directionTop)
        {
            this.directionTop = directionTop;
        }

        // Gets the current left direction of the character.
        public int GetDirectionLeft()
        {
            return directionLeft;
        }

        // Sets the left direction of the character.
        public void SetDirectionLeft(int directionLeft)
        {
            this.directionLeft = directionLeft;
        }

        // Gets the top position of the character.
        public int GetTop()
        {
            return Top;
        }

        // Sets the top position of the character.
        public void SetTop(int top)
        {
            Top = top;
        }

        // Gets the left position of the character.
        public int GetLeft()
        {
            return Left;
        }

        // Sets the left position of the character.
        public void SetLeft(int left)
        {
            Left = left;
        }

        // Gets the symbol representing the character.
        public char GetSymbol()
        {
            return Symbol;
        }

        // Sets the symbol representing the character.
        public void SetSymbol(char symbol)
        {
            Symbol = symbol;
        }

        // Gets the color of the character.
        public ConsoleColor GetColor()
        {
            return Color;
        }

        // Sets the color of the character.
        public void SetColor(ConsoleColor color)
        {
            Color = color;
        }

        // Gets the delay of the character's actions.
        public TimeSpan GetDelay()
        {
            return Delay;
        }

        // Sets the delay of the character's actions.
        public void SetDelay(TimeSpan delay)
        {
            Delay = delay;
        }

        // Gets the elapsed time since the last action of the character.
        public TimeSpan GetTimeSpan()
        {
            return TimeElapsed;
        }

        // Sets the elapsed time since the last action of the character.
        public void SetTimeSpan(TimeSpan timeElapsed)
        {
            TimeElapsed = timeElapsed;
        }

        // Changes the direction of the character randomly for movement.
        public void ChangeDirection()
        {
            bool isMoving = false;

            while (!isMoving)
            {
                directionTop = Game.GetGame().RandomBetween(0, 3) - 1;
                directionLeft = Game.GetGame().RandomBetween(0, 3) - 1;

                if (directionLeft != 0 || directionTop != 0)
                {
                    isMoving = true;
                }
            }
        }

        // Attempts to move the character by the specified deltas if allowed.
        public bool TryMove(int deltaTop, int deltaLeft)
        {
            if (CanMove(deltaTop, deltaLeft))
            {
                Move(deltaTop, deltaLeft);
                return true;
            }
            return false;
        }

        // Checks if the character can move to the specified position.
        public bool CanMove(int deltaTop, int deltaLeft)
        {
            if (Game.GetGame().CellExists(Top + deltaTop, Left + deltaLeft) && Game.GetGame().GetCell(Top + deltaTop, Left + deltaLeft).IsEnterable())
            {
                return true;
            }

            return false;
        }

        // Moves the character by the specified deltas and updates the console display.
        public void Move(int deltaTop, int deltaLeft)
        {
            Game.GetGame().GetCell(Top, Left).LeaveCell(this);

            Top += deltaTop;
            Left += deltaLeft;

            Game.GetGame().GetCell(Top, Left).EnterCell(this);
        }

        // Teleports the character to a random position on the screen.
        public void Teleport()
        {
            Console.SetCursorPosition(Left, Top);
            Console.Write(" ");

            Left = RandomNumberGenerator.GetInt32(1, 65);
            Top = RandomNumberGenerator.GetInt32(1, 26);

            Move(0, 0);
        }

        // Placeholder method indicating an action that hurts the player.
        public void HurtPlayer()
        {
            Game.GetGame().GetPlayer();
        }
    }
}
