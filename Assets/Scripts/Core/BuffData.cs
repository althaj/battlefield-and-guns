using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Core
{
    [CreateAssetMenu(fileName = "New Buff", menuName = "PSG/Create Buff")]
    public class BuffData : ScriptableObject
    {
        public enum BuffType
        {
            EnemyHealth,
            EnemySpeed,
            EnemyReward,
            WaveStrength,
            TowerBuildTime,
            TowerCost,
            WeaponDamage,
            WeaponFireRate,
            TargetterRange
        }

        public string Title;
        public bool IsBuff;
        public int Level;
        public string Description;
        public BuffType buffType;
        public float buffMultiplier;
    }
}
