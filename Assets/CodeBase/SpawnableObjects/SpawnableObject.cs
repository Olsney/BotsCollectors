using UnityEngine;

namespace CodeBase.SpawnableObjects
{
    public abstract class SpawnableObject : MonoBehaviour
    {
        public void Init(Vector3 position)
        {
            transform.position = position;
        }

        public virtual void Init(Vector3 position, Vector3 dropPlace)
        {
            transform.position = position;
        }
    }
}