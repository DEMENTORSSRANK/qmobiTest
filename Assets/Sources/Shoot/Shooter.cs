using System;
using System.Collections;
using Sources.Movement;
using UnityEngine;

namespace Sources.Shoot
{
    public sealed class Shooter : MonoBehaviour, IShooter
    {
        [SerializeField] private Transform _crossHair;

        [Min(0)] [SerializeField] private float _reloadTime = 0.33f;

        private Rotator _crossHairRotator;

        public bool CanShoot { get; private set; } = true;
        
        public event Action<Shooter, Transform> Shot;

        public event Action<IHitTaker> Hit;

        void IShooter.OnHitOther(IHitTaker other)
        {
            Hit?.Invoke(other);
        }

        public void Shoot()
        {
            if (!CanShoot)
                return;
            
            Shot?.Invoke(this, _crossHair);

            StartCoroutine(Reloading());
        }

        public void Shoot(Vector2 direction)
        {
            if (!CanShoot)
                return;
            
            _crossHairRotator.RotateTo(direction);

            Shoot();
        }

        private void Awake()
        {
            _crossHairRotator = new Rotator(5000, _crossHair);
        }

        private IEnumerator Reloading()
        {
            CanShoot = false;
            
            yield return new WaitForSeconds(_reloadTime);

            CanShoot = true;
        }
    }
}