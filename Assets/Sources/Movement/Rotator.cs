using System;
using UnityEngine;

namespace Sources.Movement
{
    public sealed class Rotator
    {
        private readonly float _rotateSpeed;

        private readonly Transform _target;

        public Rotator(float rotateSpeed, Transform target)
        {
            if (rotateSpeed < 0)
                throw new ArgumentOutOfRangeException(nameof(rotateSpeed));

            _rotateSpeed = rotateSpeed;
            _target = target ? target : throw new ArgumentNullException(nameof(target));
        }

        public void RotateTo(Vector2 direction)
        {
            RotateTo(GenerateRotationByAngle(GenerateAngleByDirection(direction)));
        }

        public void RotateTo(Quaternion rotation)
        {
            _target.rotation = Quaternion.Slerp(_target.rotation, rotation, Time.deltaTime * _rotateSpeed);
        }

        public static float GenerateAngleByDirection(Vector2 direction) =>
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        public static Quaternion GenerateRotationByAngle(float angle) =>
            Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}