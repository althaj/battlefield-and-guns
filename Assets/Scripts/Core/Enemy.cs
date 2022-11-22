using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PSG.BattlefieldAndGuns.Managers;
using PSG.RNG;
using PSG.BattlefieldAndGuns.Utility;

namespace PSG.BattlefieldAndGuns.Core
{
    public class Enemy : MonoBehaviour
    {
        #region Properties
        public int MaxHealth { get => maxHealth; }
        public int Strength { get => strength; }
        public int Reward { get => reward; }
        public EnemyManager EnemyManager { get; set; }

        private Vector3[] path;
        public Vector3[] Path
        {
            get => path;
            set
            {
                path = value;
                NavMeshAgent.SetDestination(Path[currentPathIndex]);
            }
        }

        private NavMeshAgent navMeshAgent;
        private NavMeshAgent NavMeshAgent
        {
            get
            {
                if (navMeshAgent == null)
                {
                    navMeshAgent = GetComponent<NavMeshAgent>();

                    if (navMeshAgent == null)
                    {
                        navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
                    }
                }

                return navMeshAgent;
            }
        }

        #endregion

        #region Serialized fields
        [SerializeField]
        private float speed = 2;
        [SerializeField]
        private float angularSpeed = 180;
        [SerializeField]
        private float radius = 0.4f;
        [SerializeField]
        private int maxHealth;
        [SerializeField]
        private int strength;
        [SerializeField]
        private int reward;
        [SerializeField]
        private int damage = 1;
        #endregion

        #region Private variables
        private int health;

        private int currentPathIndex;
        private NavMeshHit hit;

        private Vector3? nextPoint;
        #endregion

        #region Events
        public event EventHandler<int> OnHealthChanged;
        #endregion

        private void Start()
        {
            NavMeshAgent.updateRotation = false;

            NavMeshAgent.speed = speed * RNGManager.Manager[Constants.ENEMY_MANAGER_RNG_TITLE].NextFloat(0.9f, 1.1f);
            NavMeshAgent.angularSpeed = angularSpeed;
            NavMeshAgent.radius = radius;
            NavMeshAgent.autoBraking = false;

            NavMeshAgent.acceleration = 18f;
            NavMeshAgent.height = 0.1f;

            health = maxHealth;
            OnHealthChanged?.Invoke(this, health);
        }

        private void Update()
        {
            if (Path != null && Path.Length > currentPathIndex)
            {
                if (Vector3.Distance(transform.position, Path[currentPathIndex]) < 0.2f)
                {
                    if (Path.Length == currentPathIndex - 1)
                    {
                        FindObjectOfType<Health>().DealDamage(damage);
                        FindObjectOfType<EnemyManager>().RemoveEnemy(this);
                    }
                    else
                    {
                        currentPathIndex++;
                        NavMeshAgent.SetDestination(Path[currentPathIndex]);
                    }
                }

                Quaternion lookOnAngle = Quaternion.LookRotation(Path[currentPathIndex] - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookOnAngle, angularSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Deal damage to the enemy.
        /// </summary>
        /// <param name="damage">Amount of damage to deal.</param>
        public void DealDamage(int damage)
        {
            health -= damage;

            OnHealthChanged?.Invoke(this, health);

            if (health <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Death of the enemy.
        /// </summary>
        public void Die()
        {
            if (this.EnemyManager != null)
                this.EnemyManager.KillEnemy(this);
        }

        /// <summary>
        /// Apply buffs to this enemy.
        /// </summary>
        /// <param name="healthMultiplier">Multiplier of the max health.</param>
        /// <param name="speedMultiplier">Multiplier of the speed.</param>
        /// <param name="rewardMultiplier">Multiplier of the reward.</param>
        public void ApplyBuffs(float healthMultiplier, float speedMultiplier, float rewardMultiplier)
        {
            // Health has to be at least 1
            maxHealth = (int)Mathf.Max(maxHealth * healthMultiplier, 1);
            health = maxHealth;
            // Speed has to be at least 0.1 or whatever
            speed = Mathf.Max(speed * speedMultiplier, 0.1f);
            // Reward cannot be negative
            reward = (int)Mathf.Max(reward * rewardMultiplier, 0);
        }

        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            if (nextPoint.HasValue)
            {
                Gizmos.DrawLine(transform.position, nextPoint.Value);
                Gizmos.DrawSphere(nextPoint.Value, 0.1f);
            }
        }
    }

}