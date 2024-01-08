using System;
using System.Collections.Generic;
using System.Threading;

namespace Roguelike
{
    public enum GameResult
    {
        Win,
        Lose
    }

    internal class Program
    {
        private static int mapWidth;
        private static int mapHeight;
        private static MazeGenerator mazeGenerator;

        private static bool isGameOver;
        private static int frameUpdateTime = 500;
        private static Random random = new Random();

        private static Player player;

        private static List<Enemy> enemies;

        private static void InitializeGame()
        {
            isGameOver = false;
            Console.CursorVisible = false;

            InitializeMazeGenerator();

            player = new Player(mazeGenerator);
            InitializeEnemies();
        }

        private static void GetInput()
        {
            Console.Write("Введите ширину лабиринта (по умолчанию 10): ");
            if (int.TryParse(Console.ReadLine(), out mapWidth) == false)
                mapWidth = 10;

            Console.Write("Введите высоту лабиринта (по умолчанию 10): ");
            if (int.TryParse(Console.ReadLine(), out mapHeight) == false)
                mapHeight = 10;
        }

        private static void InitializeMazeGenerator()
        {
            GetInput();
            mazeGenerator = new MazeGenerator(mapWidth, mapHeight, random);
            mazeGenerator.GenerateMaze();
        }


        private static void InitializeEnemies()
        {
            enemies = new List<Enemy>()
            {
                new Shooter(mazeGenerator,player,random),
                new Shooter(mazeGenerator,player,random),
                new Zombie(mazeGenerator,player,random),
                new Zombie(mazeGenerator,player,random),
            };
        }

        private static void RestartGame()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            InitializeGame();
        }

    
        private static void EndGame(GameResult gameResult)
        {
            Console.Clear();

            player.StopHandleInput();
            isGameOver = true;

            if (gameResult == GameResult.Win)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Ты добрался до выхода, поздравляем с победой!!!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ты не добрался до выхода, это поражение");
            }

            Console.WriteLine("Нажми любую клавишу для продолжения...");
            Console.ReadKey();
            RestartGame();
        }

        private static void ClearFrame()
        {
            Thread.Sleep(frameUpdateTime);
            Console.Clear();
        }

        private static void UpdateEnemies()
        {
            foreach (Enemy enemy in enemies)
                enemy.Update();
        }

        private static void CheckDamage()
        {
            foreach (Enemy enemy in enemies)
                enemy.CanAttack();
        }
        private static void UpdateFrame()
        {
            mazeGenerator.PrintMaze(); 

            UpdateEnemies();
            player.Update();

            CheckDamage();
            player.PrintOtherInformation();

            TryToGetResultOfGame();
        }

        private static void TryToGetResultOfGame()
        {
            if (player.healthCount <= 0)
                EndGame(GameResult.Lose);
            else if (player.TryToWin())
                EndGame(GameResult.Win);
        }

        static void Main()
        {
            InitializeGame();

            while (isGameOver == false)
            {
                ClearFrame();

                UpdateFrame();
            }
        }
    }
}
