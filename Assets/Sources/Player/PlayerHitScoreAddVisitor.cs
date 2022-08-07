using System;
using Sources.Asteroids;
using Sources.Shoot;

namespace Sources.Player
{
    public class PlayerHitScoreAddVisitor : IHitTakerVisitor
    {
        private readonly Score _score;

        private readonly ScoreAddParameters _scoreAddParameters;

        public PlayerHitScoreAddVisitor(Score score, ScoreAddParameters scoreAddParameters)
        {
            _score = score ?? throw new ArgumentNullException(nameof(score));
            _scoreAddParameters = scoreAddParameters;
        }

        public void Visit(Asteroid asteroid)
        {
            _score.Add(_scoreAddParameters.GetScoreOfAsteroidSize(asteroid.NameSize));
        }

        public void Visit(Ship ship)
        {
            
        }

        public void Visit(Ufos.Ufo ufo)
        {
            _score.Add(_scoreAddParameters.Ufo);
        }
    }
}