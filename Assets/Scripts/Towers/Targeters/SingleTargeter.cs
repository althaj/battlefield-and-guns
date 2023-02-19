using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSG.BattlefieldAndGuns.Core;
using System.Linq;
using PSG.BattlefieldAndGuns.Managers;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class SingleTargeter : Targeter
    {
        private Enemy target;

        #region properties
        public override Enemy[] Targets => target != null ? new Enemy[] { target } : new Enemy[0];
        public override Enemy Target => target;
        #endregion

        public override Enemy[] GetTargets()
        {
            // Check old target distance.
            if(target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) > Range)
                    target = null;
            }

            // Get a new target.2
            if(target == null)
            {
                target = enemyManager.GetClosestEnemy(transform.position, Range);
            }

            // Return the target or empty.
            if (target != null)
            {
                return new Enemy[] { target };
            } else
            {
                return new Enemy[0];
            }
        }
    }
}
