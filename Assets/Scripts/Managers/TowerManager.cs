using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Towers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Managers
{
    public class TowerManager : MonoBehaviour
    {
        #region serialized variables

        [SerializeField]
        private int startingMoney;
        [SerializeField]
        private List<GameObject> towerPrefabs;

        [SerializeField]
        private GameObject rangeIndicatorPrefab;

        #endregion

        #region private variables

        private int money;
        private bool isBuilding = false;

        private Transform towerParent;

        private GameObject rangeIndicator;

        #endregion

        #region properties
        public List<GameObject> Towers { get => towerPrefabs; }
        public int Money { get => money; }

        #endregion

        #region events

        public event EventHandler<int> OnMoneyChanged;

        #endregion

        private void Start()
        {
            money = startingMoney;

            towerParent = new GameObject("Towers").transform;

            rangeIndicator = Instantiate(rangeIndicatorPrefab);
            rangeIndicator.SetActive(false);
        }

        private void Update()
        {
            if (isBuilding)
            {
                if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                {
                    StopPlacing();
                }
            }
        }

        /// <summary>
        /// Add a reward money.
        /// </summary>
        /// <param name="amount">Money to add.</param>
        public void AddReward(int amount)
        {
            money += amount;
            OnMoneyChanged?.Invoke(this, money);
        }

        /// <summary>
        /// Build a new tower.
        /// </summary>
        /// <param name="tower">Tower gameObject to build.</param>
        /// <param name="position">Position where to build.</param>
        private void BuildTower(GameObject tower, Vector3 position)
        {
            var cost = tower.GetComponent<Tower>().Cost;
            if (money < cost)
            {
                Debug.LogWarning($"Cannot build the tower {tower.name}. The cost is {cost}, but the player only has {money} money.");
            }    

            GameObject towerObject = Instantiate(tower, position, Quaternion.identity);
            towerObject.transform.parent = towerParent;

            money -= towerObject.GetComponent<Tower>().Cost;
            OnMoneyChanged?.Invoke(this, money);

            StopPlacing();
        }

        public void UpgradeTower(Tower tower)
        {
            BuildTower(tower.nextLevelPrefab, tower.transform.position);
            Destroy(tower.gameObject);
        }

        /// <summary>
        /// Build a new tower.
        /// </summary>
        /// <param name="space">TowerSpace to build the tower on.</param>
        /// <param name="tower">Tower prefab to build.</param>
        public void BuildTower(TowerSpace space, GameObject tower)
        {
            if (space == null)
            {
                Debug.LogError("BuildTower: TowerSpace is null.", this);
                return;
            }

            if (tower == null)
            {
                Debug.LogError("BuildTower: Tower is null.", this);
                return;
            }

            BuildTower(tower, space.transform.position + Vector3.up * 0.5f);
            space.IsFree = false;
        }

        /// <summary>
        /// Begin placing with the placeholder tower.
        /// </summary>
        public void BeginPlacing()
        {
            isBuilding = true;
        }

        /// <summary>
        /// Stop with the placement.
        /// </summary>
        public void StopPlacing()
        {
            isBuilding = false;
        }

        public void ShowRangeIndicator(Vector3 position, float range)
        {
            range = range * 2;

            rangeIndicator.transform.position = position;
            rangeIndicator.transform.localScale = new Vector3(range, range, range);
            rangeIndicator.SetActive(true);
        }

        public void HideRangeIndicator()
        {
            rangeIndicator.SetActive(false);
        }

    } 
}
