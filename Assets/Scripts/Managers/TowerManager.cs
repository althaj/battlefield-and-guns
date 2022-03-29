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
        [SerializeField]
        private Material placeholderMaterial;
        #endregion

        #region private variables
        private int money;
        private bool isBuilding = false;
        private int currentTowerIndex = -1;
        private GameObject currentPlaceholder;
        private List<GameObject> towerPlaceholderPrefabs;

        private Transform towerParent;
        private Transform placeholderParent;
        private TowerSpace[] towerSpaces;
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

            CreatePlaceholderTowers();

            towerSpaces = FindObjectsOfType<TowerSpace>();

            towerParent = new GameObject("Towers").transform;
        }

        private void Update()
        {
            if (isBuilding && currentPlaceholder != null)
            {
                TowerSpace closest = null;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    closest = towerSpaces
                                            .Where(x => x.IsFree)
                                            .OrderBy(x => Vector3.Distance(hit.point, x.transform.position))
                                            .FirstOrDefault();
                    
                    if(closest != null)
                        currentPlaceholder.transform.position = closest.transform.position;
                }

                if (closest != null && Input.GetMouseButtonDown(0))
                {
                    BuildTower(closest);
                }

                if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                {
                    StopPlacing();
                }
            }
        }

        /// <summary>
        /// Create a placeholder tower object and add it to the list.
        /// </summary>
        /// <param name="gameObject">Original tower to create a placeholder from.</param>
        private void AddPlaceholderTower(GameObject gameObject)
        {
            GameObject placeholderObject = Instantiate(gameObject);
            Targeter targeter = placeholderObject.GetComponent<Targeter>();

            // Set materials.
            Renderer[] renderers = placeholderObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = new Material[renderer.materials.Length];
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = placeholderMaterial;
                }
                renderer.materials = materials;
            }

            // Add range indicator.
            float range = targeter.Range * 2;
            GameObject rangeObject = Instantiate(rangeIndicatorPrefab, placeholderObject.transform.position, Quaternion.identity);
            rangeObject.transform.localScale = new Vector3(range, range, range);
            rangeObject.transform.parent = placeholderObject.transform;

            placeholderObject.transform.parent = placeholderParent;

            // Disable components
            foreach (Weapon weapon in placeholderObject.GetComponentsInChildren<Weapon>())
                weapon.enabled = false;
            targeter.enabled = false;
            placeholderObject.GetComponent<Tower>().enabled = false;

            placeholderObject.SetActive(false);
            towerPlaceholderPrefabs.Add(placeholderObject);
        }

        /// <summary>
        /// Create all placeholder towers.
        /// </summary>
        private void CreatePlaceholderTowers()
        {
            placeholderParent = new GameObject("PlaceholderTowers").transform;
            towerPlaceholderPrefabs = new List<GameObject>();

            foreach (GameObject gameObject in towerPrefabs)
            {
                AddPlaceholderTower(gameObject);
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
        /// <param name="position"></param>
        private void BuildTower(TowerSpace space)
        {
            if(!isBuilding || currentTowerIndex < 0)
            {
                Debug.LogError("BuildTower: the process of building tower has not been initiated, use BeginPlacing.", this);
                return;
            }

            if (towerPrefabs.Count <= currentTowerIndex)
            {
                Debug.LogError("BuildTower: index out of bounds! Index = " + currentTowerIndex, this);
                return;
            }

            BuildTower(towerPrefabs[currentTowerIndex], space.transform.position);
            space.IsFree = false;
        }

        /// <summary>
        /// Begin placing with the placeholder tower.
        /// </summary>
        /// <param name="index">Index of the tower.</param>
        public void BeginPlacing(int index)
        {
            if (money < towerPlaceholderPrefabs[index].GetComponent<Tower>().Cost)
                return;

            if (currentPlaceholder != null)
                currentPlaceholder.SetActive(false);

            isBuilding = true;
            currentTowerIndex = index;
            currentPlaceholder = towerPlaceholderPrefabs[index];
            currentPlaceholder.SetActive(true);
        }

        /// <summary>
        /// Stop with the placement.
        /// </summary>
        public void StopPlacing()
        {
            isBuilding = false;
            currentTowerIndex = -1;
            currentPlaceholder?.SetActive(false);
            currentPlaceholder = null;
        }
    } 
}
