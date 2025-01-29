using System.Collections;
using CodeBase.Castles;
using CodeBase.Extensions;
using CodeBase.Flags;
using CodeBase.Services;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;

namespace CodeBase.SpawnableObjects.Collectors
{
    [RequireComponent(typeof(CollectorMover))]
    public class Collector : MonoBehaviour
    {
        [SerializeField] private float _permissibleResourceDistanceDifference = 1;
        [SerializeField] private float _takeRadius;
        [SerializeField] private Vector3 _dropPlace;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private CastleFactory _castleFactory;
        
        
        private CollectorMover _collectorMover;

        public bool IsWorking { get; private set; }

        private void Awake()
        {
            _collectorMover = GetComponent<CollectorMover>();
            StopMove();
        }


        public void Initialize(Vector3 position, Vector3 dropPlace)
        {
            transform.position = position;
            _dropPlace = new Vector3(dropPlace.x, dropPlace.y, dropPlace.z);
        }

        public void Work(Vector3 destionation)
        {
            IsWorking = true;

            MoveTo(destionation);

            StartCoroutine(InteractWithMineral(destionation));
        }

        public void BuildCastle(Flag flag) => 
            StartCoroutine(BuildCastleJob(flag));

        private IEnumerator InteractWithMineral(Vector3 destionation)
        {
            while (enabled)
            {
                if (IsCloseEnough(destionation))
                {
                    if (TryFindMineral(out Mineral mineral))
                    {
                        TakeMineral(mineral);
                        GoBase();

                        yield break;
                    }
                    
                    GoBase();
                }

                yield return null;
            }
        }

        public void FinishWork()
        {
            StopMove();
            IsWorking = false;
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
            mineral.Bind(transform);

        private IEnumerator BuildCastleJob(Flag flag)
        {
            float delay = 0.1f;
            WaitForSeconds wait = new(delay);

            Vector3 flagPosition = flag.transform.position;

            while (CanBuild(flagPosition) == false)
            {
                _collectorMover.SetTargetPoint(flagPosition);

                if (Vector3.Distance(transform.position, flagPosition) <= 3f)
                {
                    Castle castle = _castleFactory.Create(flagPosition);
                    Initialize(castle.transform.position, castle.DropPlacePoint);
                }

                yield return wait;
            }

            IsWorking = false;

            flag.Destroy();
        }

        private bool CanBuild(Vector3 position) =>
            transform.position.SqrDistance(position) < 3f;

        private void StopMove() => 
            _collectorMover.StopMove();

        private void MoveTo(Vector3 destionation) => 
            _collectorMover.SetTargetPoint(destionation);

        private bool IsCloseEnough(Vector3 destionation) => 
            DataExtension.SqrDistance(transform.position, destionation) <= _permissibleResourceDistanceDifference;

        private void GoBase() =>
            MoveTo(_dropPlace);
    }
}