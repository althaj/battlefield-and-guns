using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.BattlefieldAndGuns.UI
{
    public class BuffCard : MonoBehaviour
    {
        #region serialized variables
        [SerializeField]
        private Text titleText;
        [SerializeField]
        private Text descriptionText;
        #endregion

        #region private variables
        private BuffData buffData;
        #endregion

        #region properties

        #endregion

        /// <summary>
        /// Initialize a buff button.
        /// </summary>
        /// <param name="buffData">Data to initialize the button with.</param>
        public void Initialize(BuffData buffData)
        {
            this.buffData = buffData;
            titleText.text = buffData.Title;
            descriptionText.text = buffData.Description;
        }

        /// <summary>
        /// Handle click of the buff button.
        /// </summary>
        public void OnClick()
        {
            FindObjectOfType<BuffManager>().ApplyBuff(buffData);
            FindObjectOfType<EnemyManager>().StartWaveCountdown();
            FindObjectOfType<GameUI>().HideBuffPanel();
        }
    }
}
