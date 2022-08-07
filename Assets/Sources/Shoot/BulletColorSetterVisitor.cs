using System;
using Sources.Asteroids;
using Sources.Player;

namespace Sources.Shoot
{
    public class BulletColorSetterVisitor : IHitTakerVisitor
    {
        private Bullet _bullet;

        private readonly BulletColorConfig _config;

        public BulletColorSetterVisitor(BulletColorConfig config)
        {
            _config = config;
        }

        public void SetBullet(Bullet bullet)
        {
            _bullet = bullet ? bullet : throw new ArgumentNullException(nameof(bullet));
        }

        public void Visit(Asteroid asteroid)
        {
            
        }

        public void Visit(Ship ship)
        {
            _bullet.SetColor(_config.Player);
        }

        public void Visit(Ufos.Ufo ufo)
        {
            _bullet.SetColor(_config.Ufo);
        }
    }
}