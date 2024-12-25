using UnityEngine;

namespace CodeBase.SpawnableObjects.Minerals
{
    public class Mineral : MonoBehaviour, IMineral
    {
        private bool _isAvailable;
        public Vector3 Position => transform.position;
        public bool IsAvailable => _isAvailable;

        public void Init(Vector3 position)
        {
            transform.position = position;
            _isAvailable = true;
        }
        
        public void Bind(Transform transformToBind)
        {
            transform.parent = transformToBind;
        }

        public void BecomeUnavailable()
        {
            _isAvailable = false;
        }
    }
}