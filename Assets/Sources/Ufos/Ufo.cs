using System;
using System.Collections;
using Sources.Asteroids;
using Sources.Collisions;
using Sources.Movement;
using Sources.Player;
using Sources.Shoot;
using Sources.Spawn;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Ufos
{
    [RequireComponent(typeof(ForeverMover), typeof(CollisionDetector), typeof(Shooter))]
    public class Ufo : PoolItem, IHitTaker
    {
        [Min(0)] [SerializeField] private float _minDelayShoot = 2;

        [Min(0)] [SerializeField] private float _maxDelayShoot = 4;

        [Min(0)] [SerializeField] private float _distanceTargetFollow = 4;

        private ForeverMover _mover;

        private Transform _target;

        private CollisionDetector _detector;

        public Shooter Shooter { get; private set; }
        
        public TargetDirectionGenerator TargetFollower { get; private set; }

        private float RandomDelayShoot => Random.Range(_minDelayShoot, _maxDelayShoot);

        public event Action Crashed;

        public void Crash()
        {
            Crashed?.Invoke();

            DeSpawn();
        }

        public void Accept(IHitTakerVisitor visitor)
        {
            visitor.Visit(this);
        }

        private void PositionRepeatOnOnRepeat()
        {
            DeSpawn();
        }

        private void ShootToTarget()
        {
            Shooter.Shoot(TargetFollower.GetTargetDirection());
        }

        private void DetectorOnEntered(CollisionDetector other)
        {
            if (other.GetComponent<Asteroid>() || other.GetComponent<Ship>())
                DeSpawn();
        }

        private void Awake()
        {
            TargetFollower = new TargetDirectionGenerator(transform, _distanceTargetFollow);
            Shooter = GetComponent<Shooter>();
            _mover = GetComponent<ForeverMover>();
            _detector = GetComponent<CollisionDetector>();
        }

        private void OnEnable()
        {
            _mover.MoveTo(TargetFollower.GenerateCurrentMoveDirection());

            _mover.PositionRepeat.OnRepeat += PositionRepeatOnOnRepeat;

            _detector.Entered += DetectorOnEntered;

            StartCoroutine(Shooting());
        }

        private void FixedUpdate()
        {
            _mover.MoveTo(TargetFollower.GenerateCurrentMoveDirection());
        }

        private void OnDisable()
        {
            _mover.PositionRepeat.OnRepeat -= PositionRepeatOnOnRepeat;

            _detector.Entered -= DetectorOnEntered;
        }

        private void OnValidate()
        {
            _maxDelayShoot = Mathf.Clamp(_maxDelayShoot, _minDelayShoot, _maxDelayShoot);
        }

        private IEnumerator Shooting()
        {
            while (true)
            {
                yield return new WaitForSeconds(RandomDelayShoot);

                ShootToTarget();
            }
        }
    }
}