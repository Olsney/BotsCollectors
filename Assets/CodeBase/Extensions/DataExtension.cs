using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Extensions
{
    public static class DataExtension
    {
        public static Vector3 GetRandomPosition(List<Vector3> positions) =>
            positions[Random.Range(0, positions.Count)];
        
        public static float SqrDistance(this Vector3 start, Vector3 end) => 
            (end - start).sqrMagnitude;
    }
}