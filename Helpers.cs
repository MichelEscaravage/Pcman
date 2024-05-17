namespace PcMan
{
    // A static class containing helper methods for the PcMan namespace
    static class Helpers
    {
        // Method to prompt the user with a question and return their input
        static public string Ask(string question)
        {
            // Display the question to the user
            Console.WriteLine(question);

            // Read the user's input and convert it to lowercase
            return Console.ReadLine()?.ToLower();
        }
    }
}
