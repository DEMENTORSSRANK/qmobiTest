using System;
using UnityEngine;

namespace Sources.Heal
{
    public sealed class Health
    {
        private readonly int _startValue;

        public int Value { get; private set; }
        
        public bool IsImmortal { get; set; }

        public bool IsDead => Value <= 0;
        
        public event Action Dead;

        public event Action<int> Changed;

        public event Action Damaged;

        public Health(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            Value = value;
            _startValue = value;
        }

        public void TakeDamage(int damage)
        {
            if (IsDead)
                throw new InvalidOperationException("Already dead");
            
            if (IsImmortal)
                return;
            
            Value = Mathf.Clamp(Value - damage, 0, Value);
            
            Changed?.Invoke(Value);
            
            Damaged?.Invoke();
            
            TryDie();
        }

        public void Reset()
        {
            Value = _startValue;
            
            Changed?.Invoke(Value);
        }
        
        private void TryDie()
        {
            if (Value > 0)
                return;
            
            Dead?.Invoke();
        }
    }
}