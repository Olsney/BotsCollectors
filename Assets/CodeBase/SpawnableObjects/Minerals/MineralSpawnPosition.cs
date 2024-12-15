using UnityEngine;

namespace CodeBase.SpawnableObjects.Minerals
{
    public class MineralSpawnPosition : MonoBehaviour, IMineralSpawnPosition
    {
        public Vector3 Position => gameObject.transform.position;
    }
}