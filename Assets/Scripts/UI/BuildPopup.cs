using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Managers;
using PSG.BattlefieldAndGuns.Towers;
using PSG.BattlefieldAndGuns.Utility;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.BattlefieldAndGuns.UI
{
    public class BuildPopup : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonPrefab;

        [SerializeField]
        private Transform towersPanel;

        [SerializeField]
        private GameObject textPanel;

        private List<GameObject> towers = new List<GameObject>();
        private int currentTowerIndex;
        private Text[] texts;
        private TowerSpace towerSpace;
        private RectTransform rectTransform;
        private TowerManager towerManager;
        private GameUI gameUI;
        private Button buildButton;

        private LineRenderer lineRenderer;
        private Camera mainCamera;
        private Vector3 anchorWorldPosition;
        private bool isInitialized = false;

        private bool CanBuild
        {
            get
            {
                if (currentTowerIndex < 0)
                    return false;

                return towerManager.Money >= towers[currentTowerIndex].GetComponent<Tower>().Cost;
            }
        }

        private void Update()
        {
            UpdatePosition(false);
        }

        private void UpdatePosition(bool isInstant)
        {
            var targetPosition = UIUtility.GetPanelPositionFromWorldPosition(mainCamera, anchorWorldPosition, UIUtility.GetPanelSize(rectTransform));

            if(isInstant)
                rectTransform.position = targetPosition;
            else
                rectTransform.position = Vector3.Lerp(rectTransform.position, targetPosition, 0.04f);

            lineRenderer.SetPosition(0, mainCamera.ScreenToWorldPoint(rectTransform.position));
            lineRenderer.SetPosition(1, towerSpace.transform.position);
        }

        private void Initialize()
        {
            texts = textPanel.GetComponentsInChildren<Text>();
            rectTransform = GetComponent<RectTransform>();

            lineRenderer = GetComponent<LineRenderer>();

            mainCamera = Camera.main;
            towerManager = FindObjectOfType<TowerManager>();
            gameUI = FindObjectOfType<GameUI>();
            buildButton = textPanel.GetComponentInChildren<Button>();

            isInitialized = true;
        }

        public void AddTowerButton(GameObject tower)
        {
            towers.Add(tower);

            GameObject newTowerObject = Instantiate(buttonPrefab, towersPanel);
            TowerButton button = newTowerObject.GetComponent<TowerButton>();
            button.Initialize(tower.GetComponent<Tower>().TowerData.Title, towers.Count - 1, this);
        }

        public void SelectTower(int index)
        {
            textPanel.SetActive(true);

            TowerData towerData = towers[index].GetComponent<Tower>().TowerData;

            texts[0].text = $"{towerData.Title}";
            texts[1].text = $"Cost: {towerData.GetCost(1)}";
            texts[2].text = $"Range: {towerData.GetRange(1)}";
            texts[3].text = $"Damage: {towerData.GetDamage(1)}";
            texts[4].text = $"Fire rate: {towerData.GetFireRate(1)}";
            texts[5].text = $"Build time: {towerData.GetBuildTime(1)}";

            currentTowerIndex = index;

            UpdateButtonInteractable();

            towerManager.ShowRangeIndicator(towerSpace.transform.position, towerData.GetRange(1));
        }

        public void BuildTower()
        {
            towerManager.BuildTower(towerSpace, towers[currentTowerIndex]);
            gameUI.HideBuildPopup();
        }

        public void Show(TowerSpace towerSpace)
        {
            if (!isInitialized)
                Initialize();

            gameObject.SetActive(true);
            textPanel.SetActive(false);

            this.towerSpace = towerSpace;

            currentTowerIndex = -1;

            anchorWorldPosition = towerSpace.transform.position + Vector3.up * 2;

            towerManager.OnMoneyChanged += TowerManager_OnMoneyChanged;

            UpdatePosition(true);
        }

        private void TowerManager_OnMoneyChanged(object sender, int e)
        {
            UpdateButtonInteractable();
        }

        private void UpdateButtonInteractable()
        {
            buildButton.interactable = CanBuild;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            towerManager.HideRangeIndicator();
        }
    }
}
