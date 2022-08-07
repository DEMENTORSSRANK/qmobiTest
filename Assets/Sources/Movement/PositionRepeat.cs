using System;
using UnityEngine;

namespace Sources.Movement
{
    public class PositionRepeat : IReadOnlyPositionRepeat
    {
        private readonly Vector2 _rightUpCorner;

        private readonly Vector2 _leftLowerCorner;

        private Vector2 _lastRepeat;

        public event Action OnRepeat;

        public PositionRepeat(Vector2 rightUpCorner, Vector2 leftLowerCorner)
        {
            if (rightUpCorner.x < leftLowerCorner.x)
                throw new ArgumentException("Corners initialization failed");

            _rightUpCorner = rightUpCorner;
            _leftLowerCorner = leftLowerCorner;
        }

        public Vector2 RepeatForCorners(Vector2 position)
        {
            return Repeat(position, _leftLowerCorner, _rightUpCorner);
        }

        private Vector2 Repeat(Vector2 original, Vector2 min, Vector2 max) =>
            new Vector2(Repeat(original.x, min.x, max.x), Repeat(original.y, min.y, max.y));

        private float Repeat(float original, float min, float max)
        {
            bool isAnyRepeat = original > max || original < min;

            if (isAnyRepeat)
                OnRepeat?.Invoke();

            return isAnyRepeat ? original > max ? min : max : original;
        }
    }
}