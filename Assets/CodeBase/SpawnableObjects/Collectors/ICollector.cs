using UnityEngine;

namespace CodeBase.SpawnableObjects.Collectors
{
    public interface ICollector
    {
        Transform Transform { get; }
    }
}