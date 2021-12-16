using PSG.BattlefieldAndGuns.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSG.BattlefieldAndGuns.Utility;
using UnityEngine.UI;
using System;

namespace PSG.BattlefieldAndGuns.UI
{
    public class HealthBar : MonoBehaviour
    {
        #region serialized variables

        #endregion

        #region private variables
        private Slider healthBar;
        private Enemy enemy;
        #endregion

        #region properties

        #endregion

        private void OnEnable()
        {
            enemy = this.GetComponentInParents<Enemy>();
            healthBar = GetComponent<Slider>();

            if (healthBar == null)
            {
                healthBar = gameObject.AddComponent<Slider>();
            }

            if (enemy == null)
            {
                Debug.LogError("HealthBar: Could not find an Enemy component in parents.", this);
            }
            else
            {
                healthBar.maxValue = enemy.MaxHealth;
                enemy.OnHealthChanged += Enemy_OnHealthChanged;
            }
        }

        private void OnDisable()
        {
            if(enemy != null)
                enemy.OnHealthChanged -= Enemy_OnHealthChanged;
        }

        private void Enemy_OnHealthChanged(object sender, int e)
        {
            healthBar.value = e;
        }
    }
}
