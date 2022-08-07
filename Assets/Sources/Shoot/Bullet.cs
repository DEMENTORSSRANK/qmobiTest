using System;
using System.Collections.Generic;
using Sources.Collisions;
using Sources.Movement;
using Sources.Shoot;
using Sources.Spawn;
using UnityEngine;

namespace Sources
{
    [RequireComponent(typeof(CollisionDetector), typeof(ForeverMover))]
    public sealed class Bullet : PoolItem
    {
        [SerializeField] private int _damage = 10;

        [SerializeField] private SpriteRenderer _bulletView;

        private CollisionDetector _detector;

        private ForeverMover _mover;

        private BulletEnterVisitor _enterVisitor;

        private IShooter _sender;
        
        private readonly Stack<IHitTaker> _expect = new Stack<IHitTaker>();
        
        public bool CanDamage { get; set; }

        public void SetColor(Color color)
        {
            _bulletView.color = color;
        }
        
        public void SetSender(IShooter sender)
        {
            _sender = sender;
        }
        
        public void AddExpect(IHitTaker hitTaker)
        {
            if (_expect.Contains(hitTaker))
                throw new InvalidOperationException("Space object is already in expect");

            _expect.Push(hitTaker);
        }

        public void ClearExpect()
        {
            _expect.Clear();
        }
        
        private void DetectorOnEntered(CollisionDetector other)
        {
            if (!CanDamage)
                return;
            
            if (!other.TryGetComponent<IHitTaker>(out var hitTaker))
                return;

            if (_expect.Contains(hitTaker))
                return;

            DeSpawn();
            
            hitTaker.Accept(_enterVisitor);

            _sender.OnHitOther(hitTaker);
        }

        private void PositionRepeatOnOnRepeat()
        {
            DeSpawn();
        }

        private void Awake()
        {
            _detector = GetComponent<CollisionDetector>();
            _mover = GetComponent<ForeverMover>();
            _enterVisitor = new BulletEnterVisitor(_damage);
        }

        private void OnEnable()
        {
            _detector.Entered += DetectorOnEntered;
            _mover.PositionRepeat.OnRepeat += PositionRepeatOnOnRepeat;
        }

        private void FixedUpdate()
        {
            _mover.MoveTo(transform.up);
        }

        private void OnDisable()
        {
            _detector.Entered -= DetectorOnEntered;
            _mover.PositionRepeat.OnRepeat -= PositionRepeatOnOnRepeat;
        }
    }
}