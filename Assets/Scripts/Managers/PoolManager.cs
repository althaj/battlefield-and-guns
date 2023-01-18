using PSG.BattlefieldAndGuns.Effects;
using PSG.BattlefieldAndGuns.Pooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Managers
{
    public enum ProjectileType
    {
        Bullet,
        Rocket
    }

    public enum ExplosionType
    {
        None,
        Rocket,
        SmallVehicle
    }

    public class PoolManager : MonoBehaviour
    {
        private static PoolManager instance;
        public static PoolManager Instance
        {
            get { return instance; }
        }

        // Projectiles
        private Pooler bulletPooler;
        private Pooler rocketPooler;

        // Explosions
        private Pooler rocketExplosionPooler;
        private Pooler smallVehicleExplosionPooler;

        #region serialized variables

        [Header("Projectiles")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject rocketPrefab;

        [Header("Explosions")]
        [SerializeField] private GameObject rocketExplosionPrefab;
        [SerializeField] private GameObject smallWehicleExplosionPrefab;

        #endregion

        #region private variables

        #endregion

        #region properties

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            instance = this;

            // Projectiles
            bulletPooler = new UnityPooler(bulletPrefab);
            rocketPooler = new UnityPooler(rocketPrefab);

            // Explosions
            rocketExplosionPooler = new UnityPooler(rocketExplosionPrefab);
            smallVehicleExplosionPooler = new UnityPooler(smallWehicleExplosionPrefab);
        }

        public GameObject GetProjectile(ProjectileType projectileType)
        {
            switch (projectileType)
            {
                case ProjectileType.Bullet:
                    return bulletPooler.PoolObject();
                case ProjectileType.Rocket:
                    return rocketPooler.PoolObject();
                default:
                    throw new NotImplementedException($"GetProjectile: Projectile type {projectileType} not implemented.");
            }
        }

        public void ReleaseProjectile(ProjectileType projectileType, GameObject projectile)
        {
            switch (projectileType)
            {
                case ProjectileType.Bullet:
                    bulletPooler.ReleaseObject(projectile);
                    break;
                case ProjectileType.Rocket:
                    rocketPooler.ReleaseObject(projectile);
                    break;
                default:
                    throw new NotImplementedException($"ReleaseProjectile: Projectile type {projectileType} not implemented.");
            }
        }

        public GameObject GetExplosion(ExplosionType explosionType)
        {
            switch (explosionType)
            {
                case ExplosionType.None:
                    return null;
                case ExplosionType.Rocket:
                    return rocketExplosionPooler.PoolObject();
                case ExplosionType.SmallVehicle:
                    return smallVehicleExplosionPooler.PoolObject();
                default:
                    throw new NotImplementedException($"GetProjectile: Explosion type {explosionType} not implemented.");
            }
        }

        public void ReleaseExplosion(ExplosionType explosionType, GameObject explosion)
        {
            switch (explosionType)
            {
                case ExplosionType.None:
                    return;
                case ExplosionType.Rocket:
                    rocketExplosionPooler.ReleaseObject(explosion);
                    break;
                case ExplosionType.SmallVehicle:
                    smallVehicleExplosionPooler.ReleaseObject(explosion);
                    break;
                default:
                    throw new NotImplementedException($"ReleaseExplosion: Explosion type {explosionType} not implemented.");
            }
        }

        public void CreateExplosion(ExplosionType explosionType, Vector3 position)
        {
            if(explosionType == ExplosionType.None) return;

            GameObject explosion = GetExplosion(explosionType);
            explosion.transform.position = position;

            explosion.GetComponent<Explosion>().Initialize(explosionType, this);
        }
    }
}
