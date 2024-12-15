using CodeBase.SpawnableObjects;
using UnityEngine;

namespace CodeBase.Services
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] protected SpawnableObject _prefab;
        
        protected void Spawn(Vector3 position)
        {
            Instantiate(_prefab);
            _prefab.Init(position);
        }
    }
}