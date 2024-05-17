
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

internal class HighScoreManager
{
    // File path to store high scores
    private const string HighScoresFilePath = "highscores.txt";

    // List to store high score entries
    private List<HighScoreEntry> highScores;

    // Constructor to initialize highScores by loading existing high scores
    public HighScoreManager()
    {
        highScores = LoadHighScores();
    }

    // Method to add a new high score entry, update the list, and save to file
    public void AddHighScore(string playerName, int score)
    {
        highScores.Add(new HighScoreEntry(playerName, score));
        highScores = highScores.OrderByDescending(entry => entry.Score).ToList();
        SaveHighScores();
    }

    // Method to display high scores on the console
    public void DisplayHighScores()
    {
        Console.Clear();
        Console.WriteLine("High Scores:");
        foreach (var entry in highScores)
        {
            Console.WriteLine($"{entry.PlayerName}: {entry.Score}");
        }
        Console.WriteLine();
    }

    // Method to get the lowest score from the high score list
    public int GetLowestScore()
    {
        return highScores.Min(entry => entry.Score);
    }

    // Method to load high scores from the file
    private List<HighScoreEntry> LoadHighScores()
    {
        List<HighScoreEntry> scores = new List<HighScoreEntry>();

        if (File.Exists(HighScoresFilePath))
        {
            try
            {
                string[] lines = File.ReadAllLines(HighScoresFilePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                    {
                        scores.Add(new HighScoreEntry(parts[0], score));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading high scores: {ex.Message}");
            }
        }

        return scores;
    }

    // Method to save high scores to the file
    private void SaveHighScores()
    {
        try
        {
            File.WriteAllLines(HighScoresFilePath, highScores.Select(entry => $"{entry.PlayerName},{entry.Score}"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving high scores: {ex.Message}");
        }
    }
}

internal class HighScoreEntry
{
    // Properties to store player name and score
    public string PlayerName { get; }
    public int Score { get; }

    // Constructor to initialize player name and score
    public HighScoreEntry(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
}
