using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Managers;
using PSG.BattlefieldAndGuns.Utility;
using System;
using System.Collections;
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

        [Header("Buff panel")]
        [SerializeField]
        private GameObject buffPanel;
        [SerializeField]
        private GameObject buffCardPanel;
        [SerializeField]
        private GameObject buffSkipButton;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject buffCardPrefab;

        [Header("Background")]
        [SerializeField]
        private GameObject backgroundPanel;
        #endregion

        #region private variables
        private TowerManager towerManager;
        private BuffManager buffManager;
        private UpgradePanel upgradePanel;
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
            upgradePanel = FindObjectOfType<UpgradePanel>();

            buffPanel.SetActive(false);
            backgroundPanel.SetActive(false);
        }

        private void TowerManager_OnMoneyChanged(object sender, int e)
        {
            moneyText.text = e.ToString();
        }

        /// <summary>
        /// Begin placing a tower, showing the placeholder.
        /// </summary>
        /// <param name="index"></param>
        internal void BeginPlacingTower(int index)
        {
            towerManager.BeginPlacing(index);
        }

        /// <summary>
        /// Display the buff selection panel.
        /// </summary>
        /// <param name="level">Level of the buffs.</param>
        /// <param name="isBuff">To show buffs or debuffs.</param>
        public void ShowBuffPanel(bool isBuff)
        {
            buffCardPanel.transform.DestroyAllChildren();

            List<BuffData> buffs = buffManager.GetBuffChoices(3, isBuff);
            foreach(BuffData buffData in buffs)
            {
                GameObject buffCardObject = Instantiate(buffCardPrefab);
                buffCardObject.transform.SetParent(buffCardPanel.transform, false);
                buffCardObject.GetComponent<BuffCard>().Initialize(buffData);
            }

            buffSkipButton.SetActive(isBuff);
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
            upgradePanel.Show(tower, towerData);
            backgroundPanel.SetActive(true);
        }

        public void HideUpgradePanel()
        {
            upgradePanel.Hide();
            backgroundPanel.SetActive(false);
        }

        public void BackgroundPanel_OnClick()
        {
            HideUpgradePanel();
        }
    }

}