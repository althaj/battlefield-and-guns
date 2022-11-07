using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class ProjectileWeapon : BulletWeapon
    {
        #region serialized fields
        [SerializeField]
        private Transform weaponEnd;
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
                GameObject projectileObject = poolManager.GetBullet();
                projectileObject.transform.position = weaponEnd.position;
                projectileObject.GetComponent<Projectile>().Initialize(targets[0], Damage);
            }
        }
    } 
}
