using Sirenix.OdinInspector;
using UnityEngine;

namespace Utility
{
    /// <summary>
    ///     Handles lifetime of gameObject.
    /// </summary>
    public class DestroyHandler : MonoBehaviour
    {
        public bool doNotDestroyOnLoad;
        public bool autoDestroy;

        /// <summary>
        ///     lifetime of the particle after which gameObject will be destroyed, if autoDestroy is enabled.
        /// </summary>
        [ShowIf("autoDestroy")] public float lifeTime = 40f;

        private void Start()
        {
            if (doNotDestroyOnLoad)
            {
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }

            if (autoDestroy) Destroy(gameObject, lifeTime);
        }

        public void DestroyGameObject(float delayToDestroy)
        {
            Destroy(gameObject, delayToDestroy);
        }
    }
}