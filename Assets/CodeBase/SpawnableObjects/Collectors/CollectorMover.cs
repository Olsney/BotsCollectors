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
        }

        public void SetTargetPoint(Vector3 point)
        {
            _navMesh.isStopped = false;
            _navMesh.acceleration = 5f;
            _navMesh.speed = 5f;
            
            _navMesh.SetDestination(new Vector3(point.x, 0, point.z));
        }

        public void StopMove()
        {
            _navMesh.isStopped = true;
        }
    }
}