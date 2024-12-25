using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Extensions;
using CodeBase.Services;
using CodeBase.SpawnableObjects.Collectors;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.CollectorsBases
{
    [RequireComponent(typeof(CollectorSpawner))]
    public class CollectorsBase : MonoBehaviour
    {
        [SerializeField] private BaseAreaTrigger _baseAreaTrigger;
        [SerializeField] private MineralsScanner _scanner;
        [SerializeField] private CollectorSpawner _collectorSpawner;

        private List<Collector> _collectors;
        private List<Mineral> _minerals;
        private MineralsData _mineralsData;

        public event Action<int> ResourceCollected;

        public void Construct()
        {
            _collectors = new List<Collector>();
            _minerals = new List<Mineral>();
            _mineralsData = new MineralsData();
        }

        private void Start()
        {
            _collectorSpawner.SpawnCollectors();
            StartCoroutine(FindMineralsJob());
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
            _baseAreaTrigger.ResourceEntered -= OnResourceEntered;
        }

        private void OnCollectorEntered(Collector collector)
        {
            _collectors.Add(collector);
            collector.FinishWork();
        }

        private void OnCollectorExited(Collector collector) =>
            _collectors.Remove(collector);

        private void OnResourceEntered(Mineral mineral)
        {
            _minerals.Add(mineral);
            mineral.transform.parent = transform;
            mineral.gameObject.SetActive(false);
            _mineralsData.RemoveReservation(mineral);

            ResourceCollected?.Invoke(_minerals.Count);
        }

        private void SetWorkToCollector(List<Mineral> minerals)
        {
            if (minerals == null)
                return;

            IEnumerable<Mineral> minerals2 = _mineralsData.GetFreeMinerals(minerals).OrderBy(mineral =>
                DataExtension.SqrDistance(transform.position, mineral.transform.position));
            
            if (minerals2.Any() == false)
                return;


            foreach (var mineral in minerals2)
            {
                Collector collector = GetRandomFreeCollector();

                if (collector == null)
                    return;
                
                _mineralsData.ReserveCrystal(mineral);
                collector.Work(mineral.Position);
            }
        }

        private List<Collector> FindFreeCollectors()
        {
            List<Collector> freeCollectors = new List<Collector>();

            foreach (Collector collector in _collectors)
            {
                if (collector.IsWorking == false)
                    freeCollectors.Add(collector);
            }

            return freeCollectors;
        }

        private Collector GetRandomFreeCollector()
        {
            List<Collector> freeCollectors = FindFreeCollectors();


            if (freeCollectors.Count == 0)
            {
                return default;
            }

            var randomCollector = freeCollectors[Random.Range(0, freeCollectors.Count)];

            return randomCollector;
        }

        private IEnumerator FindMineralsJob()
        {
            while (enabled)
            {
                if (_scanner.TryFindMinerals(out List<Mineral> minerals))
                {
                    SetWorkToCollector(minerals);
                }

                yield return null;
            }
        }
    }
}