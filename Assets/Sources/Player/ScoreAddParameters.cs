using System;
using System.Linq;
using Sources.Asteroids;
using UnityEngine;

namespace Sources.Player
{
    [Serializable]
    public struct ScoreAddParameters
    {
        [SerializeField] private ScoreAsteroidSize[] _asteroids;

        [Min(0)] [SerializeField] private int _ufo;

        public int Ufo => _ufo;

        public readonly int GetScoreOfAsteroidSize(AsteroidSize size) => _asteroids.First(x => x.Size == size).Score;

        [Serializable]
        private struct ScoreAsteroidSize
        {
            [SerializeField] private AsteroidSize _size;

            [Min(0)] [SerializeField] private int _score;

            public AsteroidSize Size => _size;

            public int Score => _score;
        }
    }
}