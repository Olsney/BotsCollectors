using System;
using CodeBase.Castles;
using UnityEngine;

namespace CodeBase.Flags
{
    public class FlagPlacer : MonoBehaviour
    {
        [SerializeField] private Flag _flagPrefab;
        
        private Flag _currentFlag;
        private Castle _castle;

        public event Action<Flag> Placed;

        public void Place(Vector3 position)
        {
            TryDestroyPrevious();

            _currentFlag = Instantiate(_flagPrefab, position, Quaternion.identity);

            Placed?.Invoke(_currentFlag);
        }
        
        private void TryDestroyPrevious()
        {
            if (_currentFlag != null)
                Destroy(_currentFlag.gameObject);
        }
    }
}