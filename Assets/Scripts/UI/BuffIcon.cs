using PSG.BattlefieldAndGuns.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.BattlefieldAndGuns.Ui
{
    public class BuffIcon : MonoBehaviour
    {
        #region private variables

        private Image iconImage;
        private BuffData buffData;

        #endregion

        public void Initialize(BuffData buffData)
        {
            this.iconImage = transform.GetChild(0).GetComponent<Image>();
            this.buffData = buffData;
            if(this.buffData.Icon != null)
                iconImage.sprite = buffData.Icon;
        }
    }
}
