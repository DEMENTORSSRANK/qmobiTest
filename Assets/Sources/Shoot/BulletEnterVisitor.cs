using System;
using Sources.Asteroids;
using Sources.Player;
using Object = UnityEngine.Object;

namespace Sources.Shoot
{
    public class BulletEnterVisitor : IHitTakerVisitor
    {
        private readonly int _damage;

        public BulletEnterVisitor(int damage)
        {
            if (damage <= 0)
                throw new ArgumentOutOfRangeException(nameof(damage));
            
            _damage = damage;
        }

        public void Visit(Asteroid asteroid)
        {
            asteroid.Crash();
        }

        public void Visit(Ship ship)
        {
            ship.Health.TakeDamage(_damage);
        }

        public void Visit(Ufos.Ufo ufo)
        {
            ufo.Crash();
        }
    }
}