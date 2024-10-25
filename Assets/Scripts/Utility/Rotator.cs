using Sirenix.OdinInspector;
using UnityEngine;

namespace Utility
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private bool randomStartRotation;
        [SerializeField] private bool randomRotationSpeed;

        [SerializeField] [ShowIf("@randomRotationSpeed")]
        private Vector2 rotationSpeedRange;

        [SerializeField] private bool randomRotationDirection;

        [SerializeField] [Range(-360, 360)] public float rotationSpeed;

        private void Awake()
        {
            if (randomStartRotation) transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            if (randomRotationSpeed) rotationSpeed = Random.Range(rotationSpeedRange.x, rotationSpeedRange.y);
            if (randomRotationDirection) rotationSpeed *= Random.Range(0f, 1f) > 0.5f ? 1 : -1;
        }

        private void Update()
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}