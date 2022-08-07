using System;
using UnityEngine;

namespace Sources.Movement
{
    public sealed class InertMover : Mover
    {
        [Min(0)] [SerializeField] private float _maxSpeed = 10;

        [Min(0)] [SerializeField] private float _acceleration;

        private InertMovement _movement;

        public void ResetAcceleration()
        {
            _movement.Reset();
        }
        
        public override void MoveTo(Vector2 direction)
        {
            if (Vector2.zero == direction)
                return;

            _movement.Accelerate(direction * _maxSpeed, Time.fixedDeltaTime);
        }

        protected override void OnAwake()
        {
            _movement = new InertMovement(_maxSpeed, _acceleration);
        }

        private void FixedUpdate()
        {
            if (Math.Abs(Time.timeScale) <= 0)
                return;
            
            _movement.Slowdown(Time.fixedDeltaTime);
            
            transform.position += (Vector3) _movement.Acceleration;
        }
    }
}