using System.Collections;
using CodeBase.Castles;
using CodeBase.Extensions;
using CodeBase.SpawnableObjects.Collectors;
using UnityEngine;

namespace CodeBase.Services
{
    public class CollectorSpawner : MonoBehaviour
    {
        [SerializeField] private Collector _prefab;
        [SerializeField] private UnitSpawnPointContainer _container;
        [SerializeField] private Transform _dropPlace;
        [SerializeField] private CastleFactory _castleFactory;
        
        public Vector3 DropPlace => _dropPlace.position;
        
        public void SpawnCollectors()
        {
            StartCoroutine(CollectorsSpawningJob());
        }

        public void Spawn()
        {
            Vector3 spawnPoint = DataExtension.GetRandomPosition(_container.SpawnPoints);
            
            Collector collector = Instantiate(_prefab);
            collector.Construct(_castleFactory);
            collector.Initialize(spawnPoint, _dropPlace.position);
        }

        private IEnumerator CollectorsSpawningJob()
        {
            int collectorsAmount = 3;
            int spawnedAmount = 0;
            float delay = 3;

            WaitForSeconds waitTime = new WaitForSeconds(delay);

            while (spawnedAmount < collectorsAmount)
            {
                Spawn();
                spawnedAmount++;

                yield return waitTime;
            }
        }
    }
}