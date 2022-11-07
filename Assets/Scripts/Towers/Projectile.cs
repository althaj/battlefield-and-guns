using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class Projectile : MonoBehaviour
    {
        private Enemy target;
        private int damage;
        private bool hasTarget = false;
        private PoolManager poolManager;

        [SerializeField]
        private float speed;

        private void Start()
        {
            poolManager = FindObjectOfType<PoolManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (hasTarget)
            {
                if (target != null)
                {

                    float currentSpeed = speed * Time.deltaTime;

                    if (Vector3.Distance(transform.position, target.transform.position) < currentSpeed)
                    {
                        DestroyProjectile();
                    }
                    else
                    {
                        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
                        transform.Translate(Vector3.forward * currentSpeed);
                    }
                }
                else
                {
                    DestroyProjectile();
                }
            }
        }

        /// <summary>
        /// Set the target of the projectile.
        /// </summary>
        /// <param name="enemy"></param>
        public void Initialize(Enemy enemy, int damage)
        {
            target = enemy;
            this.damage = damage;
            hasTarget = true;
        }

        /// <summary>
        /// Deal damage and destroy the projectile.
        /// </summary>
        private void DestroyProjectile()
        {
            if (target != null)
            {
                target.DealDamage(damage);
            }
            poolManager.ReleaseBullet(gameObject);
        }
    }
}
