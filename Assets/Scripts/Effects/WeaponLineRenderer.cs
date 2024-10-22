using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Effects
{
    public class WeaponLineRenderer : MonoBehaviour
    {
        #region serialized variables
        [SerializeField]
        private Transform weaponEnd;
        #endregion

        #region private variables
        private LineRenderer lineRenderer;
        private Weapon weapon;
        #endregion

        #region properties

        #endregion

        private void Start()
        {
            if (weaponEnd == null)
                weaponEnd = transform;
        }

        private void OnEnable()
        {
            weapon = this.GetComponentInParents<Weapon>();
            weapon.OnFire += Weapon_OnFire;
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void OnDisable()
        {
            weapon.OnFire -= Weapon_OnFire;
        }

        /// <summary>
        /// Plays animation with the name Fire.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Weapon_OnFire(object sender, Enemy[] e)
        {
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, weaponEnd.position);
                lineRenderer.SetPosition(1, e[0].transform.position);
            }
        }
    } 
}
