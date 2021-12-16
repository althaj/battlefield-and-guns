using PSG.BattlefieldAndGuns.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Core
{
    [CreateAssetMenu(fileName = "New Tower Data", menuName = "PSG/Crate Tower Data", order = 0)]
    public class TowerData : ScriptableObject
    {
        // Tower data
        [Header("Tower data")]
        public string Title;
        public int InitialCost;
        public float CostScaling;
        public float InitialBuildTime;
        public float BuildTimeScaling;

        // Weapon data
        [Header("Weapon data")]
        public float InitialFireRate;
        public float FireRateScaling;
        public int InitialDamage;
        public float DamageScaling;

        // Targetter data
        [Header("Targetter data")]
        public float InitialRange;
        public float RangeScaling;

        #region Methods
        /// <summary>
        /// Cost of the tower.
        /// </summary>
        /// <param name="level">Level of the tower.</param>
        public int GetCost(int level)
        {
            return InitialCost.ScaleNumber(level, CostScaling);
        }

        /// <summary>
        /// Build time of the tower.
        /// </summary>
        /// <param name="level">Level of the tower.</param>
        public float GetBuildTime(int level)
        {
            return InitialBuildTime.ScaleNumber(level, BuildTimeScaling);
        }

        /// <summary>
        /// Fire rate of the tower.
        /// </summary>
        /// <param name="level">Level of the tower.</param>
        public float GetFireRate(int level)
        {
            return InitialFireRate.ScaleNumber(level, FireRateScaling);
        }

        /// <summary>
        /// Damage of the tower.
        /// </summary>
        /// <param name="level">Level of the tower.</param>
        public int GetDamage(int level)
        {
            return InitialDamage.ScaleNumber(level, DamageScaling);
        }

        /// <summary>
        /// Range of the tower.
        /// </summary>
        /// <param name="level">Level of the tower.</param>
        public float GetRange(int level)
        {
            return InitialRange.ScaleNumber(level, RangeScaling);
        } 
        #endregion
    }
}
