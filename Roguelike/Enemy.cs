using System;
using System.Collections.Generic;

namespace Roguelike
{
    internal class Enemy
    {
        protected char enemySymbol;
        protected ConsoleColor color = ConsoleColor.Red;

        
        protected List<int[]> directions = new List<int[]>() 
        {
            new int[] {1, 0}, // Право
            new int[] {-1, 0}, // Лево
            new int[] {0, 1}, // Низ
            new int[] {0, -1} // Верх
        };
        protected int[] currentDirection;
        protected int[] currentPosition;
        protected int currentX;
        protected int currentY;
        protected Random random;

        protected MazeGenerator mazeGenerator;
        protected Player player;
        

        public Enemy(MazeGenerator mazeGenerator, Player player,Random random)
        {
            this.random = random;
            this.mazeGenerator = mazeGenerator;
            this.player = player;

            Spawn();
            ChangeDirection();
        }


        public virtual void Update()
        {
            Move();
            PrintEnemy();
        }

        protected void PrintEnemy()
        {
            SetEnemyColor();
            SetCursorOnEnemyPosition();
            DrawEnemy();
        }

        protected void SetEnemyColor() => Console.ForegroundColor = color;

        protected void SetCursorOnEnemyPosition() => Console.SetCursorPosition(currentX, currentY);

        protected void DrawEnemy() => Console.Write(enemySymbol);

        protected virtual void Move()
        {
            int newX = currentPosition[0] + currentDirection[0];
            int newY = currentPosition[1] + currentDirection[1];

            if (mazeGenerator.IsInBounds(newY, newX))
            {
                if (mazeGenerator.IsWall(newY, newX))
                    ChangeDirection();
                else
                {
                    currentPosition = new int[] { newX, newY };
                    UpdateEnemyPosition();
                }

            }
            else
                ChangeDirection();

        }

        public void Spawn()
        {
            while (true)
            {
                int spawnX = random.Next(mazeGenerator.width);
                int spawnY = random.Next(mazeGenerator.height);

                if (mazeGenerator.IsWall(spawnX, spawnY) == false)
                {
                    this.currentPosition = new int[] { spawnY, spawnX };
                    UpdateEnemyPosition();
                    return;
                }
            }
        }
        protected virtual void ChangeDirection() => currentDirection = directions[random.Next(directions.Count)];

        protected void UpdateEnemyPosition()
        {
            currentX = currentPosition[0];
            currentY = currentPosition[1];
        }

        public void Attack() => player.GetDamage();

        public virtual void CanAttack()
        {
            if(true)
                Attack();
        }
    }
}
