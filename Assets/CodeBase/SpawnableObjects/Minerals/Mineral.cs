using UnityEngine;

namespace CodeBase.SpawnableObjects.Minerals
{
    public class Mineral : SpawnableObject, IMineral
    {
        private bool _isTaken;
        private bool _isAvailable;
        public Vector3 Position => transform.position;
        public bool IsTaken => _isTaken;
        public bool IsAvailable => _isAvailable;

        private void OnEnable()
        {
            _isAvailable = true;
        }
        
        public void Bind(Transform transformToBind)
        {
            _isTaken = true;
            
            transform.parent = transformToBind;
        }

        public void BecomeUnavailable()
        {
            _isAvailable = false;
        }
    }
}