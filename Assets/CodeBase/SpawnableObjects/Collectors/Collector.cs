using System.Collections;
using CodeBase.SpawnableObjects.Minerals;
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
        }

        private IEnumerator CalculateDistanceToMineral(Vector3 destionation)
        {
            float delay = 0.1f;
            WaitForSeconds wait = new(delay);
            
            while (enabled)
            {
                if (Vector3.Distance(transform.position, destionation) <= _permissibleDifference)
                {
                    Debug.Log(TryFindMineral(out Mineral mineralTest));
                    
                    if (TryFindMineral(out Mineral mineral))
                    {
                        TakeMineral(mineral);
                        GoBase();

                        yield break;
                    }
                }

                yield return null;
            }
        }

        public void FinishWork()
        {
            _collectorMover.StopMove();
            IsWorking = false;
        }
        
        public void GoBase()
        {
            _collectorMover.SetTargetPoint(_dropPlace);
        }

        private bool TryFindMineral(out Mineral mineral)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _takeRadius);
            mineral = default;

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out mineral))
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