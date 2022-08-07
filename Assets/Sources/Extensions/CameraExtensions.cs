using UnityEngine;

namespace Sources.Extensions
{
    public static class CameraExtensions
    {
        public static Vector2 GetRightUpperCornerWorldPosition(this Camera camera) =>
            camera.ViewportToWorldPoint(Vector3.one);

        public static Vector2 GetLeftLowerCornerWorldPosition(this Camera camera) =>
            -camera.GetRightUpperCornerWorldPosition();
    }
}