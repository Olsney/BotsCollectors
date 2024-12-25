using System.Collections.Generic;
using System.Linq;
using CodeBase.SpawnableObjects.Minerals;
using UnityEngine;

namespace CodeBase.CollectorsBases
{
    public class MineralsData
    {
        private readonly HashSet<Mineral> _minerals = new();
    
        public void ReserveCrystal(Mineral mineral) => 
            _minerals.Add(mineral);
    
        public IEnumerable<Mineral> GetFreeMinerals(IEnumerable<Mineral> minerals) => 
            minerals.Where(cristal => _minerals.Contains(cristal) == false);

        public void RemoveReservation(Mineral crystal) => 
            _minerals.Remove(crystal);
    }
}