using System.Collections;
using System.Collections.Generic;
using CodeBase.Extensions;
using CodeBase.Services;
using CodeBase.SpawnableObjects.Collectors;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.CollectorsBases
{
    public interface IBaseDropPlace
    {
        public Vector3 DropPlacePoint { get; }
    }

    [RequireComponent(typeof(CollectorFactory))]
    public class CollectorsBase : MonoBehaviour
    {
        [SerializeField] private UnitSpawnPointContainer _container;
        [SerializeField] private float _scanRadius;
        [SerializeField] private BaseAreaTrigger _baseAreaTrigger;
        [SerializeField] private Transform _dropPlace;

        private CollectorFactory _collectorFactory;
        private List<Vector3> _spawnPoints;
        private List<Collector> _collectors;
        private List<Mineral> _minerals;

        private Coroutine _collectorsSpawningCoroutine;
        private Coroutine _findMineralsCoroutine;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, _scanRadius);
        }

        private void Awake()
        {
            _collectors = new List<Collector>();
            _minerals = new List<Mineral>();
        }

        private void OnEnable()
        {
            _baseAreaTrigger.CollectorEntered += OnCollectorEntered;
            _baseAreaTrigger.CollectorExited += OnCollectorExited;
            _baseAreaTrigger.ResourceEntered += OnResourceEntered;
        }

        private void OnDisable()
        {
            _baseAreaTrigger.CollectorEntered -= OnCollectorEntered;
            _baseAreaTrigger.CollectorExited -= OnCollectorExited;
            _baseAreaTrigger.ResourceEntered += OnResourceEntered;
        }

        private void Start()
        {
            _spawnPoints = _container.SpawnPoints;

            _collectorFactory = GetComponent<CollectorFactory>();

            _collectorsSpawningCoroutine = StartCoroutine(CollectorsSpawningJob());
            _findMineralsCoroutine = StartCoroutine(FindMineralsJob());
        }

        private void TryFindMinerals()
        {
            List<Mineral> minerals = new();

            Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.TryGetComponent(out Mineral mineral))
                {
                    Debug.Log("Нашли минералы");

                    if (mineral != null)
                        minerals.Add(mineral);
                }
            }

            Debug.Log($"Количество материалов на сцене: {minerals.Count}");

            if (minerals.Count < 0)
                return;

            SetWorkToCollector(minerals);
        }

        private void SetWorkToCollector(List<Mineral> minerals)
        {
            if (minerals == null)
                return;

            if (_collectors.Count == 0)
                return;


            foreach (Mineral mineral in minerals)
            {
                Debug.Log("Отправили работягу работать");

                Collector collector = GetRandomFreeCollector();

                if (collector == null)
                    return;

                Debug.Log($"{mineral.Position} - позиция позиция");

                collector.Work(mineral.Position);
            }
        }

        private Collector GetRandomFreeCollector()
        {
            var freeCollectors = new List<Collector>();

            foreach (var collector in _collectors)
            {
                if (collector.IsWorking == false)
                {
                    freeCollectors.Add(collector);
                }
            }

            if (freeCollectors.Count == 0)
                return default;

            return freeCollectors[Random.Range(0, _collectors.Count)];
        }

        private IEnumerator FindMineralsJob()
        {
            float delay = 10f;
            WaitForSeconds waitTime = new WaitForSeconds(delay);

            while (enabled)
            {
                TryFindMinerals();

                yield return delay;
            }
        }

        private IEnumerator CollectorsSpawningJob()
        {
            int collectorsAmount = 3;
            int spawnedAmount = 0;
            float delay = 3;

            WaitForSeconds waitTime = new WaitForSeconds(delay);

            while (spawnedAmount < collectorsAmount)
            {
                _collectorFactory.Spawn(DataExtension.GetRandomPosition(_spawnPoints), _dropPlace.position);
                spawnedAmount++;

                yield return waitTime;
            }
        }

        private void OnCollectorEntered(Collector collector) =>
            _collectors.Add(collector);

        private void OnCollectorExited(Collector collector) =>
            _collectors.Remove(collector);

        private void OnResourceEntered(Mineral mineral) =>
            _minerals.Add(mineral);
    }
}