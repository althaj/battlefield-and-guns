using PSG.BattlefieldAndGuns.Core;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.BattlefieldAndGuns.UI
{
    public class HealthBar : MonoBehaviour
    {
        #region serialized variables
        [SerializeField] private GameObject healthBarCanvasPrefab;
        #endregion

        #region private variables
        private Slider healthBar;
        private Enemy enemy;
        private Transform cameraTransform;

        private Transform canvas;
        private Transform iconPanel;
        #endregion

        #region properties

        #endregion

        private void Start()
        {
            cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            if(canvas != null)
                canvas.rotation = cameraTransform.rotation;
        }

        private void OnEnable()
        {
            enemy = this.GetComponent<Enemy>();

            if(healthBar == null)
            {
                canvas = Instantiate(healthBarCanvasPrefab, transform).transform;
                healthBar = canvas.GetComponentInChildren<Slider>();
                iconPanel = canvas.GetChild(0);

                if(enemy.IsTracked)
                    iconPanel.GetChild(0).gameObject.SetActive(true);
            }

            if (enemy == null)
            {
                Debug.LogError("HealthBar: Could not find an Enemy component in parents.", this);
            }
            else
            {
                healthBar.maxValue = enemy.MaxHealth;
                enemy.OnHealthChanged += Enemy_OnHealthChanged;

                Enemy_OnHealthChanged(enemy, enemy.CurrentHealth);
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
            if (enemy != null)
                healthBar.gameObject.SetActive(e != enemy.MaxHealth);

            healthBar.value = e;
        }
    }
}
