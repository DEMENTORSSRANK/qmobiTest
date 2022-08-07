using System;
using System.Collections;
using Sources.Extensions;
using Sources.Ufos;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Spawn
{
    public class UfoSpawner : Spawner
    {
        [SerializeField] private ObjectPool<Ufo> _pool;

        [SerializeField] private BulletsSpawner _bulletsSpawner;

        [SerializeField] private Transform _shootTarget;

        [Min(0)] [SerializeField] private float _delay = 20;

        [SerializeField] private float _horizontalOffset;

        [Range(0, 100)] [SerializeField] private int _minPercentsVerticalOffset = 20;

        private Camera _camera;

        private Coroutine _spawning;

        private bool IsSpawned => _pool.SpawnedCount > 0;

        public event Action Crashed;

        public event Action Shot;

        public override void StartSpawning()
        {
            _spawning = StartCoroutine(Spawning());
        }

        public override void ClearSpawnedAndStopSpawning()
        {
            if (_spawning == null)
                throw new InvalidOperationException("Not spawning yet");
            
            StopCoroutine(_spawning);
            
            _spawning = null;
            
            if (!IsSpawned)
                return;
            
            _pool.Clear();
        }

        private void Spawn()
        {
            bool fromRight = Random.Range(0, 2) == 1;

            Ufos.Ufo ufo = _pool.Spawn(GenerateNewUfoPosition(fromRight));

            ufo.TargetFollower.IsRightMove = !fromRight;
        }

        private Vector2 GenerateNewUfoPosition(bool fromRight)
        {
            Vector2 rightCorner = _camera.GetRightUpperCornerWorldPosition();

            Vector2 leftCorner = _camera.GetLeftLowerCornerWorldPosition();

            float verticalRange = (rightCorner.y - leftCorner.y) * (_minPercentsVerticalOffset / 100f);

            return new Vector2(fromRight ? rightCorner.x - _horizontalOffset : -rightCorner.x + _horizontalOffset,
                Random.Range(leftCorner.y + verticalRange, rightCorner.y - verticalRange));
        }

        private void InitUfo()
        {
            Ufo ufo = _pool.Spawn();

            ufo.TargetFollower.UpdateTarget(_shootTarget);
            
            _bulletsSpawner.AddShooter(ufo.Shooter);
            
            ufo.Crashed += delegate
            {
                Crashed?.Invoke();
            };
            
            ufo.Shooter.Shot += delegate
            {
                Shot?.Invoke();
            };
            
            _pool.DeSpawn(ufo);
        }

        private void Start()
        {
            _camera = Camera.main;
            
            _pool.Initialize();
            
            InitUfo();
        }

        private IEnumerator Spawning()
        {
            var waitDelay = new WaitForSeconds(_delay);
            
            while (true)
            {
                yield return new WaitUntil(() => !IsSpawned);
                
                yield return waitDelay;

                Spawn();
            }
        }
    }
}