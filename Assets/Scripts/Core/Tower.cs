using PSG.BattlefieldAndGuns.Managers;
using PSG.BattlefieldAndGuns.UI;
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

        private Animator animator;
        #endregion

        #region properties
        [SerializeField]
        public string Title { get => towerData.Title; }
        public int Level { get => level; }
        public int Cost { get => towerData.GetCost(level); }
        public float BuildTime { get => towerData.GetBuildTime(level); }
        public Weapon[] Weapons { get; set; }

        public TowerData TowerData { get => towerData; }
        #endregion

        private void Awake()
        {
            Weapons = GetComponents<Weapon>();
            foreach (Weapon weapon in Weapons)
            {
                weapon.Initialize(towerData, level);
            }

            animator = GetComponent<Animator>();
            BeginBuilding();
        }

        private void Update()
        {
            if(currentBuildTime > 0)
            {
                currentBuildTime -= Time.deltaTime;
                float delta = 1 - currentBuildTime / BuildTime;

                if(currentBuildTime <= 0)
                {
                    EndBuilding();
                }
            }
        }

        private void OnMouseUp()
        {
            FindObjectOfType<GameUI>().ShowUpgradePanel(this, towerData);
        }

        public void LevelUp()
        {
            FindObjectOfType<TowerManager>().UpgradeTower(this);
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

            if (animator != null)
                animator.SetTrigger("Build");
        }

        /// <summary>
        /// End building or upgrading the tower.
        /// </summary>
        private void EndBuilding()
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i].enabled = true;
            }
            
            // We need to manually recalculate the bounds of the skinned mesh (Unity restriction),
            // as the mesh is moved by animation, but the transform stays in place.
            // We only need to do this once the building is complete.
            foreach (var renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                renderer.localBounds = new Bounds
                {
                    center = -Vector3.forward / renderer.transform.localScale.x / 4,
                    extents = Vector3.one / renderer.transform.localScale.x / 4
                };
            }
        }

    }
}
