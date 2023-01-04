using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Managers;
using PSG.BattlefieldAndGuns.PSGCamera;
using PSG.BattlefieldAndGuns.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.BattlefieldAndGuns.UI
{
    [RequireComponent(typeof(LineRenderer))]
    public class UpgradePopup : MonoBehaviour
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

        private CameraController cameraController;
        private Vector2 panelSize;
        private Vector3 targetPosition;

        private LineRenderer lineRenderer;

        bool isInitialized = false;

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

        private void FixedUpdate()
        {
            if (tower != null)
            {
                targetPosition = UIUtility.GetPanelPositionFromWorldPosition(mainCamera, anchorWorldPosition, panelSize);
                UpdatePosition(false);
            }
        }

        private void Initialize()
        {
            texts = GetComponentsInChildren<Text>();
            panel = transform.GetChild(0).gameObject;
            upgradeButton = GetComponentInChildren<Button>();

            towerManager = FindObjectOfType<TowerManager>();

            rectTransform = GetComponent<RectTransform>();
            mainCamera = Camera.main;
            cameraController = mainCamera.GetComponent<CameraController>();

            GetPanelSize();

            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false;

            isInitialized = true;
        }

        private void Update()
        {
            UpdatePosition(false);
        }

        /// <summary>
        /// Show the upgrade panel. Fills the text / data of the tower, hooks to events, sets the panel active.
        /// </summary>
        /// <param name="tower"></param>
        /// <param name="towerData"></param>
        public void Show(Tower tower, TowerData towerData)
        {
            if (!isInitialized)
                Initialize();

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

            gameObject.SetActive(true);

            towerManager.OnMoneyChanged += TowerManager_OnMoneyChanged;
            SetButtonEnabled();

            anchorWorldPosition = tower.transform.position + Vector3.up * 3;

            // Get position, hook to camera events
            UpdatePosition(true);

            lineRenderer.enabled = true;

            towerManager.ShowRangeIndicator(tower.transform.position, towerData.GetRange(tower.Level + 1));
        }

        /// <summary>
        /// Upgrade the tower.
        /// </summary>
        public void Upgrade()
        {
            tower?.LevelUp();
            GameUI.HideUpgradePanel();
        }

        /// <summary>
        /// Hide the panel.
        /// </summary>
        public void Hide()
        {
            if (gameObject.activeSelf)
            {
                towerManager.OnMoneyChanged -= TowerManager_OnMoneyChanged;
                tower = null;
                towerData = null;
                gameObject.SetActive(false);

                lineRenderer.enabled = false;

                towerManager.HideRangeIndicator();
            }
        }

        /// <summary>
        /// Called whenever the amount of money is changed as to check the avaibility of the upgrade button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TowerManager_OnMoneyChanged(object sender, int e)
        {
            SetButtonEnabled();
        }

        /// <summary>
        /// Set the interactibility of the upgrade button.
        /// </summary>
        private void SetButtonEnabled()
        {
            upgradeButton.interactable = CanUpgrade;
        }

        /// <summary>
        /// Gets the size of the panel.
        /// </summary>
        /// <remarks>Probably should be called whenever we change the screen size.</remarks>
        private void GetPanelSize()
        {
            panelSize = UIUtility.GetPanelSize(rectTransform);
        }

        /// <summary>
        /// Updates the position of the panel.
        /// </summary>
        /// <param name="instant">Whether the position is updated instantly or is lerped over a short period of time.</param>
        private void UpdatePosition(bool instant)
        {
            if (tower != null)
            {
                if (instant)
                    rectTransform.position = targetPosition;
                else
                    rectTransform.position = Vector3.Lerp(rectTransform.position, targetPosition, 0.04f);

                lineRenderer.SetPosition(0, mainCamera.ScreenToWorldPoint(rectTransform.position));
                lineRenderer.SetPosition(1, tower.transform.position);
            }
        }
    }
}
