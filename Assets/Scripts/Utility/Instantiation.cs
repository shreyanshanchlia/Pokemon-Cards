using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    public class Instantiation : MonoBehaviour
    {
        [SerializeField] private GameObject defaultGameObject;
        [SerializeField] private Transform instantiationParent;
        public bool autoSpawn;

        [ShowIf("@autoSpawn")] [SerializeField]
        private float spawnAfter;

        public UnityEvent<GameObject> onNewObjectSpawn;

        private void Start()
        {
            if (autoSpawn) Invoke(nameof(InstantiateDefault), spawnAfter);
        }

        private void InstantiateGameObject(GameObject toInstantiate, Vector3 position, bool useParent = true)
        {
            if (useParent)
            {
                var spawned = Instantiate(toInstantiate, position, Quaternion.identity, instantiationParent);
                onNewObjectSpawn?.Invoke(spawned);
            }
            else
            {
                var spawned = Instantiate(toInstantiate, position, Quaternion.identity);
                onNewObjectSpawn?.Invoke(spawned);
            }
        }

        private void InstantiateGameObject(GameObject toInstantiate)
        {
            InstantiateGameObject(toInstantiate, transform.position);
        }

        public void InstantiateGameObjectWorldSpace(GameObject toInstantiate)
        {
            InstantiateGameObject(toInstantiate, transform.position, false);
        }

        public void InstantiateDefaultOnSameParent()
        {
            instantiationParent = transform.parent;
            InstantiateGameObject(defaultGameObject);
        }

        public void InstantiateDefaultGameObjectAtGameObjectPosition(GameObject toInstantiate)
        {
            InstantiateDefault(toInstantiate.transform.position);
        }

        public void InstantiateDefault()
        {
            InstantiateGameObject(defaultGameObject);
        }

        public void InstantiateDefault(Vector3 position)
        {
            InstantiateGameObject(defaultGameObject, position);
        }
    }
}