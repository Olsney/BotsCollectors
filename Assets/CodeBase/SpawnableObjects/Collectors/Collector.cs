using System.Collections;
using CodeBase.CollectorsBases;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;

namespace CodeBase.SpawnableObjects.Collectors
{
    [RequireComponent(typeof(CollectorMover))]
    public class Collector : SpawnableObject, ICollector
    {
        [SerializeField] private float _permissibleDifference;
        [SerializeField] private float _takeRadius;

        private CollectorMover _collectorMover;
        [SerializeField] private Vector3 _dropPlace;

        public bool IsWorking { get; private set; }
        public Transform Transform => transform;

        private void Awake()
        {
            _collectorMover = GetComponent<CollectorMover>();
            _collectorMover.StopMove();
        }

        public void Work(Vector3 destionation)
        {
            IsWorking = true;

            Debug.Log($"{destionation} - точка перед вызовом метода SetTargetPoint из Work");

            _collectorMover.SetTargetPoint(destionation);

            StartCoroutine(CalculateDistanceToMineral(destionation));
            
        }

        public override void Init(Vector3 position, Vector3 dropPlace)
        {
            base.Init(position, dropPlace);
            _dropPlace = new Vector3(dropPlace.x, dropPlace.y, dropPlace.z);

            Debug.Log($"Я сборщик и мой dropPlace - {_dropPlace}");
        }

        private IEnumerator CalculateDistanceToMineral(Vector3 destionation)
        {
            float delay = 1f;
            WaitForSeconds wait = new(delay);

            // if ((transform.position - destionation).sqrMagnitude <= _permissibleDifference)
            while (Vector3.Distance(transform.position, destionation) > _permissibleDifference)
            {
                if (TryFindMineral(out Mineral mineral))
                {
                    TakeMineral(mineral);
                    GoBase();
                }

                yield return wait;
            }
            
            FinishWork();
        }

        private void FinishWork()
        {
            _collectorMover.StopMove();
            IsWorking = false;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent(out CollectorsBase dummy))
                IsWorking = false;
        }

        public void GoBase()
        {
            Debug.Log($"Я иду на базу, мой dropPlace - {_dropPlace}");
            
            _collectorMover.SetTargetPoint(_dropPlace);
        }

        private bool TryFindMineral(out Mineral mineral)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _takeRadius);
            mineral = default;

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out mineral))
                {
                    Debug.Log("Сборщик нашел минерал и стоит к нему близко");

                    return true;
                }
            }

            return false;
        }

        private void TakeMineral(Mineral mineral) =>
            mineral.Bind(Transform);
    }
}