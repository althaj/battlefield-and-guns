using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Managers;
using PSG.BattlefieldAndGuns.Towers;
using PSG.BattlefieldAndGuns.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.BattlefieldAndGuns.UI
{
    public class GameUI : MonoBehaviour
    {
        #region serialized variables

        [SerializeField]
        private Text moneyText;
        [SerializeField]
        private Text currentHealthText;
        [SerializeField]
        private Text maxHealthText;

        [Header("Buff panel")]
        [SerializeField]
        private GameObject buffPanel;
        [SerializeField]
        private GameObject buffCardPanel;
        [SerializeField]
        private GameObject buffSkipButton;
        [SerializeField]
        private GameObject buffListPanel;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject buffCardPrefab;
        [SerializeField]
        private GameObject buffIconPrefab;

        [Header("Background")]
        [SerializeField]
        private GameObject backgroundPanel;

        [Header("Popups"), SerializeField]
        private UpgradePopup upgradePopup;
        [SerializeField]
        private BuildPopup buildPopup;

        [SerializeField]
        private ModularPopup modularPopup;

        #endregion

        #region private variables
        private TowerManager towerManager;
        private BuffManager buffManager;
        #endregion

        #region properties

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            towerManager = FindObjectOfType<TowerManager>();
            towerManager.OnMoneyChanged += TowerManager_OnMoneyChanged;
            TowerManager_OnMoneyChanged(this, towerManager.Money);

            buffManager = FindObjectOfType<BuffManager>();

            buffPanel.SetActive(false);
            backgroundPanel.SetActive(false);

            // Health
            Health health = FindObjectOfType<Health>();
            maxHealthText.text = health.StartingHealth.ToString();
            health.OnHealthChanged += Health_OnHealthChanged;
            health.OnGameOver += Health_OnGameOver;

            // Add towers to the build popup
            foreach(GameObject tower in towerManager.Towers)
            {
                buildPopup.AddTowerButton(tower);
            }
        }

        private void Health_OnGameOver(object sender, EventArgs e)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Constants.SCENE_MANAGEMENT_MAIN_MENU_INDEX);
        }

        private void Health_OnHealthChanged(object sender, int e)
        {
            currentHealthText.text = e.ToString();
        }

        private void TowerManager_OnMoneyChanged(object sender, int e)
        {
            moneyText.text = e.ToString();
        }

        /// <summary>
        /// Begin placing a tower, showing the placeholder.
        /// </summary>
        /// <param name="space">TowerSpace to start building on.</param>
        internal void BeginPlacingTower(TowerSpace space)
        {
            towerManager.BeginPlacing();
        }

        /// <summary>
        /// Display the buff selection panel.
        /// </summary>
        /// <param name="level">Level of the buffs.</param>
        /// <param name="isBuff">To show buffs or debuffs.</param>
        public void ShowBuffPanel()
        {
            buffCardPanel.transform.DestroyAllChildren();

            List<BuffData> buffs = buffManager.GetBuffChoices(3, true);
            foreach(BuffData buffData in buffs)
            {
                GameObject buffCardObject = Instantiate(buffCardPrefab);
                buffCardObject.transform.SetParent(buffCardPanel.transform, false);
                buffCardObject.GetComponent<BuffCard>().Initialize(buffData);
            }

            buffSkipButton.SetActive(true);
            buffPanel.SetActive(true);
        }

        /// <summary>
        /// Hide the buff panel.
        /// </summary>
        public void HideBuffPanel()
        {
            buffPanel.SetActive(false);
        }

        public void ShowUpgradePanel(Tower tower, TowerData towerData)
        {
            upgradePopup.Show(tower, towerData);
            backgroundPanel.SetActive(true);
        }

        public void HideUpgradePanel()
        {
            upgradePopup.Hide();
            backgroundPanel.SetActive(false);
        }

        public void BackgroundPanel_OnClick()
        {
            HideUpgradePanel();
            HideBuildPopup();
        }

        public void AddBuffIcon(BuffData buffData)
        {
            GameObject icon = Instantiate(buffIconPrefab, buffListPanel.transform);
            icon.transform.name = buffData.Title;
            icon.GetComponent<BuffIcon>().Initialize(buffData, this);
        }

        /// Show a modular dialog.
        /// </summary>
        /// <param name="title">Title of the dialog.</param>
        /// <param name="text">Text of the dialog.</param>
        /// <param name="position">Position of the dialog in screen space. Popup is in the center of the screen if null.</param>
        public void ShowPopup(string title, string text, Vector2? position = null)
        {
            modularPopup.ShowPopup(title, text, position);
        }

        public void HidePopup()
        {
            modularPopup.HidePopup();
        }

        public void ShowBuildPopup(TowerSpace towerSpace)
        {
            buildPopup.Show(towerSpace);
            backgroundPanel.SetActive(true);
        }

        public void HideBuildPopup()
        {
            buildPopup.Hide();
            backgroundPanel.SetActive(false);
        }
    }

}