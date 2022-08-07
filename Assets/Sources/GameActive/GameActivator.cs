using System;
using System.Threading.Tasks;
using Sources.Player;
using Sources.Spawn;
using UnityEngine;

namespace Sources.GameActive
{
    public sealed class GameActivator : MonoBehaviour
    {
        [SerializeField] private Ship _ship;

        [SerializeField] private Spawner[] _spawners;

        public bool IsGameStarted { get; private set; }

        public bool IsPaused { get; private set; }

        public event Action Paused;

        public event Action UnPaused;

        public event Action GameHasEnd;

        public event Action GameStarted;

        public event Action<bool> GameStateUpdated;

        public void TogglePause()
        {
            if (!IsGameStarted)
                return;

            if (IsPaused)
                UnPause();
            else
                Pause();
        }

        public void Pause()
        {
            if (!IsGameStarted)
                return;

            if (IsPaused)
                return;

            IsPaused = true;

            Time.timeScale = 0;

            Paused?.Invoke();
        }

        public void UnPause()
        {
            if (!IsGameStarted)
                return;

            if (!IsPaused)
                return;

            IsPaused = false;

            Time.timeScale = 1;

            UnPaused?.Invoke();
        }

        public void RestartGame()
        {
            if (!IsGameStarted)
                throw new InvalidOperationException("Game is not started yet");

            EndGame();
            
            StartGame();
        }

        public void StartGame()
        {
            if (IsGameStarted)
                throw new InvalidOperationException("Game is already started");

            IsGameStarted = true;

            GameStarted?.Invoke();

            GameStateUpdated?.Invoke(IsGameStarted);

            foreach (var spawner in _spawners)
                spawner.StartSpawning();

            _ship.Health.Reset();
            
            _ship.Score.Reset();
            
            Time.timeScale = 1;

            IsPaused = false;
        }

        public void EndGame()
        {
            if (!IsGameStarted)
                throw new InvalidOperationException("Game is not started yet");
            
            IsGameStarted = false;

            GameHasEnd?.Invoke();

            GameStateUpdated?.Invoke(IsGameStarted);

            foreach (var spawner in _spawners)
                spawner.ClearSpawnedAndStopSpawning();
        }

        private async void OnEnable()
        {
            await Task.Delay(1);

            _ship.Health.Dead += EndGame;
        }

        private void OnDisable()
        {
            _ship.Health.Dead -= EndGame;
        }
    }
}