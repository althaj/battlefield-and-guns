using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class ProjectileWeapon : Weapon
    {
        #region serialized fields
        [SerializeField]
        private Transform weaponEnd;
        [SerializeField] ProjectileType projectileType;
        #endregion

        private PoolManager poolManager;

        private void Start()
        {
            if (weaponEnd == null)
                weaponEnd = transform;

            poolManager = FindObjectOfType<PoolManager>();
        }

        public override void Fire()
        {
            if (targets.Length > 0)
            {
                base.Fire();
                GameObject projectileObject = poolManager.GetProjectile(projectileType);
                projectileObject.transform.position = weaponEnd.position;
                projectileObject.transform.rotation = weaponEnd.rotation;
                projectileObject.GetComponent<Projectile>().Initialize(targets[0], Damage, projectileType);
            }
        }

        public override bool HasValidTargets()
        {
            targets = targeter.GetTargets();
            if (targets.Length > 0)
                targets = new Enemy[] { targets[0] };
            else targets = new Enemy[0];
            return targets.Length > 0;
        }
    } 
}
