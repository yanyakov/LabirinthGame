using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Roguelike
{
    internal class Player
    {
        public const char playerSymbol = '@';
        public int[] currentPosition { get; private set; }
        private int currentX;
        private int currentY;

        private MazeGenerator maze;

        private List<int[]> directions = new List<int[]>()
        {
            new int[] {1, 0}, // Право
            new int[] {-1, 0}, // Лево
            new int[] {0, 1}, // Низ
            new int[] {0, -1} // Верх
        };
        private int[] currentDirection;
        private ConsoleKeyInfo pressedKey;
        private bool canHandleInput;

        public int healthCount { get; private set; }

        public Player(MazeGenerator maze)
        {
            this.maze = maze;

            Spawn();
            currentDirection = directions[0];
            canHandleInput = true;

            healthCount = 3;

            StartHandleInput();
        }


        public void Update()
        {
            Move();
            PrintPlayer();
        }

        private void PrintPlayer()
        {
            SetPlayerColor();
            SetCursorOnPlayerPosition();
            DrawPlayer();
        }

        public void PrintOtherInformation()
        {
            PrintHealthCount();
            PrintLastPressedKey();
        }

        private void SetPlayerColor() => Console.ForegroundColor = ConsoleColor.Yellow;

        private void SetCursorOnPlayerPosition() => Console.SetCursorPosition(currentX, currentY);

        private void PrintLastPressedKey()
        {
            Console.SetCursorPosition(maze.height + 1, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Последняя нажатая кнопка: {pressedKey.KeyChar}");
        }

        private void PrintHealthCount()
        {
            Console.SetCursorPosition(maze.height + 1, 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"Количество здоровья: {healthCount}");
        }

        private void DrawPlayer() => Console.Write(playerSymbol);

        private void UpdatePlayerPosition()
        {
            currentX = currentPosition[0];
            currentY = currentPosition[1];
        }

        private void Move()
        {
            ChangeDirection();

            int newX = currentPosition[0] + currentDirection[0];
            int newY = currentPosition[1] + currentDirection[1];

            if (maze.IsInBounds(newY, newX) && maze.IsWall(newY, newX) == false)
            {
                currentPosition = new int[] { newX, newY };
                UpdatePlayerPosition();
            }
                
        }

        private void Spawn()
        {
            currentPosition = new int[] { 0, 1 };
            UpdatePlayerPosition();
        }


        /// Метод который запускает обработку ввода в другом потоке

        public void StartHandleInput()
        {
            Task.Run(() =>
            {
                while (canHandleInput)
                {
                    pressedKey = Console.ReadKey();
                }
            });
        }

        
        /// Метод для остановки обработки вывода
        public void StopHandleInput() => canHandleInput = false;


        private void ChangeDirection()
        {
            switch (pressedKey.Key)
            {
                case ConsoleKey.W:
                    currentDirection = directions[3];
                    break;
                case ConsoleKey.A:
                    currentDirection = directions[1];
                    break;
                case ConsoleKey.S:
                    currentDirection = directions[2];
                    break;
                case ConsoleKey.D:
                    currentDirection = directions[0];
                    break;
            }
        }

       
        /// Проверяем можем ли мы закончить игру
        public bool TryToWin() => maze.IsExit(currentY, currentX);

        public void GetDamage() => healthCount--;
    }
}
