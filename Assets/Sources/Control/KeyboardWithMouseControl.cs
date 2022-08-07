using Sources.Movement;
using UnityEngine;

namespace Sources.Control
{
    public sealed class KeyboardWithMouseControl : BaseControl
    {
        protected override void AdaptiveCheckInput()
        {
            InvokeInputRotate(Rotator.GenerateRotationByAngle(Rotator.GenerateAngleByDirection(CalculateToMousePositionDirection())));
        }

        private Vector2 CalculateToMousePositionDirection()
        {
            Vector3 mousePosition = Camera.ScreenToWorldPoint(Input.mousePosition);

            return (mousePosition - Ship.transform.position).normalized;
        }
    }
}