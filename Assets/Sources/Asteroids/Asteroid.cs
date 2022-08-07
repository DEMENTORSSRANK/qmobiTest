using System;
using Sources.Collisions;
using Sources.Movement;
using Sources.Shoot;
using Sources.Spawn;
using UnityEngine;

namespace Sources.Asteroids
{
    [RequireComponent(typeof(ForeverMover), typeof(CollisionDetector))]
    public class Asteroid : PoolItem, IHitTaker
    {
        private CollisionDetector _collisionDetector;

        public int StepIndex { get; private set; }

        public AsteroidSize NameSize { get; private set; }

        public ForeverMover Mover { get; private set; }

        public event Action<Asteroid> Crashed; 

        public void SetSize(float size, int sizeIndex, AsteroidSize nameSize)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size));
            
            transform.localScale = new Vector3(size, size, 1);

            StepIndex = sizeIndex;

            NameSize = nameSize;
        }

        public void Accept(IHitTakerVisitor visitor)
        {
            visitor.Visit(this);
        }

        private void CollisionDetectorOnEntered(CollisionDetector detector)
        {
            if (!detector.TryGetComponent<IHitTaker>(out _))
                return;
            
            DeSpawn();
        }

        public void Crash()
        {
            Crashed?.Invoke(this);
            
            DeSpawn();
        }

        private void Awake()
        {
            Mover = GetComponent<ForeverMover>();
            _collisionDetector = GetComponent<CollisionDetector>();
        }

        private void OnEnable()
        {
            _collisionDetector.Entered += CollisionDetectorOnEntered;
        }

        private void FixedUpdate()
        {
            Mover.MoveTo(transform.up);
        }

        private void OnDisable()
        {
            _collisionDetector.Entered -= CollisionDetectorOnEntered;
        }
    }
}