using System.Collections;
using CodeBase.CollectorsBases;
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

        public void SpawnCollectors()
        {
            StartCoroutine(CollectorsSpawningJob());
        }

        private void Spawn(Vector3 position, Vector3 dropPlace)
        {
            Collector collector = Instantiate(_prefab);
            collector.Init(position, dropPlace);
        }
        
        private IEnumerator CollectorsSpawningJob()
        {
            int collectorsAmount = 3;
            int spawnedAmount = 0;
            float delay = 3;

            WaitForSeconds waitTime = new WaitForSeconds(delay);

            while (spawnedAmount < collectorsAmount)
            {
                Spawn(DataExtension.GetRandomPosition(_container.SpawnPoints), _dropPlace.position);
                spawnedAmount++;

                yield return waitTime;
            }
        }
    }
}