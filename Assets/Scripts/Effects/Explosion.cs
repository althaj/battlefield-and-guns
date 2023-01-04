using PSG.BattlefieldAndGuns.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace PSG.BattlefieldAndGuns.Effects
{
    public class Explosion : MonoBehaviour
    {
        private PoolManager poolManager;
        private ExplosionType explosionType;
        private VisualEffect effect;

        [SerializeField]
        private float effectDuration;

        public void Initialize(ExplosionType explosionType, PoolManager poolManager)
        {
            this.poolManager = poolManager;

            if(effect == null)
                effect = GetComponent<VisualEffect>();

            this.explosionType = explosionType;
            effect.Play();
            Invoke("Release", effectDuration);
        }

        private void Release()
        {
            poolManager.ReleaseExplosion(explosionType, gameObject);
        }
    }
}
