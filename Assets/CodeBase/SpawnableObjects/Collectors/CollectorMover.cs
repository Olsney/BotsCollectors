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

        // private void Update()
        // {
        //     // _navMesh.isStopped = false;
        //     // _navMesh.acceleration = 
        //     // _navMesh.speed = 10f;
        //     // _navMesh.SetDestination(_targetPosition);
        // }

        public void SetTargetPoint(Vector3 point)
        {
            Debug.Log($"Идем к {point}");
            //
            // _navMesh.isStopped = false;
            // _navMesh.acceleration = 
            //     _navMesh.speed = 10f;
            // _navMesh.SetDestination(_targetPosition);
            
            _targetPosition = point;
        }
    }
}