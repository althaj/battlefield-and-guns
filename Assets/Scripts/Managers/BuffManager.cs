using PSG.BattlefieldAndGuns.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Managers
{
    public class BuffManager : MonoBehaviour
    {
        #region serialized variables
        [SerializeField]
        private List<BuffData> availableBuffs;
        #endregion

        #region private variables
        private List<BuffData> appliedBuffs;
        private int level = 1;
        private EnemyManager enemyManager;
        private TowerManager towerManager;
        #endregion

        #region properties
        public List<BuffData> AppliedBuffs { get => appliedBuffs; }
        #endregion

        private void Start()
        {
            enemyManager = FindObjectOfType<EnemyManager>();
            towerManager = FindObjectOfType<TowerManager>();
            appliedBuffs = new List<BuffData>();
        }

        /// <summary>
        /// Get random choices for buff selection.
        /// </summary>
        /// <param name="count">Number of choices to return.</param>
        /// <param name="isBuff">To return buffs or debuffs.</param>
        /// <returns></returns>
        public List<BuffData> GetBuffChoices(int count, bool isBuff)
        {
            return availableBuffs
                .Where(x => x.Level == level && x.IsBuff == isBuff)
                .OrderBy(x => Random.Range(0f, 1f))
                .Take(count).ToList();
        }

        /// <summary>
        /// Apply a buff.
        /// </summary>
        /// <param name="buff">Buff to be applied.</param>
        public void ApplyBuff(BuffData buff)
        {
            if(buff == null || !availableBuffs.Contains(buff))
            {
                Debug.LogError("ApplyBuff: could not find the buff!", buff);
                return;
            }

            if(buff.Level > level)
            {
                Debug.LogError("ApplyBuff: buff is of a higher level!", buff);
                return;
            }

            appliedBuffs.Add(buff);
            availableBuffs.Remove(buff);

            if (buff.IsBuff)
                level++;
        }

        /// <summary>
        /// Get active multiplier of a type.
        /// </summary>
        /// <param name="buffType">TType of the buff.</param>
        /// <returns></returns>
        public float GetAppliedMultiplier(BuffData.BuffType buffType)
        {
            return 1 + appliedBuffs.Where(x => x.buffType == buffType).Sum(x => x.buffMultiplier);
        }
    }
}