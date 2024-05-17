namespace PcMan.Model.Interfaces
{
    // Interface definition for objects that can be viewed in the game
    internal interface IViewable
    {
        // Method to get the top position of the object
        public int GetTop();

        // Method to get the left position of the object
        public int GetLeft();

        // Method to get the symbol representing the object
        public char GetSymbol();

        // Method to get the color of the object
        public ConsoleColor GetColor();
    }
}
