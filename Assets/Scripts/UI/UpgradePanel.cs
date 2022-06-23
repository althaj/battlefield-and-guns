using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.BattlefieldAndGuns.UI
{
    public class UpgradePanel : MonoBehaviour
    {
        #region serialized variables

        #endregion

        #region private variables

        private Tower tower;
        private TowerData towerData;

        private Text[] texts;
        private GameObject panel;

        private Button upgradeButton;

        private TowerManager towerManager;
        private GameUI gameUI;

        private RectTransform rectTransform;
        private Camera mainCamera;
        private Vector3 anchorWorldPosition;

        #endregion

        #region properties

        private TowerManager TowerManager
        {
            get
            {
                if (towerManager == null)
                    towerManager = FindObjectOfType<TowerManager>();

                return towerManager;
            }
        }

        private GameUI GameUI
        {
            get
            {
                if (gameUI == null)
                    gameUI = FindObjectOfType<GameUI>();

                return gameUI;
            }
        }

        private bool CanUpgrade
        {
            get
            {
                if (towerData == null)
                    return false;

                return TowerManager.Money >= towerData.GetCost(tower.Level + 1);
            }
        }

        #endregion

        private void Start()
        {
            texts = GetComponentsInChildren<Text>();
            panel = transform.GetChild(0).gameObject;
            upgradeButton = GetComponentInChildren<Button>();

            panel.SetActive(false);

            towerManager = FindObjectOfType<TowerManager>();

            rectTransform = GetComponent<RectTransform>();
            mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (tower != null)
            {
                rectTransform.position = mainCamera.WorldToScreenPoint(anchorWorldPosition);
            }
        }

        public void Show(Tower tower, TowerData towerData)
        {
            this.tower = tower;
            this.towerData = towerData;

            try
            {
                texts[0].text = $"{towerData.Title} level {tower.Level}";
                texts[1].text = $"Cost: {towerData.GetCost(tower.Level)} > {towerData.GetCost(tower.Level + 1)}";
                texts[2].text = $"Range: {towerData.GetRange(tower.Level)} > {towerData.GetRange(tower.Level + 1)}";
                texts[3].text = $"Damage: {towerData.GetDamage(tower.Level)} > {towerData.GetDamage(tower.Level + 1)}";
                texts[4].text = $"Fire rate: {towerData.GetFireRate(tower.Level)} > {towerData.GetFireRate(tower.Level + 1)}";
                texts[5].text = $"Build time: {towerData.GetBuildTime(tower.Level)}s > {towerData.GetBuildTime(tower.Level + 1)}s";
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            towerManager.OnMoneyChanged += TowerManager_OnMoneyChanged;
            SetButtonEnabled();

            anchorWorldPosition = tower.transform.position + Vector3.up * 2 + Vector3.forward * 2;

            panel.SetActive(true);
        }

        public void Upgrade()
        {
            tower?.LevelUp();
            GameUI.HideUpgradePanel();
        }

        public void Hide()
        {
            towerManager.OnMoneyChanged -= TowerManager_OnMoneyChanged;
            tower = null;
            towerData = null;
            panel.SetActive(false);
        }

        private void TowerManager_OnMoneyChanged(object sender, int e)
        {
            SetButtonEnabled();
        }

        private void SetButtonEnabled()
        {
            upgradeButton.interactable = CanUpgrade;
        }
    }
}
