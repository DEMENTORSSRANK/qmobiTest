using System;
using UnityEngine;

namespace Sources.Player
{
    [Serializable]
    public struct ShipVisual
    {
        [SerializeField] private GameObject[] _views;

        public void SetAllActive(bool isActive)
        {
            foreach (var view in _views)
            {
                view.SetActive(isActive);
            }
        }
    }
}