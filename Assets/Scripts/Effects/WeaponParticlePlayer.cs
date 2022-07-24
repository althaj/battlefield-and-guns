using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Utility;
using UnityEngine.VFX;

namespace PSG.BattlefieldAndGuns.Effects
{
    [RequireComponent(typeof(VisualEffect))]
    public class WeaponParticlePlayer : MonoBehaviour
    {
        #region private variables
        private VisualEffect visualEffect;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            visualEffect = GetComponent<VisualEffect>();
            this.GetComponentInParents<Weapon>().OnFire += Weapon_OnFire;
        }

        private void Weapon_OnFire(object sender, Enemy[] e)
        {
            if (visualEffect != null && visualEffect.visualEffectAsset != null)
            {
                visualEffect.Play();
            }
        }
    }
}
