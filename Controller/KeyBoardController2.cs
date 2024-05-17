

using Interfaces;
internal class KeyboardController2 : IUpdatable
{
    private ConsoleKey lastPressed;

    public KeyboardController2()
    {

    }
    // Returns the last pressed key without resetting it.
    public ConsoleKey ReadKey()
    {
        return lastPressed;
    }
    // Updates the input state, capturing the last pressed key if available.
    public void Update(TimeSpan deltaTime)
    {

        if (Console.KeyAvailable)
        {
            lastPressed = Console.ReadKey(true).Key;
        }
    }
    /// Returns the last pressed key and optionally resets it./
    public ConsoleKey ReadKey(bool resetKey)
    {
        if (resetKey)
        {
            ConsoleKey tmp = lastPressed;
            lastPressed = 0;
            return tmp;
        }

        return lastPressed;
    }
}

