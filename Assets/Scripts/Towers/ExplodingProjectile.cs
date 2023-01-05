using PSG.BattlefieldAndGuns.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class ExplodingProjectile : Projectile
    {
        [SerializeField] private float explosionRadius;

        protected override void DealDamage()
        {
            IEnumerable<Enemy> enemies = enemyManager.GetEnemiesInRange(transform.position, explosionRadius);
            foreach(Enemy enemy in enemies)
            {
                enemy.DealDamage(damage);
            }
        }
    }
}
