using PSG.BattlefieldAndGuns.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace PSG.BattlefieldAndGuns.Effects
{
    public class ExplosionOnDisable : MonoBehaviour
    {
        [SerializeField] private ExplosionType explosionType;

        private PoolManager poolManager;

        private void Start()
        {
            poolManager = FindObjectOfType<PoolManager>();
        }

        private void OnDisable()
        {
            GameObject explosion = poolManager.GetExplosion(explosionType);
            explosion.transform.position = transform.position;

            explosion.GetComponent<Explosion>().Initialize(explosionType, poolManager);
        }
    }
}
