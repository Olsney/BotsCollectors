using System;
using CodeBase.Castles;
using CodeBase.Flags;
using UnityEngine;

namespace CodeBase.Inputs
{
    [RequireComponent(typeof(FlagPlacer))]
    public class PlayerInput : MonoBehaviour
    {
        private const int LeftMouseButton = 0;
        
        private Camera _mainCamera;
        private FlagPlacer _flagPlacer;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _flagPlacer = GetComponent<FlagPlacer>();
        }

        private void Update()
        {
            HandleLeftButtonClick();
        }

        private void HandleLeftButtonClick()
        {
            if (ClickLeftButton())
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

                HandleRaycast(ray);
            }
        }

        private static bool ClickLeftButton() => 
            Input.GetMouseButtonDown(LeftMouseButton);

        private void HandleRaycast(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (HitFlagPlacer(hit))
                {
                    ChooseFlagPlacer(hit);

                    return;
                } 

                if (HitPointToBuild(hit)) 
                    PlaceFlag(hit);
            }
        }

        private static bool HitFlagPlacer(RaycastHit hit) => 
            hit.collider.TryGetComponent(out FlagPlacer flagPlacer);

        private void ChooseFlagPlacer(RaycastHit hit) => 
            _flagPlacer = hit.collider.GetComponent<FlagPlacer>();

        private bool HitPointToBuild(RaycastHit hit) => 
            hit.collider.TryGetComponent(out Collider collider) && _flagPlacer != null;

        private void PlaceFlag(RaycastHit hit)
        {
            _flagPlacer.Place(hit.point);
            _flagPlacer = null;
        }
    }
}