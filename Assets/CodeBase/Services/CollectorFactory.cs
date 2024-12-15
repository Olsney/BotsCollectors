using CodeBase.SpawnableObjects.Collectors;
using UnityEngine;

namespace CodeBase.Services
{
    public class CollectorFactory : MonoBehaviour
    {
        [SerializeField] private Collector _collectorPrefab;

        public void Create(Vector3 spawnPoint)
        {
            Instantiate(_collectorPrefab);
            
            _collectorPrefab.Init(spawnPoint);
        }
    }
}