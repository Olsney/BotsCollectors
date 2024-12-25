using System.Collections;
using CodeBase.Extensions;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;

namespace CodeBase.Services
{
    public class MineralSpawner : MonoBehaviour
    {
        [SerializeField] private Mineral _prefab;
        
        private MineralContainer _mineralContainer;

        public void Construct(MineralContainer mineralContainer)
        {
            _mineralContainer = mineralContainer;
            StartCoroutine(StartResourceSpawning());
        }
        
        // private void Start()
        // {
        //     StartCoroutine(StartResourceSpawning());
        // }

        private void Spawn(Vector3 position)
        {
            Mineral mineral = Instantiate(_prefab);
            mineral.Init(position);
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