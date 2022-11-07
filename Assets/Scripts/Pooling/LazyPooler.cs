using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PSG.BattlefieldAndGuns.Pooling
{
    public class LazyPooler: Pooler
    {
        #region serialized variables

        #endregion

        #region private variables

        #endregion

        #region properties
        private List<GameObject> Pool { get; set; }
        #endregion

        #region Constructor

        public LazyPooler(GameObject prefab, int poolSize = 5) : base(prefab, poolSize)
        {
            Pool = new List<GameObject>();
            IncreaseSize();
        }

        #endregion

        private void IncreaseSize()
        {
            GameObject instance;
            for(int i = 0; i < PoolSize; i++)
            {
                instance = GameObject.Instantiate(Prefab);
                instance.SetActive(false);
                Pool.Add(instance);
            }
        }

        public override GameObject PoolObject()
        {
            if(!Pool.Any(x => !x.activeSelf))
                IncreaseSize();

            GameObject instance = Pool.FirstOrDefault(x => !x.activeSelf);
            instance.SetActive(true);
            return instance;
        }

        public override void ReleaseObject(GameObject go)
        {
            if (Pool.Contains(go))
                go.SetActive(false);
        }
    }
}
