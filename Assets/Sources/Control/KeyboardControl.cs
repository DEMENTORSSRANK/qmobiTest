using System;
using System.Linq;
using UnityEngine;

namespace Sources.Control
{
    [Serializable]
    public sealed class KeyboardControl : BaseControl
    {
        [Min(0)] [SerializeField] private float _forceRotate = .3f;

        [SerializeField] private KeyCode[] _rightRotate;

        [SerializeField] private KeyCode[] _leftRotate;

        protected override void AdaptiveCheckInput()
        {
            Vector3 euler = Ship.transform.eulerAngles;

            if (_leftRotate.Any(Input.GetKey))
            {
                euler.z += _forceRotate;
                
                InvokeInputRotate(Quaternion.Euler(euler));
            }

            if (_rightRotate.Any(Input.GetKey))
            {
                euler.z -= _forceRotate;
                
                InvokeInputRotate(Quaternion.Euler(euler));
            }
        }
    }
}