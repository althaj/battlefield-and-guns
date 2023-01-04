using UnityEngine;
using UnityEngine.Pool;

namespace PSG.BattlefieldAndGuns.Pooling
{
    public class UnityPooler : Pooler
    {
        #region serialized variables

        #endregion

        #region private variables

        #endregion

        #region properties
        private IObjectPool<GameObject> Pool { get; set; }
        #endregion

        #region Constructor

        public UnityPooler(GameObject prefab, int poolSize = 5) : base(prefab, poolSize)
        {
            Pool = new ObjectPool<GameObject>(CreateObject, OnGetObject, OnReleaseObject, defaultCapacity: poolSize);
        }

        #endregion

        private GameObject CreateObject()
        {
            return GameObject.Instantiate(Prefab);
        }

        private void OnGetObject(GameObject obj)
        {
            obj.SetActive(true);
        }

        private void OnReleaseObject(GameObject obj)
        {
            obj.SetActive(false);
        }

        public override GameObject PoolObject()
        {
            return Pool.Get();
        }

        public override void ReleaseObject(GameObject go)
        {
            Pool.Release(go);
        }
    }
}
