using CodeBase.Castles;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.UI
{
    public class CastleResourceView : MonoBehaviour
    {
        [FormerlySerializedAs("castle")] [FormerlySerializedAs("_collectorsBase")] [SerializeField] private Castle _castle;
        [SerializeField] private TMP_Text _textMesh;
        private Camera _mainCamera;

        private void Start() => 
            _mainCamera = Camera.main;

        private void OnEnable() => 
            _castle.ResourceCollected += OnResourcesCollected;

        private void LateUpdate() => 
            ConfigureLooking();

        private void ConfigureLooking() => 
            transform.LookAt(transform.position + _mainCamera.transform.forward);

        private void OnResourcesCollected(int count) => 
            _textMesh.text = $"{count}";
    }
}