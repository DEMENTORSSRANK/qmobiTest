using System;
using UnityEngine;

namespace Sources.Movement
{
    public sealed class ForeverMover : Mover
    {
        [Min(0)] [SerializeField] private float _speed = 10;

        public override void MoveTo(Vector2 direction)
        {
            transform.position += (Vector3) direction * (_speed * Time.fixedDeltaTime);
        }

        public void ChangeSpeed(float speed)
        {
            if (speed < 0)
                throw new ArgumentOutOfRangeException(nameof(speed));

            _speed = speed;
        }
    }
}