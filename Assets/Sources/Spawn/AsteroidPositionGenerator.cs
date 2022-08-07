using System;
using Sources.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Spawn
{
    public class AsteroidPositionGenerator
    {
        private readonly Vector2 _leftCorner;
        
        private readonly Vector2 _rightCorner;

        public AsteroidPositionGenerator(Camera camera)
        {
            if (camera == null)
                throw new ArgumentNullException(nameof(camera));
            
            _leftCorner = camera.GetLeftLowerCornerWorldPosition();
            _rightCorner = camera.GetRightUpperCornerWorldPosition();
        }

        public Vector2 GenerateNewAsteroidPosition()
        {
            var position = new Vector2(Random.Range(_leftCorner.x, _rightCorner.x),
                Random.Range(_leftCorner.y, _rightCorner.y));

            ClampPositionToRandomSide(ref position);

            return position;
        }
        
        public Quaternion GenerateNewAsteroidRotation()
        {
            return Quaternion.AngleAxis(Random.Range(0, 360f), Vector3.back);
        }

        private void ClampPositionToRandomSide(ref Vector2 position)
        {
            SpawnSide side = (SpawnSide) Random.Range(0, Enum.GetValues(typeof(SpawnSide)).Length);

            switch (side)
            {
                case SpawnSide.Bottom:
                    position.y = _leftCorner.y;
                    break;
                case SpawnSide.Up:
                    position.y = _rightCorner.y;
                    break;
                case SpawnSide.Left:
                    position.x = _leftCorner.x;
                    break;
                case SpawnSide.Right:
                    position.x = _rightCorner.x;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}