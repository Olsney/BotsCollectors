using System;
using CodeBase.SpawnableObjects.Collectors;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;

namespace CodeBase.Castles
{
    public class CastleAreaTrigger : MonoBehaviour
    {
        public event Action<Collector> CollectorEntered;
        public event Action<Collector> CollectorExited;
        public event Action<Mineral> ResourceEntered;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Collector collector)) 
                CollectorEntered?.Invoke(collector);

            if (other.TryGetComponent(out Mineral mineral)) 
                ResourceEntered?.Invoke(mineral);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Collector collector)) 
                CollectorExited?.Invoke(collector);
        }
    }
}