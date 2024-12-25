using System.Collections;
using CodeBase.Extensions;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;

namespace CodeBase.Services
{
    public class MineralSpawner : MonoBehaviour
    {
        [SerializeField] private Mineral _prefab;
        [SerializeField] private MineralContainer _mineralContainer;
        
        private void Start()
        {
            StartCoroutine(StartResourceSpawning());
        }
        
        public void Spawn(Vector3 position)
        {
            Instantiate(_prefab);
            _prefab.Init(position);
        }

        private IEnumerator StartResourceSpawning()
        {
            float delay = 5f;
            WaitForSeconds wait = new WaitForSeconds(delay);

            while (enabled)
            {
                Vector3 position = DataExtension.GetRandomPosition(_mineralContainer.SpawnPoints);
                
                Spawn(position);

                yield return wait;
            }
        }
    }
}