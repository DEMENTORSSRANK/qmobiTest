using System;
using Sources.Asteroids;
using Sources.Heal;
using Sources.Shoot;

namespace Sources.Player
{
    public class PlayerDamageTakerVisitor : IHitTakerVisitor
    {
        private readonly DamageParameters _damageParameters;

        private readonly Health _health;

        public PlayerDamageTakerVisitor(Health health, DamageParameters damageParameters)
        {
            _damageParameters = damageParameters ?? throw new ArgumentNullException(nameof(damageParameters));
            _health = health ?? throw new ArgumentNullException(nameof(health));
        }

        public void Visit(Asteroid asteroid)
        {
            _health.TakeDamage(_damageParameters.Asteroid);
        }

        public void Visit(Ship ship)
        {
            
        }

        public void Visit(Ufos.Ufo ufo)
        {
            _health.TakeDamage(_damageParameters.Ufo);
        }
    }
}