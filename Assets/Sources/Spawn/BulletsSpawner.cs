using System.Collections.Generic;
using Sources.Shoot;
using UnityEngine;

namespace Sources.Spawn
{
    public class BulletsSpawner : Spawner
    {
        [SerializeField] private ObjectPool<Bullet> _pool;

        [SerializeField] private List<Shooter> _shooters;

        [SerializeField] private BulletColorConfig _config;

        private BulletColorSetterVisitor _bulletColorSetter;
        
        public override void StartSpawning()
        {
            foreach (var shooter in _shooters)
            {
                shooter.Shot += CreateBulletFromCrossHair;
            }
        }

        public override void ClearSpawnedAndStopSpawning()
        {
            foreach (var shooter in _shooters)
            {
                shooter.Shot -= CreateBulletFromCrossHair;
            }
            
            _pool.Clear();
        }
        
        public void AddShooter(Shooter shooter)
        {
            if (_shooters.Contains(shooter))
                return;

            shooter.Shot += CreateBulletFromCrossHair;
            
            _shooters.Add(shooter);
        }

        public void RemoveShooter(Shooter shooter)
        {
            if (!_shooters.Contains(shooter))
                return;

            shooter.Shot -= CreateBulletFromCrossHair;
            
            _shooters.Remove(shooter);
        }

        private void CreateBulletFromCrossHair(Shooter from, Transform crossHair)
        {
            var bullet = _pool.Spawn(crossHair.position, crossHair.rotation);

            bullet.CanDamage = false;
            
            bullet.SetSender(from);
            
            bullet.ClearExpect();
            
            _bulletColorSetter.SetBullet(bullet);

            if (from.TryGetComponent<IHitTaker>(out var spaceObject))
                bullet.AddExpect(spaceObject);

            spaceObject.Accept(_bulletColorSetter);
            
            bullet.CanDamage = true;
        }

        private void Awake()
        {
            _bulletColorSetter = new BulletColorSetterVisitor(_config);
            
            _pool.Initialize();
        }
    }
}