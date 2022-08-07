using System;
using System.Collections;
using System.Threading.Tasks;
using Sources.Player;
using UnityEngine;

namespace Sources.GameActive
{
    public sealed class ShipReSpawn : MonoBehaviour
    {
        [SerializeField] private Vector2 _startPosition;

        [SerializeField] private Ship _ship;

        [Min(0)] [SerializeField] private float _immortalityTime = 3;

        [Min(0)] [SerializeField] private float _pickInvisibleDelay = .5f;

        [SerializeField] private GameActivator _activator;

        private float _currentTime;
        
        private Coroutine _respawning;

        private Coroutine _picking;

        public bool IsReSpawning => _currentTime < _immortalityTime;
        
        public void ReSpawn()
        {
            if (IsReSpawning)
            {
                StopCoroutine(_picking);
                
                StopCoroutine(_respawning);
            }

            _ship.transform.position = _startPosition;
            
            _ship.Mover.ResetAcceleration();
            
            _respawning = StartCoroutine(ReSpawning(_immortalityTime));

            _picking = StartCoroutine(Picking(_pickInvisibleDelay, () => IsReSpawning));
        }

        private async void OnEnable()
        {
            await Task.Delay(1);
            
            _ship.Health.Damaged += ReSpawn;
            _activator.GameStarted += ReSpawn;
        }

        private void Start()
        {
            _currentTime = _immortalityTime;
        }

        private void OnDisable()
        {
            _ship.Health.Damaged -= ReSpawn;
            _activator.GameStarted -= ReSpawn;
        }

        private IEnumerator Picking(float delay, Func<bool> condition)
        {
            var wait = new WaitForSeconds(delay);
            
            while (condition.Invoke())
            {
                _ship.Visual.SetAllActive(false);
            
                yield return wait;
                
                _ship.Visual.SetAllActive(true);
                
                yield return wait;
            }
            
            _ship.Visual.SetAllActive(true);
        }

        private IEnumerator ReSpawning(float time)
        {
            _ship.Health.IsImmortal = true;
            
            _currentTime = 0;
            
            while (_currentTime < time)
            {
                _currentTime += Time.deltaTime;

                yield return null;
            }

            _ship.Health.IsImmortal = false;
        }
    }
}