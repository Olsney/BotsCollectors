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
        private Camera _mainCamera;

        private void Start() => 
            _mainCamera = Camera.main;

        private void OnEnable() => 
            _collectorsBase.ResourcesCollected += OnResourcesCollected;

        private void LateUpdate() => 
            ConfigureLooking();

        private void ConfigureLooking() => 
            transform.LookAt(transform.position + _mainCamera.transform.forward);

        private void OnResourcesCollected(int count) => 
            _textMesh.text = $"{count}";
    }
}