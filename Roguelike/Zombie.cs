using System;

namespace Roguelike
{
    internal class Zombie : Enemy
    {
        public Zombie(MazeGenerator mazeGenerator, Player player, Random random) : base(mazeGenerator, player, random) 
        {
            enemySymbol = 'Z';
        }

        public override void CanAttack()
        {
            if (currentPosition[0] == player.currentPosition[0] &&
                   currentPosition[1] == player.currentPosition[1])
                Attack();
        }
    }
}
