using UnityEngine;

namespace CodeBase.SpawnableObjects.Minerals
{
    public class Mineral : SpawnableObject, IMineral
    {
        private bool _isAvailable;
        public Vector3 Position => transform.position;
        public bool IsAvailable => _isAvailable;

        private void OnEnable()
        {
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