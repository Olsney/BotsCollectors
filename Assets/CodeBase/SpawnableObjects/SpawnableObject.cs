using CodeBase.CollectorsBases;
using UnityEngine;

namespace CodeBase.SpawnableObjects
{
    public class SpawnableObject : MonoBehaviour
    {
        public void Init(Vector3 position)
        {
            transform.position = position;
        }

        public void Init(Vector3 position, Vector3 dropPlace)
        {
            transform.position = position;
            
        }
    }
}