using System;

namespace Roguelike
{
    internal class Shooter : Enemy
    {

        private char bullet = '+';
        private int[] bulletPosition = new int[2];
        private int bulletX; 
        private int bulletY;
        private int[] shootDirection = new int[2];
        private int currentBulletDistance; // Текущая дистанция пули т.е. сколько она уже пролетела
        private int maxBulletDistance = 5; // Максимальная дистанция полета пули
        private bool thereIsAbullet = false; // Cуществует ли пуля в данный момент

        public Shooter(MazeGenerator mazeGenerator, Player player, Random random) : base(mazeGenerator, player, random)
        {
            enemySymbol = 'S';
        }

        public override void CanAttack()
        {
            if (thereIsAbullet &&
                bulletPosition[0] == player.currentPosition[0] &&
                    bulletPosition[1] == player.currentPosition[1])
                Attack();
        }

        public override void Update()
        {
            base.Update();

            if (CanShoot())
                Shoot();
            UpdateBullet();
        }

        private bool CanShoot() => !thereIsAbullet;

        private void Shoot()
        {
            GetShootDirection();
            ResetBullet();
        }

        /// Получение направления выстрела
        private void GetShootDirection() => shootDirection = directions[random.Next(directions.Count)];

        /// Пересоздание пули
        private void ResetBullet()
        {
            bulletPosition = currentPosition;
            UpdateBulletPosition();
            currentBulletDistance = 0;
            thereIsAbullet = true;
        }

        /// Перемещение пули
        private void ChangeBulletPosition()
        {
            int tempX = bulletPosition[0] + shootDirection[0];
            int tempY = bulletPosition[1] + shootDirection[1];

            if (mazeGenerator.IsInBounds(tempY, tempX) && mazeGenerator.IsWall(tempY, tempX) == false)
            {
                bulletPosition = new int[] { tempX, tempY };
                UpdateBulletPosition();
                currentBulletDistance++;
            }
            else
                DestroyBullet();
        }

        private void UpdateBulletPosition()
        {
            bulletX = bulletPosition[0];
            bulletY = bulletPosition[1];
        }

        private void DrawBullet()
        {
            Console.SetCursorPosition(bulletX, bulletY);
            Console.Write(bullet);
        }

        private void UpdateBullet()
        {
            if (currentBulletDistance >= maxBulletDistance)
                DestroyBullet();

            ChangeBulletPosition();
            if (thereIsAbullet)
                DrawBullet();
        }

        private void DestroyBullet() => thereIsAbullet = false;
    }
}
