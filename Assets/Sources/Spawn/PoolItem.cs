using System;
using UnityEngine;

namespace Sources.Spawn
{
    public abstract class PoolItem : MonoBehaviour
    {
        public event Action<PoolItem> InitDeSpawn;

        public event Action<PoolItem> DeSpawned;

        protected void DeSpawn()
        {
            InitDeSpawn?.Invoke(this);
        }

        public void OnDeSpawned()
        {
            DeSpawned?.Invoke(this);
        }
    }
}