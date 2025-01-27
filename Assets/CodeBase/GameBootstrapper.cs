using CodeBase.Castles;
using CodeBase.Services;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase
{
    public class GameBootstrapper : MonoBehaviour
    {
        [FormerlySerializedAs("castle")] [FormerlySerializedAs("_collectorsBase")] [SerializeField] private Castle _castle;
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
            _castle.Construct();
        }
    }
}