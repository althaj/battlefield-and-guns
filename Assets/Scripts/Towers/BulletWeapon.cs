using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSG.BattlefieldAndGuns.Core;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class BulletWeapon : Weapon
    {
        private void Update()
        {
            if(coolDown <= 0)
            {
                targets = targeter.GetTargets();
                if (targets.Length > 0)
                    targets = new Enemy[] { targets[0] };
                else targets = new Enemy[0];

                if (targets.Length > 0)
                {
                    Fire();
                }
            } else
            {
                coolDown -= Time.deltaTime;
            }
        }

        /// <summary>
        /// Shoot the weapon.
        /// </summary>
        public override void Fire()
        {
            if(targets.Length > 0)
            {
                base.Fire();
                coolDown = FireRate;
                targets[0].DealDamage(Damage);
            }
        }
    } 
}
