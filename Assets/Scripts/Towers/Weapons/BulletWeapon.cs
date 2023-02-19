using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSG.BattlefieldAndGuns.Core;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class BulletWeapon : Weapon
    {
        /// <summary>
        /// Does the tower have valid targets?
        /// </summary>
        /// <returns></returns>
        public override bool HasValidTargets()
        {
            targets = targeter.GetTargets();
            if (targets.Length > 0)
                targets = new Enemy[] { targets[0] };
            else targets = new Enemy[0];
            return targets.Length > 0;
        }

        /// <summary>
        /// Shoot the weapon.
        /// </summary>
        public override void Fire()
        {
            if(targets.Length > 0)
            {
                base.Fire();
                targets[0].DealDamage(Damage);
            }
        }
    } 
}
