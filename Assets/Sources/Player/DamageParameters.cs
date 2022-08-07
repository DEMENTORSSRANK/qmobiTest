using System;
using UnityEngine;

namespace Sources.Player
{
    [Serializable]
    public class DamageParameters
    {
        [Min(0)] [SerializeField] private int _asteroid = 1;
        
        [Min(0)] [SerializeField] private int _ufo = 1;

        public int Asteroid => _asteroid;

        public int Ufo => _ufo;
    }
}