using System.Collections.Generic;
using CodeBase.SpawnableObjects.Resources;
using UnityEngine;

namespace CodeBase.Services
{
    public class ResourceContainer : MonoBehaviour
    {
        private List<Vector3> _spawnPoints;
        
        public List<Vector3> SpawnPoints => new List<Vector3>(_spawnPoints);

        private void Awake()
        {
            _spawnPoints = new List<Vector3>();
            
            foreach (IResourceSpawnPosition resourceSpawnPosition in gameObject.GetComponentsInChildren<IResourceSpawnPosition>())
                _spawnPoints.Add(resourceSpawnPosition.Position);
        }
    }
}