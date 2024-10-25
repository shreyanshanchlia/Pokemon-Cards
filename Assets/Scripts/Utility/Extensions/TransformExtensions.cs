using UnityEngine;

namespace Utility.Extensions
{
    public static class TransformExtensions
    {
        public static void SetLossyScale(this Transform transform, Vector3 newScale)
        {
            var lossyScale = transform.lossyScale;
            var localScale = transform.localScale;
            var relativeScale = new Vector3(
                newScale.x / lossyScale.x * localScale.x,
                newScale.y / lossyScale.y * localScale.y,
                newScale.z / lossyScale.z * localScale.z
            );
            transform.localScale = relativeScale;
        }

        public static Vector3 GetPointInLine(this float position, Vector3 start, Vector3 end)
        {
            return start + (end - start) * position;
        }

        public static Vector3 GetPointInLineDistance(this float distance, Vector3 start, Vector3 end)
        {
            var position = distance / Vector3.Distance(start, end);
            return GetPointInLine(position, start, end);
        }
    }
}