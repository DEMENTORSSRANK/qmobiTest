using System;
using System.Linq;
using Sources.Player;
using UnityEngine;

namespace Sources.Control
{
    [Serializable]
    public abstract class BaseControl
    {
        [SerializeField] private KeyCode _pause = KeyCode.Escape;

        [SerializeField] private KeyCode[] _shoot = {KeyCode.Space};

        [SerializeField] private KeyCode[] _move = {KeyCode.W};

        protected Ship Ship;

        protected Camera Camera;

        public event Action InputPause;

        public event Action InputShoot;

        public event Action InputStartThrust;

        public event Action InputEndThrust;

        public bool IsThrust { get; protected set; }

        public event Action<Quaternion> InputRotate;

        public void SetHelpers(Camera camera, Ship ship)
        {
            Ship = ship ? ship : throw new ArgumentNullException(nameof(ship));
            Camera = camera ? camera : throw new Exception("Camera is not initailized");
        }

        public void CheckInput()
        {
            if (Input.GetKeyDown(_pause))
                InputPause?.Invoke();

            if (_shoot.Any(Input.GetKeyDown))
                InputShoot?.Invoke();

            if (_move.Any(Input.GetKeyDown))
                InputStartThrust?.Invoke();
            
            if (_move.Any(Input.GetKeyUp))
                InputEndThrust?.Invoke();
            
            IsThrust = _move.Any(Input.GetKey);
            
            AdaptiveCheckInput();
        }

        protected virtual void AdaptiveCheckInput()
        {
            
        }

        protected void InvokeInputRotate(Quaternion rotation)
        {
            InputRotate?.Invoke(rotation);
        }
    }
}