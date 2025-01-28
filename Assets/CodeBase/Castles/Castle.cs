using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Extensions;
using CodeBase.Flags;
using CodeBase.Services;
using CodeBase.SpawnableObjects.Collectors;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Castles
{
    [RequireComponent(typeof(CollectorSpawner))]
    public class Castle : MonoBehaviour
    {
        private const int CollectorPrice = 3;
        private const int MaxCollectorsToBuy = 5;
        private const int NewCastlePrice = 5;
        private const int MinCollectorsAmount = 1;

        [SerializeField] private CastleAreaTrigger _castleAreaTrigger;
        [SerializeField] private MineralsScanner _scanner;
        [SerializeField] private CollectorSpawner _collectorSpawner;

        private List<Collector> _collectors;
        private List<Mineral> _minerals;
        private MineralsData _mineralsData;
        private FlagPlacer _flagPlacer;
        private int _boughtCollectorsCount;

        private bool _isFarmingForNewCastle;

        public FlagPlacer FlagPlacer => _flagPlacer;
        public Vector3 DropPlacePoint => _collectorSpawner.DropPlace;
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
            _castleAreaTrigger.CollectorEntered += OnCollectorEntered;
            _castleAreaTrigger.CollectorExited += OnCollectorExited;
            _castleAreaTrigger.ResourceEntered += OnResourceEntered;
        }

        private void OnDisable()
        {
            _castleAreaTrigger.CollectorEntered -= OnCollectorEntered;
            _castleAreaTrigger.CollectorExited -= OnCollectorExited;
            _castleAreaTrigger.ResourceEntered -= OnResourceEntered;
        }

        public void BecomeFlagPlacer(FlagPlacer flagPlacer)
        {
            _flagPlacer = flagPlacer;
            _flagPlacer.Placed += OnFlagPlaced;
        }

        public void LostFlagPlacer()
        {
            _flagPlacer.Placed -= OnFlagPlaced;
            _flagPlacer = null;
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

        private void OnFlagPlaced(Flag flag)
        {
            _isFarmingForNewCastle = true;
            
            Collector collector = GetRandomFreeCollector();

            if (!CanBuild(collector))
                return;

            if (_minerals.Count >= NewCastlePrice)
            {
                Pay(NewCastlePrice);
                collector.BuildCastle(flag);
            }
            
            _isFarmingForNewCastle = false;
        }

        private bool CanBuild(Collector collector) => 
            collector != null && _collectors.Count > MinCollectorsAmount;

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
                if (_isFarmingForNewCastle)
                    yield return null;
                
                if (_scanner.TryFindMinerals(out List<Mineral> minerals))
                    SetWorkToCollector(minerals);

                yield return null;
            }
        }

        private bool CanBuyCollector() =>
            _minerals.Count >= 3 && _boughtCollectorsCount < MaxCollectorsToBuy;

        private void BuyCollector()
        {
            Pay(CollectorPrice);
            SpawnCollector();
            IncreaseBoughtCollectorsCount();
        }

        private void InstantiateCollectors() =>
            _collectorSpawner.SpawnCollectors();

        private void Pay(int price)
        {
            if (price <= 0 || _minerals.Count < price)
            {
                Debug.LogError($"Wrong price - {price} is cannot be payed.");

                return;
            }
            
            _minerals.RemoveRange(0, price);
        }

        private void SpawnCollector() =>
            _collectorSpawner.Spawn();

        private void IncreaseBoughtCollectorsCount() =>
            _boughtCollectorsCount++;
    }
}