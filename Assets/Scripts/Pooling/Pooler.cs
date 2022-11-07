using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Pooling
{
    public abstract class Pooler
    {
        public GameObject Prefab { get; private set; }
        public int PoolSize { get; private set; }

        public Pooler(GameObject prefab, int poolSize = 5)
        {
            Prefab = prefab;
            PoolSize = poolSize;
        }
        public abstract GameObject PoolObject();
        public abstract void ReleaseObject(GameObject go);
    }
}
