using Cells.Model;
using Interfaces;
using PcMan.Model.Characters;
using PcMan.View;

using Cells.Model;
using Interfaces;
using PcMan.Model.Characters;
using PcMan.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace PcMan.Model
{
    internal class Game
    {
        private static Game CurrentGame;
        private Random random;
        private KeyboardController keyboardController;
        private KeyboardController2 keyboardController2;
        private Scoreboard scoreboard;
        private HighScoreManager highScoreManager;
        private int gameSpeed;
        private bool isRunning;
        private DateTime endTime;
        private DateTime startTime;
        private TimeSpan timeElapsed;
        private int currentLevel = 0;
        private int scoretoWinLevel;
        private int scoreThisLevel;
        private Player player;
        private Player2 player2;
        private Portal portal;
        private List<Portal> portals;
        private List<Life> lives;
        private List<Coin> coins;
        private List<IUpdatable> updatables;
        private List<Character> enemies;
        private int width;
        private int height;
        private int score = 0;
        private int score2 = 0;
        private int life = 0;
        private Cell[,] cells;

        public Game()
        {
            SetupCell();
            keyboardController = new KeyboardController();
            keyboardController2 = new KeyboardController2();
            gameSpeed = 1;
            isRunning = true;
            random = new Random();
            CurrentGame = this;
            scoreboard = new Scoreboard(width + 3, 8);
            highScoreManager = new HighScoreManager();
            lives = new List<Life>();
            for (int numLives = 0; numLives < 4; numLives++)
                lives.Add(new Life());

            coins = new List<Coin>();
            for (int numCoins = 0; numCoins < 4; numCoins++)
                coins.Add(new Coin());

            portal = new Portal();
            updatables = new List<IUpdatable>();
            updatables.Add(new Bouncer(15, 15));
            updatables.Add(new RandomEnemy(15, 10));
            updatables.Add(new Teleporter(25, 25));
            updatables.Add(new Player(10, 10, keyboardController));
            updatables.Add(new Player2(20, 20, keyboardController2));
            updatables.Add(keyboardController);
            updatables.Add(keyboardController2);

            EnemyDamagePlayer();
            SetupLevel();
        }

        // Main method to control the game flow.
        public void Play()
        {
            // Loop that runs while the game is running.
            while (isRunning)
            {
                // Update input and position for player 1.
                keyboardController.Update(timeElapsed);
                UpdatePlayer(player);

                // Update input and position for player 2.
                keyboardController2.Update(timeElapsed);
                UpdatePlayer(player2);

                // Update other game elements.
                Update();

                // Check if players' lives are depleted, display game over message, and handle high scores.
                if (life < 0)
                {
                    // Display game over message.
                    Console.Clear();
                    string gameSummary = $"Game Over! Score Player 1 :{GetScore()} Score Player 2 :{GetScore2()}";
                    Console.SetCursorPosition(30, 15);
                    foreach (char letter in gameSummary)
                    {
                        Console.Write(letter);
                        Thread.Sleep(70);
                    }
                    Thread.Sleep(100);
                    Console.Clear();
                    // Check if the total score is greater than the lowest score in the highscore.txt
                    if (GetScore() + GetScore2() > highScoreManager.GetLowestScore())
                    {
                        // Prompt the user for their name and add their score to the high scores.
                        string highScoreMessageDeath = "You've beaten a highscore! Enter your name to save your progress: ";
                        Console.SetCursorPosition(30, 15);
                        foreach (char letter in highScoreMessageDeath)
                        {
                            Console.Write(letter);
                            Thread.Sleep(70);
                        }
                        Thread.Sleep(1000);
                        string playerName = Console.ReadLine();
                        Thread.Sleep(1000);
                        Console.Clear();
                        string savedMessage = "Progress saved!";
                        Console.SetCursorPosition(30, 15);
                        foreach (char letter in savedMessage)
                        {
                            Console.Write(letter);
                            Thread.Sleep(70);
                        }
                        Thread.Sleep(1000);
                        highScoreManager.AddHighScore(playerName, GetScore() + GetScore2());
                    }

                    // Display high scores and wait for user input.
                    highScoreManager.DisplayHighScores();
                    Console.ReadKey();
                    isRunning = false;
                }
            }
        }

        // Method to update the position and input of a player.
        private void UpdatePlayer(IUpdatable player)
        {
            foreach (IUpdatable updatable in updatables)
                if (updatable == player)
                    updatable.Update(timeElapsed);
        }

        // Main update method for the game.
        public void Update()
        {
            // Update time elapsed for the game.
            timeElapsed = endTime - startTime;
            startTime = DateTime.Now;
            timeElapsed *= gameSpeed;

            // Update input for both players.
            keyboardController.Update(timeElapsed);
            keyboardController2.Update(timeElapsed);

            // Update all updatable game elements.
            foreach (IUpdatable updatable in updatables)
                updatable.Update(timeElapsed);

            endTime = DateTime.Now;

            // Check if the current score meets the level completion criteria.
            if (scoreThisLevel >= scoretoWinLevel)
                SetupLevel();

            scoreThisLevel = GetScore() + GetScore2();
        }

        // Method to retrieve the current instance of the Game class.
        public static Game GetGame()
        {
            return Game.CurrentGame;
        }

        // Getter method for the game height.
        public int GetHeight()
        {
            return height;
        }

        // Getter method for the game width.
        public int GetWidth()
        {
            return width;
        }

        // Method to generate a random integer between two values.
        public int RandomBetween(int a, int b)
        {
            return random.Next(a, b);
        }

        // Method to generate a random float between two values.
        public float RandomBetween(float a, float b)
        {
            return (float)random.NextDouble() * (b - a) + a;
        }

        // Method to check if a cell exists at the specified coordinates.
        public bool CellExists(int top, int left)
        {
            if (left >= 0 && left < width && top >= 0 && top < height)
                if (cells[top, left] != null)
                    return true;

            return false;
        }

        // Method to get the cell at the specified coordinates.
        public Cell GetCell(int top, int left)
        {
            if (CellExists(top, left))
                return cells[top, left];

            throw new Exception("Cell does not exist");
        }

        // Method to get the coordinates of player 1.
        public (int left, int top) GetPlayer()
        {
            return (player.GetLeft(), player.GetTop());
        }

        // Method to get the coordinates of player 2.
        public (int left, int top) GetPlayer2()
        {
            return (player2.GetLeft(), player2.GetTop());
        }

        // Method to increase the score of player 1.
        public void IncreaseScore(int amount)
        {
            score += amount;
            scoreboard.Counters();
        }

        // Method to increase the score of player 2.
        public void IncreaseScore2(int amount)
        {
            score2 += amount;
            scoreboard.Counters();
        }

        // Method to decrease the score of player 1.
        public void DecreaseScore(int amount)
        {
            score -= amount;
            scoreboard.Counters();
        }

        // Method to decrease the score of player 2.
        public void DecreaseScore2(int amount)
        {
            score2 -= amount;
            scoreboard.Counters();
        }

        // Method to reset the scores of both players.
        public void ResetScore()
        {
            score = 0;
            score2 = 0;
        }

        // Getter method for the score of player 1.
        public int GetScore()
        {
            return score;
        }

        // Getter method for the score of player 2.
        public int GetScore2()
        {
            return score2;
        }

        // Getter method for the current level.
        public int GetCurrentLevel()
        {
            return currentLevel;
        }

        // Getter method for the score required to win the current level.
        public int GetScoreToWin()
        {
            return scoretoWinLevel;
        }

        // Method to increase the life count.
        public void IncreaseLife(int amount)
        {
            life += amount;
            scoreboard.Counters();
        }

        // Method to decrease the life count.
        public void DecreaseLife(int amount)
        {
            life -= amount;
            scoreboard.Counters();
        }

        // Method to reset the life count.
        public void ResetLife()
        {
            life = 0;
        }

        // Getter method for the current life count.
        public int GetLife()
        {
            return life;
        }

        // Method to identify and store all enemies in the game.
        public void EnemyDamagePlayer()
        {
            player = (Player)updatables[3];
            player2 = (Player2)updatables[4];
            enemies = new List<Character>();
            foreach (Character enemy in updatables.OfType<Character>())
                if (enemy != player && enemy != player2)
                    enemies.Add(enemy);
        }

        // Method to check for collisions between players and enemies.
        public bool CheckCollision()
        {
            foreach (Character enemy in enemies)
                if (player.GetLeft() == enemy.GetLeft() && player.GetTop() == enemy.GetTop() || player2.GetLeft() == enemy.GetLeft() && player2.GetTop() == enemy.GetTop())
                    return true;

            return false;
        }

        // Method to reset the game level.
        public void ResetLevel()
        {
            updatables = new List<IUpdatable>();
        }

        // Method to initialize the game cell grid.
        public void SetupCell()
        {
            height = 30;
            width = 70;
            cells = new Cell[height + 1, width + 1];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    cells[i, j] = new Cell(i, j);
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
                        cells[i, j].MakeWall();
                }
        }

        // Method to set up a new game level.
        public void SetupLevel()
        {
            if (currentLevel < 5)
            {
                currentLevel++;

                // Display loading message.
                Console.Clear();
                Console.SetCursorPosition(30, 15);
                Console.WriteLine("Loading level...");
                Thread.Sleep(1000);

                // Update score display and reset game elements for the new level.
                scoreboard.Counters();
                SetupCell();
                ResetLevel();
                SetupEntitiesForCurrentLevel();
            }
            else
            {
                currentLevel++;
                Console.Clear();
                SetupEntitiesForCurrentLevel();
            }
        }

        // Method to set up entities based on the current game level.
        private void SetupEntitiesForCurrentLevel()
        {
            {
                switch (currentLevel)
                {
                    case 1:
                        updatables.Add(new Bouncer(25, 25));
                        updatables.Add(new RandomEnemy(23, 10));
                        updatables.Add(new Teleporter(15, 25));
                        updatables.Add(new Player(20, 20, keyboardController));
                        updatables.Add(new Player2(10, 20, keyboardController2));
                        portal = new Portal();
                        portal.Refresh();
                        EnemyDamagePlayer();
         
                        lives = new List<Life>();
                        for (int numLives = 0; numLives < 4; numLives++)
                            lives.Add(new Life());

                        coins = new List<Coin>();
                        for (int numCoins = 0; numCoins < 4; numCoins++)
                            coins.Add(new Coin());

                        scoretoWinLevel = 5;
                        break;

                    case 2:
                        updatables.Add(new Bouncer(10, 15));
                        updatables.Add(new RandomEnemy(29, 5));
                        updatables.Add(new Teleporter(5, 20));
                        updatables.Add(new Player(15, 10, keyboardController));
                        updatables.Add(new Player2(10, 20, keyboardController2));
                        portal = new Portal();
                        portal.Refresh();
                        EnemyDamagePlayer();
                        lives = new List<Life>();
                        for (int numLives = 0; numLives < 4; numLives++)
                            lives.Add(new Life());

                        coins = new List<Coin>();
                        for (int numCoins = 0; numCoins < 4; numCoins++)
                            coins.Add(new Coin());
                        scoretoWinLevel = 10;
                        break;

                    case 3:
                        updatables.Add(new Bouncer(18, 22));
                        updatables.Add(new RandomEnemy(12, 5));
                        updatables.Add(new Teleporter(25, 15));
                        updatables.Add(new Player(29, 18, keyboardController));
                        updatables.Add(new Player2(10, 20, keyboardController2));
                        portal = new Portal();
                        portal.Refresh();
                        EnemyDamagePlayer();
                        lives = new List<Life>();
                        for (int numLives = 0; numLives < 4; numLives++)
                            lives.Add(new Life());

                        coins = new List<Coin>();
                        for (int numCoins = 0; numCoins < 4; numCoins++)
                            coins.Add(new Coin());
                        scoretoWinLevel = 15;
                        break;

                    case 4:
                        updatables.Add(new Bouncer(29, 12));
                        updatables.Add(new RandomEnemy(20, 25));
                        updatables.Add(new Teleporter(8, 29));
                        updatables.Add(new Player(5, 8, keyboardController));
                        updatables.Add(new Player2(10, 20, keyboardController2));
                        portal = new Portal();
                        portal.Refresh();
                        EnemyDamagePlayer();
                        lives = new List<Life>();
                        for (int numLives = 0; numLives < 4; numLives++)
                            lives.Add(new Life());

                        coins = new List<Coin>();
                        for (int numCoins = 0; numCoins < 4; numCoins++)
                            coins.Add(new Coin());
                        scoretoWinLevel = 20;
                        break;

                    case 5:
                        updatables.Add(new Bouncer(15, 10));
                        updatables.Add(new RandomEnemy(22, 20));
                        updatables.Add(new Teleporter(29, 5));
                        updatables.Add(new Player(25, 30, keyboardController));
                        updatables.Add(new Player2(10, 20, keyboardController2));
                        portal = new Portal();
                        portal.Refresh();
                        EnemyDamagePlayer();
                        lives = new List<Life>();
                        for (int numLives = 0; numLives < 4; numLives++)
                            lives.Add(new Life());

                        coins = new List<Coin>();
                        for (int numCoins = 0; numCoins < 4; numCoins++)
                            coins.Add(new Coin());
                        scoretoWinLevel = 30;
                        break;
                    case 6:
                        Console.Clear();
                        string endMessage = "Congrationlations! You've beaten the game!";
                        Console.SetCursorPosition(30, 15);
                        foreach (char letter in endMessage)
                        {
                            Console.Write(letter);
                            Thread.Sleep(70);
                        }
                        Thread.Sleep(1000);
                        Console.Clear();
                        if (GetScore() + GetScore2() > highScoreManager.GetLowestScore())
                        {
                            // Prompt the user for their name and add their score to the high scores.
                            string highScoreMessage = $"You've also beaten a highscore! Enter your name to save your progress: ";
                            Console.SetCursorPosition(30, 15);
                            foreach (char letter in highScoreMessage)
                            {
                                Console.Write(letter);
                                Thread.Sleep(70);
                            }
                            string playerName = Console.ReadLine();
                            Thread.Sleep(1000);
                            Console.Clear();
                            string savedMessage = "Progress saved!";
                            Console.SetCursorPosition(30, 15);
                            foreach (char letter in savedMessage)
                            {
                                Console.Write(letter);
                                Thread.Sleep(70);
                            }
                            highScoreManager.AddHighScore(playerName, GetScore() + GetScore2());
                        }
                        Thread.Sleep(1000);
                        // Display high scores and wait for user input.
                        highScoreManager.DisplayHighScores();
                        Console.ReadKey();
                        isRunning = false;
                        break;

                }
            }
        }
    }
}
