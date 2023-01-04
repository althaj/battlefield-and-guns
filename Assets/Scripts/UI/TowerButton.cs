using PSG.BattlefieldAndGuns.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.BattlefieldAndGuns.UI
{
    public class TowerButton : MonoBehaviour
    {
        private int index;

        private BuildPopup buildPopup;

        public void Initialize(string title, int index, BuildPopup buildPopup)
        {
            transform.GetChild(0).GetComponent<Text>().text = title;
            this.index = index;
            this.buildPopup = buildPopup;
        }

        public void BuildTower()
        {
            buildPopup.SelectTower(index);
        }
    } 
}
