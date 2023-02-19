using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class AllInRangeTargeter : Targeter
    {
        private Enemy[] targets;

        public override Enemy[] Targets => targets;

        public override Enemy Target => null;

        public override Enemy[] GetTargets()
        {
            targets = enemyManager.GetEnemiesInRange(transform.position, Range).ToArray();
            return targets;
        }
    } 
}
