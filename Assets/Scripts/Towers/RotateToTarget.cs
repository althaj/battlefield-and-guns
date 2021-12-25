using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.Utility;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class RotateToTarget : MonoBehaviour
    {
        private Targeter targeter;

        [Range(0, 10)]
        [SerializeField]
        private float rotationSpeed;
        
        // Start is called before the first frame update
        void Start()
        {
            targeter = this.GetComponentInParents<Targeter>();
        }

        // Update is called once per frame
        void Update()
        {
            if(targeter != null)
            {
                Enemy target = targeter.Target;
                if(target != null)
                {
                    Quaternion lookOnLook = Quaternion.LookRotation(target.transform.position - transform.position);

                    transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * rotationSpeed);
                }
            }
        }
    }
}
