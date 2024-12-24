using System;
using CodeBase.CollectorsBases;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.UI
{
    public class BaseResourceView : MonoBehaviour
    {
        [SerializeField] private CollectorsBase _collectorsBase;
        [SerializeField] private TMP_Text _textMesh; 

        private void OnEnable() => 
            _collectorsBase.ResourcesCollected += OnResourcesCollected;

        private void OnResourcesCollected(int count) => 
            _textMesh.text = $"{count}";
    }
}