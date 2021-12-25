using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Core
{
    public abstract class Weapon: MonoBehaviour
    {
        #region serialized variables
        #endregion

        #region private variables
        private int level;
        private TowerData towerData;
        internal Targeter targeter;
        internal float coolDown;
        internal Enemy[] targets;
        #endregion

        #region properties
        [SerializeField]
        public int Level { get => level; }
        public int Damage { get => towerData.GetDamage(level); }
        public float FireRate { get => towerData.GetFireRate(level); }
        #endregion

        #region Events
        public event EventHandler<Enemy[]> OnFire;
        #endregion

        private void Awake()
        {
            targeter = GetComponent<Targeter>();
        }

        private void Update()
        {
            if (coolDown <= 0)
            {
                if (HasValidTargets())
                {
                    Fire();
                }
            }
            else
            {
                coolDown -= Time.deltaTime;
            }
        }

        public abstract bool HasValidTargets();

        /// <summary>
        /// Fire the weapon.
        /// </summary>
        public virtual void Fire()
        {
            coolDown = FireRate;
            OnFire?.Invoke(this, targets);
        }

        /// <summary>
        /// Level up the weapon to the next level if possible.
        /// </summary>
        public void LevelUp()
        {
            level++;
        }

        /// <summary>
        /// Initialize the weapon.
        /// </summary>
        /// <param name="towerData">Tower data to initialize with.</param>
        /// <param name="level">Level to initialize with.</param>
        public virtual void Initialize(TowerData towerData, int level)
        {
            this.towerData = towerData;
            this.level = level;

            if(targeter == null)
                targeter = GetComponent<Targeter>();
            targeter.Initialize(towerData, level);
        }
    } 
}
