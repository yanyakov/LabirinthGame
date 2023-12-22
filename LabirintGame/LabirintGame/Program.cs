using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabirintGame
{
    class Program
    {
        static char[,] maze;
        static int playerRow;
        static int playerCol;
        static int lives;
        static bool gameOver;

        static void Main()
        {
            while (true)
            {
                GenerateMaze();
                InitializePlayer();
                InitializeEnemies();


                while (!gameOver)
                {
                    DrawMaze();
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    UpdatePlayer(key);
                    MoveEnemies();
                    CheckCollisions();
                }

                Console.WriteLine(gameOver ? "Игра окончена!" : "Поздравляем! Вы победили!");
                Console.WriteLine("Сыграть еще раз? (Y/N)");
                if (Console.ReadKey(true).Key != ConsoleKey.Y)
                    break;
            }
        }

        static void GenerateMaze()
        {
            // Генерация проходимого лабиринта
            int rows = 10;
            int cols = 20;
            maze = new char[rows, cols];

            Random random = new Random();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    maze[row, col] = random.NextDouble() < 0.7 ? ' ' : '#';
                }
            }

            // Добавление входа и выхода из лабиринта
            maze[0, random.Next(1, cols - 1)] = 'S';
            maze[rows - 1, random.Next(1, cols - 1)] = 'E';
        }

        static void DrawMaze()
        {
            Console.Clear();
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    Console.Write(maze[row, col]);
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Lives: {lives}");
        }

        static void InitializePlayer()
        {
            // Добавление игрока в лабиринт
            lives = 3;
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    if (maze[row, col] == 'S')
                    {
                        playerRow = row;
                        playerCol = col;
                        maze[row, col] = '@';
                        return;
                    }
                }
            }
        }
       
        static void UpdatePlayer(ConsoleKeyInfo key)
        {
            // Обновление координат игрока в зависимости от нажатой клавиши
            int newRow = playerRow;
            int newCol = playerCol;

            switch (key.Key)
            {
                case ConsoleKey.W:
                    newRow--;
                    break;
                case ConsoleKey.S:
                    newRow++;
                    break;
                case ConsoleKey.A:
                    newCol--;
                    break;
                case ConsoleKey.D:
                    newCol++;
                    break;
                default:
                    return;
            }

            if (maze[newRow, newCol] == ' ')
            {
                maze[playerRow, playerCol] = ' ';
                playerRow = newRow;
                playerCol = newCol;
                maze[playerRow, playerCol] = '@';
            }
        }

        static List<int[]> enemies;

        static void InitializeEnemies()
        {
            // Инициализация врагов для лабиринта

            // Получаем список координат всех пустых ячеек
            List<int[]> emptyCells = new List<int[]>();
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    if (maze[row, col] == ' ')
                    {
                        emptyCells.Add(new int[] { row, col });
                    }
                }
            }

            // Выбираем случайные пустые ячейки для размещения врагов
            Random random = new Random();
            enemies = new List<int[]>();
            int numEnemies = Math.Min(3, emptyCells.Count); // Создаем не более 3 врагов
            for (int i = 0; i < numEnemies; i++)
            {
                int index = random.Next(0, emptyCells.Count);
                int[] coordinates = emptyCells[index];
                emptyCells.RemoveAt(index);
                enemies.Add(coordinates);
            }
        }

        static void MoveEnemies()
        {
            // Движение всех врагов

            Random random = new Random();
            int[] directions = { -1, 0, 1 }; // Возможные направления движения

            for (int i = 0; i < enemies.Count; i++)
            {
                // Выбираем случайное направление для каждого врага
                int dx = directions[random.Next(0, directions.Length)];
                int dy = directions[random.Next(0, directions.Length)];

                int[] enemyPosition = enemies[i];
                int newRow = enemyPosition[0] + dx;
                int newCol = enemyPosition[1] + dy;

                // Проверяем, чтобы враг не вышел за границы лабиринта и не перешел в стену
                if (newRow >= 0 && newRow < maze.GetLength(0) && newCol >= 0 && newCol < maze.GetLength(1) && maze[newRow, newCol] != '#')
                {
                    // Обновляем позицию врага
                    maze[enemyPosition[0], enemyPosition[1]] = ' ';
                    maze[newRow, newCol] = 'Z';
                    enemies[i] = new int[] { newRow, newCol };
                }
            }
        }

        static void CheckCollisions()
        {
            // Проверка столкновений
            if (maze[playerRow, playerCol] == 'Z')
            {
                lives--;
                if (lives == 0)
                {
                    gameOver = true;
                }
            }
         
            else if (maze[playerRow, playerCol] == 'E')
            {
                gameOver = true;
            }
        }

        

        

        
    }
}
