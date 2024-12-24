using CodeBase.CollectorsBases;
using CodeBase.SpawnableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Services
{
    public class Spawner<T> : MonoBehaviour where T : SpawnableObject
    {
        [SerializeField] protected T Prefab;
        
        // public void Spawn(Vector3 position)
        // {
        //     Instantiate(Prefab);
        //     Prefab.Init(position);
        // }

        // public void Spawn(Vector3 position, Vector3 dropPlace)
        // {
        //     Instantiate(Prefab);
        //     Prefab.Init(position, dropPlace);
        // }
    }
}