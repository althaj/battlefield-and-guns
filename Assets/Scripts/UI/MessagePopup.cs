using TMPro;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.UI
{
    public class MessagePopup : MonoBehaviour
    {
        private GameObject messagePanel;
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI subtitleText;
        private Animator animator;

        void Start()
        {
            messagePanel = transform.GetChild(0).gameObject;
            titleText = messagePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            subtitleText = messagePanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Displays a message in the message popup.
        /// </summary>
        /// <param name="title">Title of the message.</param>
        /// <param name="subtitle">Subtitle of the message.</param>
        /// <param name="displayDuration">For how long the message should stay displayed.</param>
        public void ShowMessage(string title, string subtitle = null, MessageDisplayDuration displayDuration = MessageDisplayDuration.Short)
        {
            titleText.text = title;
            subtitleText.text = subtitle;
            animator.SetTrigger("Show");

            CancelInvoke(nameof(HideMessage));

            switch (displayDuration)
            {
                case MessageDisplayDuration.Manual:
                    break;
                case MessageDisplayDuration.Short:
                    Invoke(nameof(HideMessage), 5f);
                    break;
                case MessageDisplayDuration.Long:
                    Invoke(nameof(HideMessage), 8f);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Hides the message popup.
        /// </summary>
        public void HideMessage()
        {
            animator.SetTrigger("Hide");
        }

    }
}
