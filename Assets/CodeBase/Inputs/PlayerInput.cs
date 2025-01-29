using System;
using UnityEngine;

namespace CodeBase.Inputs
{
    public class PlayerInput : MonoBehaviour
    {
        private const int LeftMouseButton = 0;
        private Camera _mainCamera;

        public event Action<Ray> RayCasted;

        private void Awake() => 
            _mainCamera = Camera.main;

        private void Update() => 
            HandleLeftButtonClick();

        private void HandleLeftButtonClick()
        {
            if (IsLeftButtonClicked())
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                
                RayCasted?.Invoke(ray);
            }
        }

        private static bool IsLeftButtonClicked() =>
            Input.GetMouseButtonDown(LeftMouseButton);
        
    }
}