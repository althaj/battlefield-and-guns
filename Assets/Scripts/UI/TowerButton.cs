using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.UI
{
    public class TowerButton : MonoBehaviour
    {
        [SerializeField]
        private int index;

        private GameUI gameUI;

        private void Start()
        {
            gameUI = FindObjectOfType<GameUI>();
        }

        /// <summary>
        /// Begin building a tower.
        /// </summary>
        public void BuildTower()
        {
            gameUI.BeginPlacingTower(index);
        }
    } 
}
