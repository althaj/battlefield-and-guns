using PSG.BattlefieldAndGuns.Core;

namespace PSG.BattlefieldAndGuns.EnemyDebuffs
{
    public class MinorSlowEnemyDebuff : EnemyDebuff
    {
        public override void Register(Enemy enemy)
        {
            if (enemy.IsTracked)
                return;

            enemy.MultiplySpeed(0.5f);
        }

        public override void Unregister(Enemy enemy)
        {
            enemy.MultiplySpeed(null);
        }
    }
}
