using System;
using System.Collections;
using Sources.Asteroids;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Spawn
{
    public class AsteroidsSpawner : Spawner
    {
        [SerializeField] private ObjectPool<Asteroid> _asteroidsPool;

        [Min(1)] [SerializeField] private int _firstWaveCount = 2;

        [Min(0)] [SerializeField] private int _everyWaveAddCount = 1;

        [Min(0)] [SerializeField] private float _newWaveDelay = 2;

        [Min(0)] [SerializeField] private float _minSpeed = 1;

        [SerializeField] private float _maxSpeed = 1.6f;

        [SerializeField] private AsteroidSizeStep[] _splitSteps;

        [Min(0)] [SerializeField] private float _angleSplit = 45;

        private int _lastWaveCount = -1;

        private Coroutine _spawning;
        
        private AsteroidPositionGenerator _asteroidPositionGenerator;

        private AsteroidSplitter _splitter;

        private float GenerateRandomSpeed => Random.Range(_minSpeed, _maxSpeed);

        public event Action AnyCrashed;

        public override void StartSpawning()
        {
            _spawning = StartCoroutine(Spawning());
        }

        public override void ClearSpawnedAndStopSpawning()
        {
            if (_spawning == null)
                throw new InvalidOperationException("Not started spawn yet");

            StopCoroutine(_spawning);

            _spawning = null;

            _lastWaveCount = -1;

            _asteroidsPool.Clear();
        }

        private void SpawnNewWave()
        {
            _lastWaveCount = _lastWaveCount < 0 ? _firstWaveCount : _lastWaveCount + _everyWaveAddCount;

            for (int i = 0; i < _lastWaveCount; i++)
                SpawnNewAsteroid(0, _asteroidPositionGenerator.GenerateNewAsteroidPosition(),
                    _asteroidPositionGenerator.GenerateNewAsteroidRotation());
        }

        private void SpawnNewAsteroid(int sizeIndex, Vector2 position, Quaternion rotation)
        {
            if (sizeIndex < 0 || sizeIndex >= _splitSteps.Length)
                throw new ArgumentOutOfRangeException(nameof(sizeIndex));

            Asteroid asteroid = _asteroidsPool.Spawn(position, rotation);

            AsteroidSizeStep step = _splitSteps[sizeIndex];

            asteroid.SetSize(step.Size, sizeIndex, step.Name);

            asteroid.Mover.ChangeSpeed(GenerateRandomSpeed);

            asteroid.Crashed += OnAsteroidCrashed;

            asteroid.DeSpawned += AsteroidOnDeSpawned;
        }

        private void AsteroidOnDeSpawned(PoolItem item)
        {
            if (!(item is Asteroid))
                return;

            Asteroid asteroid = (Asteroid) item;

            AnyCrashed?.Invoke();

            asteroid.Crashed -= OnAsteroidCrashed;

            asteroid.DeSpawned -= AsteroidOnDeSpawned;
        }

        private void OnAsteroidCrashed(Asteroid asteroid)
        {
            _splitter.TrySplitAsteroid(asteroid, SpawnNewAsteroid);
        }

        private void Awake()
        {
            _asteroidPositionGenerator = new AsteroidPositionGenerator(Camera.main);
            _splitter = new AsteroidSplitter(_splitSteps.Length, _angleSplit);
            Application.targetFrameRate = 0;
        }

        private void Start()
        {
            _asteroidsPool.Initialize();
        }

        private void OnValidate()
        {
            _maxSpeed = Mathf.Clamp(_maxSpeed, _minSpeed, _maxSpeed);
        }

        private IEnumerator Spawning()
        {
            var waitDelay = new WaitForSeconds(_newWaveDelay);

            while (true)
            {
                SpawnNewWave();

                yield return new WaitUntil(() => _asteroidsPool.SpawnedCount <= 0);

                yield return waitDelay;
            }
        }

        [Serializable]
        public struct AsteroidSizeStep
        {
            [Min(0)] [SerializeField] private float _size;

            [SerializeField] private AsteroidSize _name;

            public float Size => _size;

            public AsteroidSize Name => _name;
        }
    }
}