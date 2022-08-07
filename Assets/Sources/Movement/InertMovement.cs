using System;
using UnityEngine;

namespace Sources.Movement
{
    public sealed class InertMovement
    {
        private readonly float _maxSpeed;

        private readonly float _unitsPerSecond;

        private const float SecondsToStop = 1f;

        public Vector2 Acceleration { get; private set; }

        public InertMovement(float maxSpeed, float unitsPerSecond)
        {
            if (maxSpeed < 0)
                throw new ArgumentOutOfRangeException(nameof(maxSpeed));
            
            if (unitsPerSecond < 0)
                throw new ArgumentOutOfRangeException(nameof(unitsPerSecond));

            _maxSpeed = maxSpeed;
            _unitsPerSecond = unitsPerSecond / 1000;
        }

        public void Accelerate(Vector2 forward, float deltaTime)
        {
            Acceleration += forward * (_unitsPerSecond * deltaTime);

            Acceleration = Vector2.ClampMagnitude(Acceleration, _maxSpeed);
        }

        public void Slowdown(float deltaTime)
        {
            Acceleration -= Acceleration * (deltaTime / SecondsToStop);
        }

        public void Reset()
        {
            Acceleration = Vector2.zero;
        }
    }
}