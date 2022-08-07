using System;

namespace Sources.Movement
{
    public interface IReadOnlyPositionRepeat
    {
        event Action OnRepeat;
    }
}