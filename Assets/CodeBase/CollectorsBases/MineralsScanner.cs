using System.Collections.Generic;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;

namespace CodeBase.CollectorsBases
{
    public class MineralsScanner : MonoBehaviour
    {
        [SerializeField] private float _scanRadius;
        
        public bool TryFindMinerals(out List<Mineral> minerals)
        {
            minerals = new();

            Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.TryGetComponent(out Mineral mineral))
                {
                    if (mineral != null)
                        minerals.Add(mineral);
                }
            }

            if (minerals.Count < 0)
                return false;

            return true;
        }
    }
}