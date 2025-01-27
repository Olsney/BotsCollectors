using System.Collections;
using CodeBase.Castles;
using CodeBase.Extensions;
using CodeBase.Flags;
using CodeBase.Services;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.SpawnableObjects.Collectors
{
    [RequireComponent(typeof(CollectorMover))]
    public class Collector : MonoBehaviour
    {
        [FormerlySerializedAs("_permissibleDifference")] [SerializeField] private float _permissibleResourceDistanceDifference;

        [SerializeField] private float _takeRadius;
        [SerializeField] private Vector3 _dropPlace;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private CastleFactory _castleFactory;


        private CollectorMover _collectorMover;

        public bool IsWorking { get; private set; }
        public Transform Transform => transform;

        private void Awake()
        {
            _collectorMover = GetComponent<CollectorMover>();
            _collectorMover.StopMove();
        }

        public void Initialize(Vector3 position, Vector3 dropPlace)
        {
            transform.position = position;
            _dropPlace = new Vector3(dropPlace.x, dropPlace.y, dropPlace.z);
        }

        public void Work(Vector3 destionation)
        {
            IsWorking = true;

            _collectorMover.SetTargetPoint(destionation);

            StartCoroutine(InteractWithMineral(destionation));
        }

        public void BuildCastle(Flag flag)
        {
            StartCoroutine(BuildCastleJob(flag));
        }

        private IEnumerator InteractWithMineral(Vector3 destionation)
        {
            float delay = 0.1f;
            WaitForSeconds wait = new(delay);

            while (enabled)
            {
                if (DataExtension.SqrDistance(transform.position, destionation) <=
                    _permissibleResourceDistanceDifference)
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
        }

        public void FinishWork()
        {
            _collectorMover.StopMove();
            IsWorking = false;
        }

        private void GoBase() =>
            _collectorMover.SetTargetPoint(_dropPlace);

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

        private IEnumerator BuildCastleJob(Flag flag)
        {
            float delay = 0.1f;
            WaitForSeconds wait = new(delay);

            Vector3 flagPosition = flag.transform.position;

            while (CanBuild(flagPosition) == false)
            {
                _collectorMover.SetTargetPoint(flagPosition);

                if (CanBuild(flagPosition))
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
            DataExtension.SqrDistance(transform.position, position) < 3f;
    }
}