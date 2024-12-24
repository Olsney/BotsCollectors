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
            StopMove();
            
            _navMesh.SetDestination(_targetPosition);
        }

        private void Update()
        {
            // _navMesh.SetDestination(_targetPosition);
        }

        public void SetTargetPoint(Vector3 point)
        {
            _navMesh.isStopped = false;
            _navMesh.acceleration = 5f;
            _navMesh.speed = 5f;

            // _targetPosition = point;

            _navMesh.SetDestination(new Vector3(point.x, point.y, point.z));
        }

        public void StopMove()
        {
            _navMesh.acceleration = 0f;
            _navMesh.isStopped = true;
            _navMesh.speed = 0;
        }
    }
}