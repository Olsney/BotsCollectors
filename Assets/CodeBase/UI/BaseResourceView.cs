using CodeBase.Castles;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.UI
{
    public class BaseResourceView : MonoBehaviour
    {
        [FormerlySerializedAs("_collectorsBase")] [SerializeField] private Castle castle;
        [SerializeField] private TMP_Text _textMesh;
        private Camera _mainCamera;

        private void Start() => 
            _mainCamera = Camera.main;

        private void OnEnable() => 
            castle.ResourceCollected += OnResourcesCollected;

        private void LateUpdate() => 
            ConfigureLooking();

        private void ConfigureLooking() => 
            transform.LookAt(transform.position + _mainCamera.transform.forward);

        private void OnResourcesCollected(int count) => 
            _textMesh.text = $"{count}";
    }
}