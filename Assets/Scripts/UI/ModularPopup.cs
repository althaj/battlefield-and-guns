using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.BattlefieldAndGuns.UI
{
    public class ModularPopup : MonoBehaviour
    {
        #region private variables

        private bool isInitialized = false;
        private RectTransform rectTransform;
        private Text title;
        private Text text;

        #endregion

        /// <summary>
        /// Show a modular dialog.
        /// </summary>
        /// <param name="title">Title of the dialog.</param>
        /// <param name="text">Text of the dialog.</param>
        /// <param name="position">Position of the dialog in screen space. Popup is in the center of the screen if null.</param>
        public void ShowPopup(string title, string text, Vector2? position = null)
        {
            if (!isInitialized)
                Initialize();

            this.title.text = title;
            this.text.text = text;
            position = position.HasValue ? position.Value : new Vector2(Screen.width / 2, Screen.height / 2);
            this.rectTransform.position = position.Value;

            gameObject.SetActive(true);
        }

        /// <summary>
        /// Hide the popup.
        /// </summary>
        public void HidePopup()
        {
            gameObject.SetActive(false);
        }

        private void Initialize()
        {
            var parentPanel = transform.GetChild(0);
            title = parentPanel.GetChild(0).GetComponent<Text>();
            text = parentPanel.GetChild(1).GetComponent<Text>();

            rectTransform = GetComponent<RectTransform>();

            isInitialized = true;
        }
    }
}