using System.Collections;
using System.Collections.Generic;
using CodeBase.Extensions;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;

namespace CodeBase.Services
{
    public class MineralSpawner : Spawner<Mineral>
    {
        [SerializeField] private MineralContainer _mineralContainer;

        private List<Vector3> _spawnPoints = new List<Vector3>();

        private void Start()
        {
            _spawnPoints = _mineralContainer.SpawnPoints;

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
    }
}