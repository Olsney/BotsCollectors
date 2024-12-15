using CodeBase.SpawnableObjects;
using UnityEngine;

namespace CodeBase.Services
{
    public class Spawner<T> : MonoBehaviour where T : SpawnableObject
    {
        [SerializeField] protected T _prefab;
        
        public void Spawn(Vector3 position)
        {
            Instantiate(_prefab);
            _prefab.Init(position);
        }
    }
}