using UnityEngine;

namespace CodeBase.SpawnableObjects.Resources
{
    public class ResourceSpawnPosition : MonoBehaviour, IResourceSpawnPosition
    {
        public Vector3 Position => gameObject.transform.position;
    }
}