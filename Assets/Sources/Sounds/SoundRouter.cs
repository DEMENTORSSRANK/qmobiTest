using Sources.GameActive;
using Sources.Player;
using Sources.Shoot;
using Sources.Spawn;
using UnityEngine;

namespace Sources.Sounds
{
    public class SoundRouter : MonoBehaviour
    {
        [SerializeField] private SoundPlayer _soundPlayer;

        [SerializeField] private Ship _ship;

        [SerializeField] private AsteroidsSpawner _asteroidsSpawner;

        [SerializeField] private GameActivator _gameActivator;

        [SerializeField] private UfoSpawner _ufoSpawner;

        private void MainShooterOnShot(Shooter arg1, Transform arg2)
        {
            _soundPlayer.PlayShoot();
        }

        private void Subscribe()
        {
            _ship.Health.Damaged += _soundPlayer.PlayExplosion;
            _ship.Shooter.Shot += MainShooterOnShot;
            _asteroidsSpawner.AnyCrashed += _soundPlayer.PlayExplosion;
            _ufoSpawner.Crashed += _soundPlayer.PlayExplosion;
            _gameActivator.UnPaused += _soundPlayer.UnPause;
            _gameActivator.Paused += _soundPlayer.Pause;
            _ufoSpawner.Shot += _soundPlayer.PlayShoot;
        }

        private void UnSubscribe()
        {
            _ship.Health.Damaged -= _soundPlayer.PlayExplosion;
            _ship.Shooter.Shot -= MainShooterOnShot;
            _asteroidsSpawner.AnyCrashed -= _soundPlayer.PlayExplosion;
            _ufoSpawner.Crashed -= _soundPlayer.PlayExplosion;
            _gameActivator.UnPaused -= _soundPlayer.UnPause;
            _gameActivator.Paused -= _soundPlayer.Pause;
            _ufoSpawner.Shot -= _soundPlayer.PlayShoot;
        }

        private void OnEnable()
        {
            _gameActivator.GameStarted += Subscribe;
            _gameActivator.GameHasEnd += UnSubscribe;
        }

        private void OnDisable()
        {
            _gameActivator.GameStarted -= Subscribe;
            _gameActivator.GameHasEnd -= UnSubscribe;
        }
    }
}