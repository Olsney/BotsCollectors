using CodeBase.Castles;
using CodeBase.Services;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private Castle _firstCastle;
        [SerializeField] private MineralContainer _mineralContainer; 
        [SerializeField] private MineralSpawner _mineralSpawner;
        [SerializeField] private Castle _castlePrefab;
        [SerializeField] private CastleFactory _castleFactory;

        private void Awake() => 
            InitGameWorld();

        private void InitGameWorld()
        {
            _mineralContainer.Construct();
            _mineralSpawner.Construct(_mineralContainer);
            _firstCastle.Construct();
            _castleFactory.Construct(_castlePrefab);
        }
    }
}