using System;
using CodeBase.SpawnableObjects.Collectors;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;

namespace CodeBase.CollectorsBases
{
    public class BaseAreaTrigger : MonoBehaviour
    {
        public event Action<Collector> CollectorEntered;
        public event Action<Collector> CollectorExited;
        public event Action<Mineral> ResourceEntered;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Collector collector))
            {
                CollectorEntered?.Invoke(collector);
                Debug.Log("Collector entered invoked");
            }

            if (other.TryGetComponent(out Mineral mineral))
            {
                ResourceEntered?.Invoke(mineral);
                Debug.Log("Resource entered invoked");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Collector collector))
            {
                CollectorExited?.Invoke(collector);
                Debug.Log("Collector exited entered invoked");
            }
        }
    }
}