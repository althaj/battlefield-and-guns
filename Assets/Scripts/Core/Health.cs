using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Core
{
    public class Health : MonoBehaviour
    {
        #region Properties

        [field: SerializeField] public int StartingHealth { get; private set; } = 10;

        #endregion

        #region Private variables

        private int currentHealth;

        #endregion

        #region Events

        public event EventHandler<int> OnHealthChanged;
        public event EventHandler OnGameOver;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            SetHealth(StartingHealth);
        }

        /// <summary>
        /// Deal damage to the player.
        /// </summary>
        /// <param name="damage">The amount of health to lose.</param>
        public void DealDamage(int damage)
        {
            SetHealth(currentHealth - damage);
        }

        private void SetHealth(int health)
        {
            currentHealth = health;
            OnHealthChanged?.Invoke(this, health);

            if (currentHealth <= 0)
                OnGameOver?.Invoke(this, null);
        }
    } 
}
