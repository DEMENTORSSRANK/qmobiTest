using System;

namespace Sources.Player
{
    public class Score
    {
        public int Value { get; private set; }

        public event Action<int> Changed;

        public void Add(int score)
        {
            if (score <= 0)
                throw new ArgumentOutOfRangeException(nameof(score));

            Value += score;
            
            Changed?.Invoke(Value);
        }

        public void Reset()
        {
            Value = 0;
            
            Changed?.Invoke(Value);
        }
    }
}