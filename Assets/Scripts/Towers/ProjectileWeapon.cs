using PSG.BattlefieldAndGuns.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class ProjectileWeapon : BulletWeapon
    {
        #region serialized fields
        [SerializeField]
        private GameObject projectilePrefab;
        [SerializeField]
        private Transform weaponEnd;
        #endregion

        private void Start()
        {
            if(projectilePrefab == null)
            {
                Debug.LogError("ProjectileWeapon: No projectile assigned.");
            }

            if(projectilePrefab.GetComponent<Projectile>() == null)
            {
                Debug.LogError("ProjectileWeapon: Assigned projectile does not have a Projectile component.");
                projectilePrefab = null;
            }

            if (weaponEnd == null)
                weaponEnd = transform;
        }

        public override void Fire()
        {
            if (projectilePrefab != null && targets.Length > 0)
            {
                base.Fire();
                GameObject projectileObject = Instantiate(projectilePrefab, weaponEnd.position, Quaternion.identity);
                projectileObject.GetComponent<Projectile>().Initialize(targets[0], Damage);
            }
        }
    } 
}
