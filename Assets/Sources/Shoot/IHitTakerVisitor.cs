using Sources.Asteroids;
using Sources.Player;

namespace Sources.Shoot
{
    public interface IHitTakerVisitor
    {
        void Visit(Asteroid asteroid);

        void Visit(Ship ship);

        void Visit(Ufos.Ufo ufo);
    }
}