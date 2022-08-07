using System;
using Sources.GameActive;
using Sources.Player;
using Sources.Sounds;
using UnityEngine;

namespace Sources.Control
{
    public sealed class ControlManagement : MonoBehaviour
    {
        [SerializeReference] [SerializeField] private BaseControl[] _allowedControls;

        [SerializeField] private Ship _ship;

        [SerializeField] private GameActivator _activator;

        [Min(0)] [SerializeField] private int _startControlIndex;

        [SerializeField] private SoundPlayer _soundPlayer;

        private bool _controlSet;

        public event Action<int> ControlUpdated;

        public int CurrentControlIndex { get; private set; }

        public BaseControl CurrentControl { get; private set; }

        public void SetCurrentControl(int index)
        {
            if (index < 0 || index >= _allowedControls.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (CurrentControl != null)
                UnSubscribeCurrentControl();

            CurrentControl = _allowedControls[index];

            _controlSet = true;

            CurrentControlIndex = index;

            ControlUpdated?.Invoke(index);

            if (_activator.IsGameStarted && !_activator.IsPaused)
                SubscribeCurrentControl();
        }

        private void SubscribeCurrentControl()
        {
            CurrentControl.InputShoot += _ship.Shooter.Shoot;
            CurrentControl.InputRotate += _ship.Rotator.RotateTo;
            CurrentControl.InputPause += _activator.TogglePause;
            CurrentControl.InputStartThrust += _soundPlayer.StartPlayThrust;
            CurrentControl.InputEndThrust += _soundPlayer.EndPlayThrust;
        }

        private void UnSubscribeCurrentControl()
        {
            CurrentControl.InputShoot -= _ship.Shooter.Shoot;
            CurrentControl.InputRotate -= _ship.Rotator.RotateTo;
            CurrentControl.InputPause -= _activator.TogglePause;
            CurrentControl.InputStartThrust -= _soundPlayer.StartPlayThrust;
            CurrentControl.InputEndThrust -= _soundPlayer.EndPlayThrust;
        }

        private void Awake()
        {
            Camera mainCamera = Camera.main;
            
            foreach (var control in _allowedControls)
                control.SetHelpers(mainCamera, _ship);
        }

        private void OnEnable()
        {
            _activator.GameStarted += SubscribeCurrentControl;
            _activator.GameHasEnd += UnSubscribeCurrentControl;
            _activator.Paused += UnSubscribeCurrentControl;
            _activator.UnPaused += SubscribeCurrentControl;
        }

        private void Start()
        {
            SetCurrentControl(_startControlIndex);
        }

        private void Update()
        {
            if (!_controlSet)
                return;

            CurrentControl.CheckInput();
        }

        private void FixedUpdate()
        {
            if (!_controlSet)
                return;
            
            if (CurrentControl.IsThrust)
                _ship.Thrust();
        }

        private void OnDisable()
        {
            _activator.GameStarted -= SubscribeCurrentControl;
            _activator.GameHasEnd -= UnSubscribeCurrentControl;
            _activator.Paused -= UnSubscribeCurrentControl;
            _activator.UnPaused -= SubscribeCurrentControl;
        }

        private void OnValidate()
        {
            _startControlIndex = Mathf.Clamp(_startControlIndex, 0, _allowedControls.Length - 1);
        }
    }
}