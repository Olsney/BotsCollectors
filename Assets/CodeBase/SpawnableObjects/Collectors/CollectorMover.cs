using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.SpawnableObjects.Collectors
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CollectorMover : MonoBehaviour
    {
        private Vector3 _targetPosition;
        private NavMeshAgent _navMesh;

        private void Awake()
        {
            _navMesh = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            _navMesh.SetDestination(_targetPosition);
        }

        public void SetDirection(Vector3 point)
        {
            _targetPosition = point;
        }
    }
}