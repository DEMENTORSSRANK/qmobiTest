using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Spawn
{
    [Serializable]
    public class ObjectPool<T> where T : PoolItem
    {
        [SerializeField] private T _prefab;

        [SerializeField] private Transform _parent;

        [Min(1)] [SerializeField] private int _capacity = 1;

        private List<T> _pool;

        private List<T> _spawned;

        public bool Initialized { get; private set; }

        public int SpawnedCount => _spawned.Count;

        public void Initialize()
        {
            if (Initialized)
                throw new InvalidOperationException("Pool is already initialized");

            Initialized = true;

            _pool = new List<T>(_capacity);

            _spawned = new List<T>(_capacity);

            for (int i = 0; i < _capacity; i++)
                CreateNewElementInPool(Vector2.zero, Quaternion.identity);
        }

        public void Clear()
        {
            _spawned.ToList().ForEach(DeSpawn);
        }

        public T Spawn(Vector2 position = default, Quaternion rotation = default)
        {
            if (_spawned.Count >= _pool.Count || _pool.All(x => x.gameObject.activeSelf))
            {
                CreateNewElementInPool(position, rotation);

                T newElement = GetNewElementFromPool(position, rotation);
                
                _spawned.Add(newElement);

                return newElement;
            }

            T elementFromPool = GetNewElementFromPool(position, rotation);

            _spawned.Add(elementFromPool);

            return elementFromPool;
        }

        public void DeSpawn(T spawned)
        {
            if (!_spawned.Contains(spawned))
                return;

            spawned.gameObject.SetActive(false);
                
            spawned.OnDeSpawned();

            _spawned.Remove(spawned);
        }

        private T GetNewElementFromPool(Vector2 position, Quaternion rotation)
        {
            T elementFromPool = _pool.First(x => !x.gameObject.activeSelf);

            Transform transform = elementFromPool.transform;

            transform.localPosition = position;

            transform.localRotation = rotation;

            elementFromPool.gameObject.SetActive(true);

            return elementFromPool;
        }

        private T CreateNewElementInPool(Vector2 position, Quaternion rotation)
        {
            T newElement = Object.Instantiate(_prefab, position, rotation, _parent);

            newElement.InitDeSpawn += delegate(PoolItem item) { DeSpawn(item as T); };

            _pool.Add(newElement);

            newElement.gameObject.SetActive(false);

            return newElement;
        }
    }
}