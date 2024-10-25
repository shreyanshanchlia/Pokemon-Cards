using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable Unity.InefficientPropertyAccess

namespace Utility
{
    public class LookAt : MonoBehaviour
    {
        public Transform lookAt;
        public bool alwaysLook;

        private void Start()
        {
            InvokeLookAt();
        }

        private void LateUpdate()
        {
            if (alwaysLook) InvokeLookAt();
        }

        [Button]
        private void InvokeLookAt()
        {
            if (lookAt) transform.up = transform.position - lookAt.position;
        }
    }
}