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
        private Transform cameraTransform;
        #endregion

        #region properties

        #endregion

        private void Start()
        {
            cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            if(healthBar != null)
                healthBar.transform.rotation = cameraTransform.rotation;
        }

        private void OnEnable()
        {
            enemy = this.GetComponent<Enemy>();
            healthBar = GetComponentInChildren<Slider>();

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
            if (enemy != null)
                enemy.OnHealthChanged -= Enemy_OnHealthChanged;
        }

        private void Enemy_OnHealthChanged(object sender, int e)
        {
            // First hit
            if (enemy != null && e == enemy.MaxHealth)
                healthBar.gameObject.SetActive(true);

            healthBar.value = e;
        }
    }
}
