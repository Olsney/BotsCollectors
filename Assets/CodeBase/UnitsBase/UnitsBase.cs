using System.Collections;
using System.Collections.Generic;
using CodeBase.Extensions;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UnitsBase
{
    [RequireComponent(typeof(CollectorFactory))]
    public class UnitsBase : MonoBehaviour
    {
        [SerializeField] private UnitSpawnPointContainer _container;
        
        private CollectorFactory _collectorFactory;
        private List<Vector3> _spawnPoints;

        private void Awake()
        {
            
        }

        private void Start()
        {
            _spawnPoints = _container.SpawnPoints;

            _collectorFactory = GetComponent<CollectorFactory>();
            
            StartCoroutine(StartCollectorsSpawning());
        }

        private IEnumerator StartCollectorsSpawning()
        {
            int collectorsAmount = 3;
            int spawnedAmount = 0;
            float delay = 3;
            
            WaitForSeconds waitTime = new WaitForSeconds(delay);

            while (spawnedAmount < collectorsAmount)
            {
                _collectorFactory.Spawn(DataExtension.GetRandomPosition(_spawnPoints));
                spawnedAmount++;

                yield return waitTime;
            }
        }
    }
}