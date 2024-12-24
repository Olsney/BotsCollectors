using System.Collections;
using System.Reflection.Emit;
using CodeBase.CollectorsBases;
using CodeBase.SpawnableObjects.Minerals;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.SpawnableObjects.Collectors
{
    [RequireComponent(typeof(CollectorMover))]
    public class Collector : SpawnableObject, ICollector
    {
        [SerializeField] private float _permissibleDifference;
        [SerializeField] private float _takeRadius;
        [SerializeField] private Vector3 _dropPlace;
        [SerializeField] private LayerMask _layerMask;

        private CollectorMover _collectorMover;

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
            float delay = 0.1f;
            WaitForSeconds wait = new(delay);

            Debug.Log($"{Vector3.Distance(transform.position, destionation)} - distance between collector and mineral");

            // if ((transform.position - destionation).sqrMagnitude <= _permissibleDifference)
            while (enabled)
            {
                if (Vector3.Distance(transform.position, destionation) <= _permissibleDifference)
                {
                    if (TryFindMineral(out Mineral mineral))
                    {
                        TakeMineral(mineral);
                        GoBase();
                        
                        yield break;
                    }
                }
                
                yield return null;
            }

            // FinishWork();
        }

        public void FinishWork()
        {
            _collectorMover.StopMove();
            IsWorking = false;
        }

        // private void OnTriggerEnter(Collider collider)
        // {
        //     if (collider.TryGetComponent(out CollectorsBase dummy))
        //         FinishWork();
        // }

        public void GoBase()
        {
            Debug.Log($"Я иду на базу, мой dropPlace - {_dropPlace}");

            _collectorMover.SetTargetPoint(_dropPlace);
            Debug.Log("I'm going BASE! ");

            // FinishWork();
        }

        private bool TryFindMineral(out Mineral mineral)
        {
            Collider[] colliders = new Collider[1];

            var collidersCount = Physics.OverlapSphereNonAlloc(transform.position, _takeRadius, colliders, _layerMask);
            mineral = default;


            if (colliders[0].TryGetComponent(out mineral))
            {
                return true;
            }

            return false;
        }

        private void TakeMineral(Mineral mineral) =>
            mineral.Bind(Transform);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _takeRadius);
        }
    }
}