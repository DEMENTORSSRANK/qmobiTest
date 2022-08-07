using System;
using UnityEngine;

namespace Sources.Shoot
{
    [Serializable]
    public struct BulletColorConfig
    {
        [SerializeField] private Color _player;

        [SerializeField] private Color _ufo;

        public Color Player => _player;

        public Color Ufo => _ufo;
    }
}