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
            _navMesh.speed = 0;

        }

        private void Update()
        {
            _navMesh.isStopped = false;
            _navMesh.SetDestination(_targetPosition);
        }

        public void SetTargetPoint(Vector3 point)
        {
            Debug.Log($"Идем к {point}");
            //
            // _navMesh.isStopped = false;
            // _navMesh.acceleration = 
            //     _navMesh.speed = 10f;
            // _navMesh.SetDestination(_targetPosition);

            _navMesh.speed = 5f;

            _targetPosition = point;
        }

        public void StopMove()
        {
                _navMesh.speed = 0;
        }
    }
}