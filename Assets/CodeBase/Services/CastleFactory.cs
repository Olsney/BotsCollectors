using CodeBase.Castles;
using UnityEngine;

namespace CodeBase.Services
{
    public class CastleFactory : MonoBehaviour
    {
        [SerializeField] private Castle _castlePrefab;

        public Castle Create(Vector3 position) => 
            Instantiate(_castlePrefab, position, Quaternion.identity, null);
    }
}