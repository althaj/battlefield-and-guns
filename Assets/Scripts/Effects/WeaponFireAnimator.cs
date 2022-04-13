using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Effects
{
    public class WeaponFireAnimator : MonoBehaviour
    {
        #region serialized variables

        #endregion

        #region private variables
        private Animator animator;
        private Weapon weapon;
        #endregion

        #region properties

        #endregion

        private void OnEnable()
        {
            weapon = this.GetComponentInParents<Weapon>();
            weapon.OnFire += Weapon_OnFire;
            animator = GetComponent<Animator>();
        }

        private void OnDisable()
        {
            weapon.OnFire -= Weapon_OnFire;
        }

        /// <summary>
        /// Sets the trigger Fire.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Weapon_OnFire(object sender, Enemy[] e)
        {
            if(animator != null)
            {
                animator.SetTrigger("Fire");
            }
        }
    } 
}
