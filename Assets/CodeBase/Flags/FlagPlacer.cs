using System;
using CodeBase.Castles;
using UnityEditor.Searcher;
using UnityEngine;

namespace CodeBase.Flags
{
    public class FlagPlacer : MonoBehaviour
    {
        [SerializeField] private Flag _flagPrefab;
        
        private Flag _currentFlag;
        private Castle _castle;

        public bool IsPlaced { get; private set; }
        public event Action Placed;

        public void Place(Vector3 position)
        {
            TryDestroyPrevious();

            Flag _currentFlag = Instantiate(_flagPrefab, position, Quaternion.identity);

            IsPlaced = true;
        }
        
        private void TryDestroyPrevious()
        {
            if (_currentFlag != null)
                Destroy(_currentFlag.gameObject);

            IsPlaced = false;
        }
    }
}