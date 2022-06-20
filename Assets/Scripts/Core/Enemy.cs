using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PSG.BattlefieldAndGuns.Managers;

namespace PSG.BattlefieldAndGuns.Core
{
    public class Enemy : MonoBehaviour
    {
        #region Properties
        public int MaxHealth { get => maxHealth; }
        public int Strength { get => strength; }
        public int Reward { get => reward; }
        public EnemyManager EnemyManager { get; set; }
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
        private NavMeshAgent navMeshAgent;

        private Vector3 destination;
        #endregion

        #region Events
        public event EventHandler<int> OnHealthChanged;
        #endregion

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();

            if(navMeshAgent == null)
            {
                navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            }

            navMeshAgent.speed = speed;
            navMeshAgent.radius = radius;
            navMeshAgent.angularSpeed = angularSpeed;

            destination = GameObject.FindGameObjectWithTag("End").transform.position;
            navMeshAgent.SetDestination(destination);

            health = maxHealth;
            OnHealthChanged?.Invoke(this, health);
        }

        private void Update()
        {
            if(Vector3.Distance(transform.position, destination) < 0.5f)
            {
                FindObjectOfType<Health>().DealDamage(damage);

                FindObjectOfType<EnemyManager>().RemoveEnemy(this);
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

            if(health <= 0)
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
    }

}