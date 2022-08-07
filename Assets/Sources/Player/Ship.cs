using Sources.Collisions;
using Sources.Heal;
using Sources.Movement;
using Sources.Shoot;
using UnityEngine;

namespace Sources.Player
{
    [RequireComponent(typeof(CollisionDetector), typeof(Shooter), typeof(InertMover))]
    public sealed class Ship : MonoBehaviour, IHitTaker
    {
        [Min(1)] [SerializeField] private int _startHealth = 100;

        [Min(0)] [SerializeField] private float _rotateSpeed = 15;

        [SerializeField] private ShipVisual _shipVisual;

        [SerializeField] private ScoreAddParameters _scoreAddParameters;

        [SerializeField] private DamageParameters _damageParameters;

        private PlayerHitScoreAddVisitor _scoreAddVisitor;

        private PlayerDamageTakerVisitor _damageTakerVisitor;

        private CollisionDetector _collisionDetector;

        public Health Health { get; private set; }

        public Score Score { get; private set; }

        public Shooter Shooter { get; private set; }

        public InertMover Mover { get; private set; }
        
        public Rotator Rotator { get; private set; }

        public ShipVisual Visual => _shipVisual;

        public void Thrust()
        {
            Mover.MoveTo(transform.up);
        }

        public void Accept(IHitTakerVisitor visitor)
        {
            visitor.Visit(this);
        }

        private void ShooterOnHit(IHitTaker other)
        {
            other.Accept(_scoreAddVisitor);
        }

        private void CollisionDetectorOnEntered(CollisionDetector other)
        {
            if (!other.TryGetComponent(out IHitTaker hitTaker))
                return;
            
            hitTaker.Accept(_damageTakerVisitor);
        }

        private void Awake()
        {
            Health = new Health(_startHealth);
            Score = new Score();
            _scoreAddVisitor = new PlayerHitScoreAddVisitor(Score, _scoreAddParameters);
            _damageTakerVisitor = new PlayerDamageTakerVisitor(Health, _damageParameters);
            Rotator = new Rotator(_rotateSpeed, transform);
            Shooter = GetComponent<Shooter>();
            Mover = GetComponent<InertMover>();
            _collisionDetector = GetComponent<CollisionDetector>();
        }

        private void OnEnable()
        {
            Shooter.Hit += ShooterOnHit;
            _collisionDetector.Entered += CollisionDetectorOnEntered;
        }

        private void OnDisable()
        {
            Shooter.Hit -= ShooterOnHit;
            _collisionDetector.Entered -= CollisionDetectorOnEntered;
        }
    }
}