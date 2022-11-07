using PSG.BattlefieldAndGuns.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Managers
{
    public class PoolManager : MonoBehaviour
    {
        private Pooler bulletPooler;

        #region serialized variables
        [SerializeField] private GameObject bulletPrefab;
        #endregion

        #region private variables

        #endregion

        #region properties

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            bulletPooler = new UnityPooler(bulletPrefab);
        }

        public GameObject GetBullet()
        {
            return bulletPooler.PoolObject();
        }

        public void ReleaseBullet(GameObject bullet)
        {
            bulletPooler.ReleaseObject(bullet);
        }
    }
}
