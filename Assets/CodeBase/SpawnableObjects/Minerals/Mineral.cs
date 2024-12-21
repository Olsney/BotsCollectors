using UnityEngine;

namespace CodeBase.SpawnableObjects.Minerals
{
    public class Mineral : SpawnableObject, IMineral
    {
        private bool _isTaken;
        public Vector3 Position => transform.position;
        public bool IsTaken => _isTaken;

        public void Bind(Transform transformToBind)
        {
            _isTaken = true;
            
            transform.parent = transformToBind;
        } 
    }
}