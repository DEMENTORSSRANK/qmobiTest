using System;
using TMPro;
using UnityEngine;

namespace Sources.UserInterface
{
    [Serializable]
    public struct FormatViewText
    {
        [SerializeField] private TMP_Text _text;

        [SerializeField] private string _format;

        public void UpdateValue(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            _text.text = string.Format(_format, value);
        }

        public void UpdateValue(int value)
        {
            _text.text = string.Format(_format, value.ToString());
        }
    }
}