using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class TowerSpace : MonoBehaviour
    {
        private Animator animator;

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private bool isFree = true;
        public bool IsFree
        {
            get => isFree;
            set
            {
                isFree = value;
                if(!isFree)
                    UseSpace();
            }
        }

        private void UseSpace()
        {
            if (animator != null)
                animator.SetTrigger("Open");
        }

        private void OnMouseUp()
        {
            if(isFree)
                FindObjectOfType<GameUI>().ShowBuildPopup(this);
        }
    }
}
