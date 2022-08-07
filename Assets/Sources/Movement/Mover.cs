using Sources.Extensions;
using UnityEngine;

namespace Sources.Movement
{
    public abstract class Mover : MonoBehaviour
    {
        private PositionRepeat _positionRepeat;

        private Transform _transform;

        public IReadOnlyPositionRepeat PositionRepeat => _positionRepeat ?? InitPositionRepeat();

        public abstract void MoveTo(Vector2 direction);

        private PositionRepeat InitPositionRepeat()
        {
            if (_positionRepeat != null)
                return null;

            _positionRepeat = new PositionRepeat(Camera.main.GetRightUpperCornerWorldPosition(),
                Camera.main.GetLeftLowerCornerWorldPosition());

            return _positionRepeat;
        }

        private void ClampPosition()
        {
            Vector3 position = _transform.position;

            position = _positionRepeat.RepeatForCorners(position);

            _transform.position = position;
        }

        private void Awake()
        {
            _transform = transform;

            InitPositionRepeat();

            OnAwake();
        }

        private void Update()
        {
            OnUpdate();

            ClampPosition();
        }

        protected virtual void OnAwake()
        {
        }

        protected virtual void OnUpdate()
        {
        }
    }
}