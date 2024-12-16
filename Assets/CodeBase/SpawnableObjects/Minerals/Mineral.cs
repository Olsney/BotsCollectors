using UnityEngine;

namespace CodeBase.SpawnableObjects.Minerals
{
    public class Mineral : SpawnableObject, IMineral
    {
        public Vector3 Position => transform.position;

        public void Bind(Transform transformToBind) => 
            transform.parent = transformToBind;
    }
}