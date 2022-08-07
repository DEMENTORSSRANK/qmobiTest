using System;
using UnityEngine;

namespace Sources.Collisions
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public sealed class CollisionDetector : MonoBehaviour
    {
        public event Action<CollisionDetector> Entered; 
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.TryGetComponent(out CollisionDetector entered))
                return;
            
            Entered?.Invoke(entered);
        }
    }
}