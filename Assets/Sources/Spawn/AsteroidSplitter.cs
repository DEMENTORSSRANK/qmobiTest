using System;
using Sources.Asteroids;
using UnityEngine;

namespace Sources.Spawn
{
    public sealed class AsteroidSplitter
    {
        private readonly int _count;

        private readonly float _angle;

        public AsteroidSplitter(int count, float angle)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (angle < 0 || angle > 360)
                throw new ArgumentOutOfRangeException(nameof(angle));

            _count = count;
            _angle = angle;
        }

        public void TrySplitAsteroid(Asteroid asteroid, Action<int, Vector2, Quaternion> spawn)
        {
            int nextStepIndex = asteroid.StepIndex + 1;

            if (nextStepIndex >= _count)
                return;

            Transform asteroidTransform = asteroid.transform;

            Vector3 originalPosition = asteroidTransform.position;

            asteroidTransform.rotation.ToAngleAxis(out float originalAngle, out Vector3 angleAxis);

            spawn?.Invoke(nextStepIndex, originalPosition, Quaternion.AngleAxis(originalAngle + _angle, angleAxis));

            spawn?.Invoke(nextStepIndex, originalPosition, Quaternion.AngleAxis(originalAngle - _angle, angleAxis));
        }
    }
}