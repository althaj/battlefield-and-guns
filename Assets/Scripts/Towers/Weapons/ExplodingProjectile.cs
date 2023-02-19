using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class ExplodingProjectile : Projectile
    {
        [SerializeField] private float explosionRadius;

        [SerializeField] private ExplosionType explosionType;

        protected override void DealDamage()
        {
            IEnumerable<Enemy> enemies = enemyManager.GetEnemiesInRange(transform.position, explosionRadius);
            foreach(Enemy enemy in enemies.ToList())
            {
                enemy.DealDamage(damage);
            }

            PoolManager.Instance.CreateExplosion(explosionType, transform.position);
        }
    }
}
