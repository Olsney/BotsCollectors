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
        private const int MaxCollectorsToBuy = 2;
        private const int NewCastlePrice = 5;
        private const int MinCollectorsAmount = 1;

        [SerializeField] private CastleAreaTrigger _castleAreaTrigger;
        [SerializeField] private MineralsScanner _scanner;
        [SerializeField] private CollectorSpawner _collectorSpawner;

        private List<Collector> _collectors;
        private List<Mineral> _minerals;
        private MineralsData _mineralsData;
        private int _boughtCollectorsCount;

        private bool _isFarmingForNewCastle;
        private bool _flagPlaced;
        private Flag _flag;

        public FlagPlacer FlagPlacer { get; private set; }
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

        private void Update()
        {
            if (_flagPlaced) 
                TryBuildNewBase();
        }

        public void BecomeFlagPlacer(FlagPlacer flagPlacer)
        {
            FlagPlacer = flagPlacer;
            FlagPlacer.Placed += OnFlagPlaced;
        }

        public void LoseFlagPlacer()
        {
            FlagPlacer.Placed -= OnFlagPlaced;
            FlagPlacer = null;
        }

        private void TryBuildNewBase()
        {
            Collector collector = GetRandomFreeCollector();
            _isFarmingForNewCastle = true;

            if (IsEnoughFreeCollectorsToBuild(collector))
            {
                if (IsEnoughResourcesToBuild())
                    BuildNewBase(collector);
            }
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
            _flag = flag;
            _flagPlaced = true;
        }

        private void BuildNewBase(Collector collector)
        {
            Pay(NewCastlePrice);
            collector.BuildCastle(_flag);

            _isFarmingForNewCastle = false;
            _flagPlaced = false;
        }

        private bool IsEnoughResourcesToBuild() =>
            _minerals.Count >= NewCastlePrice;

        private bool IsEnoughFreeCollectorsToBuild(Collector collector) =>
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
                if (TryFindMinerals(out List<Mineral> minerals))
                    SetWorkToCollector(minerals);

                yield return null;
            }
        }

        private bool TryFindMinerals(out List<Mineral> minerals) => 
            _scanner.TryFindMinerals(out minerals);

        private bool CanBuyCollector() => 
            _minerals.Count >= CollectorPrice && _boughtCollectorsCount < MaxCollectorsToBuy &&_isFarmingForNewCastle == false;

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
            if (IsPriceIncorrect(price))
                return;

            _minerals.RemoveRange(0, price);
        }

        private void SpawnCollector() =>
            _collectorSpawner.Spawn();

        private void IncreaseBoughtCollectorsCount() =>
            _boughtCollectorsCount++;

        private bool IsPriceIncorrect(int price) => 
            price <= 0 || _minerals.Count < price;
    }
}