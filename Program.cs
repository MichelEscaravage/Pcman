
using PcMan.Model;

namespace PcMan
{
    // The main entry point for the application
    static class Program
    {
        // The entry point for the application, initiating a new game session based on user input.
        static public void Main(String[] args)
        {
            // Clear the console screen
            Console.Clear();

            // Keep asking the user for a new game until they type 'X' for exit
            while (Helpers.Ask("Type 'X' to stop or any other key to play!") != "x")
            {
                // Hide the cursor during the game
                Console.CursorVisible = false;

                // Create a new instance of the Game class
                Game game = new Game();

                // Start the game session
                game.Play();

                // Set the cursor position for the exit message
                Console.SetCursorPosition(0, 25);

                // Show the cursor again
                Console.CursorVisible = true;
            }

            // Display an exit message when the user chooses to exit
            Console.WriteLine("Exit Game!");
        }
    }
}
