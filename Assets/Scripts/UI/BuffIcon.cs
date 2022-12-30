using PSG.BattlefieldAndGuns.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PSG.BattlefieldAndGuns.UI
{
    public class BuffIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        #region private variables

        private Image iconImage;
        private BuffData buffData;

        private RectTransform rectTransform;
        private GameUI gameUI;

        #endregion

        public void Initialize(BuffData buffData, GameUI gameUI)
        {
            this.iconImage = transform.GetChild(0).GetComponent<Image>();
            this.buffData = buffData;
            if(this.buffData.Icon != null)
                iconImage.sprite = buffData.Icon;

            this.gameUI = gameUI;
            this.rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var popupPosition = rectTransform.position + Vector3.left * 90f;
            popupPosition.y = Mathf.Clamp(popupPosition.y, 30f, Screen.height - 30f);
            gameUI.ShowPopup(buffData.Title, buffData.Description, popupPosition);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            gameUI.HidePopup();
        }
    }
}
