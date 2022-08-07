using System;
using UnityEngine;

namespace Sources.UserInterface
{
    [Serializable]
    public class ToggleView
    {
        [SerializeField] private string _firstName = "Keyboard";

        [SerializeField] private string _secondName = "Keyboard + Mouse";

        [SerializeField] private FormatViewText _name;
        
        private int _lastIndex;

        public int GetOppositeLastIndex => _lastIndex == 0 ? 1 : 0; 
        
        public void IndexChanged(int index)
        {
            if (index > 1 || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            string correctName = index == 0 ? _firstName : _secondName;
            
            _name.UpdateValue(correctName);

            _lastIndex = index;
        }
    }
}