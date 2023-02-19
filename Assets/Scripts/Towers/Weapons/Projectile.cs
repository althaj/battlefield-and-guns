using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private bool instantRotation = true;
        [SerializeField]

        private float angularSpeed;
        private Enemy target;
        protected int damage;
        private ProjectileType projectileType;
        private bool hasTarget = false;
        private PoolManager poolManager;

        private Vector3? lastTargetPosition;

        protected EnemyManager enemyManager;

        private void Start()
        {
            poolManager = FindObjectOfType<PoolManager>();

            enemyManager = FindObjectOfType<EnemyManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (hasTarget)
            {
                if (target != null)
                {
                    lastTargetPosition = target.transform.position;
                }

                float currentSpeed = speed * Time.deltaTime;

                if (!lastTargetPosition.HasValue || Vector3.Distance(transform.position, lastTargetPosition.Value) < currentSpeed)
                {
                    DestroyProjectile();
                }
                else
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lastTargetPosition.Value - transform.position);

                    if (instantRotation || Vector3.Distance(transform.position, lastTargetPosition.Value) < 1f)
                        transform.rotation = targetRotation;
                    else
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, angularSpeed * Time.deltaTime);

                    transform.Translate(Vector3.forward * currentSpeed);
                }
            }
        }

        /// <summary>
        /// Set the target of the projectile.
        /// </summary>
        /// <param name="enemy"></param>
        public void Initialize(Enemy enemy, int damage, ProjectileType projectileType)
        {
            target = enemy;
            this.damage = damage;
            this.projectileType = projectileType;
            hasTarget = true;
        }

        /// <summary>
        /// Deal damage and destroy the projectile.
        /// </summary>
        private void DestroyProjectile()
        {
            DealDamage();

            target = null;
            hasTarget = false;
            lastTargetPosition = null;

            poolManager.ReleaseProjectile(projectileType, gameObject);
        }

        protected virtual void DealDamage()
        {
            if (target != null)
            {
                target.DealDamage(damage);
            }
        }
    }
}
