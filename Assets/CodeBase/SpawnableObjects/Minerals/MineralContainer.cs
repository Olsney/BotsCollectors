using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.SpawnableObjects.Minerals
{
    public class MineralContainer : MonoBehaviour
    {
        private List<Vector3> _spawnPoints;
        
        public List<Vector3> SpawnPoints => new List<Vector3>(_spawnPoints);

        public void Construct()
        {
            _spawnPoints = new List<Vector3>();
            
            foreach (IMineralSpawnPosition resourceSpawnPosition in gameObject.GetComponentsInChildren<IMineralSpawnPosition>())
                _spawnPoints.Add(resourceSpawnPosition.Position);
        }
    }
}