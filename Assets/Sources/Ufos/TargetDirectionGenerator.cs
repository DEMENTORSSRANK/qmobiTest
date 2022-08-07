using System;
using UnityEngine;

namespace Sources.Ufos
{
    public class TargetDirectionGenerator
    {
        private readonly Transform _follower;
        
        private readonly float _followDistance;

        private Transform _target;

        private bool _targetWasSet;
        
        private Vector2 TargetPosition => _target.localPosition;

        private Vector2 FollowerPosition => _follower.position;
        
        public bool IsRightMove { get; set; }

        public float CurrentDistance => Vector2.Distance(FollowerPosition, TargetPosition);

        public TargetDirectionGenerator(Transform follower, float followDistance)
        {
            if (followDistance < 0)
                throw new ArgumentOutOfRangeException(nameof(followDistance));
            
            _follower = follower ? follower : throw new ArgumentNullException(nameof(follower));
            _followDistance = followDistance;
        }
        
        public void UpdateTarget(Transform target)
        {
            _target = target;
            _targetWasSet = true;
        }

        public Vector2 GetTargetDirection() => (TargetPosition - FollowerPosition).normalized;

        public Vector2 GenerateCurrentMoveDirection()
        {
            if (!_targetWasSet || CurrentDistance > _followDistance)
                return IsRightMove ? Vector2.right : Vector2.left;
            
            return GetTargetDirection();
        }
    }
}