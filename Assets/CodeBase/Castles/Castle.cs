using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Extensions;
using CodeBase.Services;
using CodeBase.SpawnableObjects.Collectors;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CodeBase.Castles
{
    [RequireComponent(typeof(CollectorSpawner))]
    public class Castle : MonoBehaviour
    {
        private const int CollectorPrice = 3;
        private const int MaxCollectorsToBuy = 5;
        
        [FormerlySerializedAs("_baseAreaTrigger")] [SerializeField] private CastleAreaTrigger castleAreaTrigger;
        [SerializeField] private MineralsScanner _scanner;
        [SerializeField] private CollectorSpawner _collectorSpawner;

        private List<Collector> _collectors;
        private List<Mineral> _minerals;
        private MineralsData _mineralsData;
        private int _boughtCollectorsCount;

        public event Action<int> ResourceCollected;

        public void Construct()
        {
            _collectors = new List<Collector>();
            _minerals = new List<Mineral>();
            _mineralsData = new MineralsData();
        }

        private void Start()
        {
            InstantiateCollectors();
            StartCoroutine(FindMineralsJob());
        }

        private void OnEnable()
        {
            castleAreaTrigger.CollectorEntered += OnCollectorEntered;
            castleAreaTrigger.CollectorExited += OnCollectorExited;
            castleAreaTrigger.ResourceEntered += OnResourceEntered;
        }

        private void OnDisable()
        {
            castleAreaTrigger.CollectorEntered -= OnCollectorEntered;
            castleAreaTrigger.CollectorExited -= OnCollectorExited;
            castleAreaTrigger.ResourceEntered -= OnResourceEntered;
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

            if (CanBuyCollector()) 
                BuyCollector();

            ResourceCollected?.Invoke(_minerals.Count);
        }

        private void SetWorkToCollector(List<Mineral> minerals)
        {
            if (minerals == null)
                return;

            IEnumerable<Mineral> freeMinerals = _mineralsData.GetFreeMinerals(minerals).OrderBy(mineral =>
                DataExtension.SqrDistance(transform.position, mineral.transform.position));
            
            if (freeMinerals.Any() == false)
                return;


            foreach (var mineral in freeMinerals)
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
                return default;

            var randomCollector = freeCollectors[Random.Range(0, freeCollectors.Count)];

            return randomCollector;
        }

        private IEnumerator FindMineralsJob()
        {
            while (enabled)
            {
                if (_scanner.TryFindMinerals(out List<Mineral> minerals)) 
                    SetWorkToCollector(minerals);

                yield return null;
            }
        }

        private bool CanBuyCollector() => 
            _minerals.Count >= 3 && _collectors.Count < MaxCollectorsToBuy;

        private void BuyCollector()
        {
            Pay(CollectorPrice);
            SpawnCollector();
            IncreaseBoughtCollectorsCount();
        }

        private void InstantiateCollectors() => 
            _collectorSpawner.SpawnCollectors();

        private void Pay(int price) => 
            _minerals.RemoveRange(0, price);

        private void SpawnCollector() => 
            _collectorSpawner.Spawn();

        private void IncreaseBoughtCollectorsCount() => 
            _boughtCollectorsCount++;
    }
}