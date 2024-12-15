using System.Collections;
using System.Collections.Generic;
using CodeBase.Extensions;
using CodeBase.SpawnableObjects.Resources;
using UnityEngine;

namespace CodeBase.Services
{
    public class ResourceSpawner : Spawner
    {
        [SerializeField] private ResourceContainer _resourceContainer;
        [SerializeField] private Resource _resourcePrefab;

        private List<Vector3> _spawnPoints = new List<Vector3>();

        private void Start()
        {
            _spawnPoints = _resourceContainer.SpawnPoints;

            StartCoroutine(StartResourceSpawning());
        }

        private IEnumerator StartResourceSpawning()
        {
            float delay = 5f;
            WaitForSeconds wait = new WaitForSeconds(delay);

            while (enabled)
            {
                Vector3 position = DataExtension.GetRandomPosition(_spawnPoints);
                
                Spawn(position);

                yield return wait;
            }
        }

        private void Spawn(Vector3 position)
        {
            Instantiate(_resourcePrefab);
            _resourcePrefab.Init(position);
        }
    }
}