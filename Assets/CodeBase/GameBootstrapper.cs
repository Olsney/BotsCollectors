using CodeBase.CollectorsBases;
using CodeBase.Services;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;

namespace CodeBase
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private CollectorsBase _collectorsBase;
        [SerializeField] private MineralContainer _mineralContainer; 
        [SerializeField] private MineralSpawner _mineralSpawner;

        private void Awake()
        {
            InitGameWorld();
        }

        private void InitGameWorld()
        {
            _mineralContainer.Construct();
            _mineralSpawner.Construct(_mineralContainer);
            _collectorsBase.Construct();
        }
    }
}