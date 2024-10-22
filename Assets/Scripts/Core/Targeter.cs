using PSG.BattlefieldAndGuns.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Core
{
    public abstract class Targeter: MonoBehaviour
    {
        #region serialized variables
        #endregion

        #region private variables
        private int level;
        private TowerData towerData;
        #endregion

        #region properties
        [SerializeField]
        public int Level { get => level; }
        public float Range { get => towerData.GetRange(level); }
        public abstract Enemy[] Targets { get; }
        public abstract Enemy Target { get; }
        #endregion

        internal EnemyManager enemyManager;

        private void Start()
        {
            enemyManager = FindObjectOfType<EnemyManager>();
        }

        /// <summary>
        /// Fire the weapon.
        /// </summary>
        public abstract Enemy[]  GetTargets();

        /// <summary>
        /// Level up the weapon to the next level if possible.
        /// </summary>
        public void LevelUp()
        {
            level++;
        }

        internal void Initialize(TowerData towerData, int level)
        {
            this.towerData = towerData;
            this.level = level;
        }
    }
}
