using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Core
{
    public class Tower : MonoBehaviour
    {
        #region serialized variables
        [SerializeField]
        private TowerData towerData;
        [SerializeField]
        private int level;
        [SerializeField]
        public GameObject nextLevelPrefab;
        #endregion

        #region private variables
        private float currentBuildTime;
        private float buildStartHeight;
        private float buildEndHeight;
        #endregion

        #region properties
        [SerializeField]
        public string Title { get => towerData.Title; }
        public int Level { get => level; }
        public int Cost { get => towerData.GetCost(level); }
        public float BuildTime { get => towerData.GetBuildTime(level); }
        public Weapon[] Weapons { get; set; }
        #endregion

        private void Awake()
        {
            Weapons = GetComponents<Weapon>();
            foreach (Weapon weapon in Weapons)
            {
                weapon.Initialize(towerData, level);
            }

            buildEndHeight = transform.position.y;
            buildStartHeight = buildEndHeight - 3;
            BeginBuilding();
        }

        private void Update()
        {
            if(currentBuildTime > 0)
            {
                currentBuildTime -= Time.deltaTime;
                float delta = 1 - currentBuildTime / BuildTime;
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(buildStartHeight, buildEndHeight, delta), transform.position.z);

                if(currentBuildTime <= 0)
                {
                    EndBuilding();
                }
            }
        }

        public void LevelUp()
        {
            // Swap for next level tower.
        }

        /// <summary>
        /// Begin building or upgrading the tower.
        /// </summary>
        private void BeginBuilding()
        {
            currentBuildTime = BuildTime;
            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i].enabled = false;
            }
        }

        /// <summary>
        /// End building or upgrading the tower.
        /// </summary>
        private void EndBuilding()
        {
            transform.position = new Vector3(transform.position.x, buildEndHeight, transform.position.z);
            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i].enabled = true;
            }
        }
    }
}