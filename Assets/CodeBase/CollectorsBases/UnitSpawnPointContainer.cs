using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.CollectorsBases
{
    public class UnitSpawnPointContainer : MonoBehaviour
    {
        private Transform _transform;
        private List<Vector3> _spawnPoints;

        public List<Vector3> SpawnPoints => new List<Vector3>(_spawnPoints);

        private void Awake()
        {
            _spawnPoints = new List<Vector3>();
            
            foreach (Transform position in GetComponentsInChildren<Transform>())
            {
                _spawnPoints.Add(position.position);
            }
        }
    }
}