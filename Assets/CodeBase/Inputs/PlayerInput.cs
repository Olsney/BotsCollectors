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
                hit.collider.TryGetComponent<Castle>(out Castle castle);

                if (CastleHasFlagPlacer(castle) && castle != null)
                    LoseFlagPlacerInCastle(castle);

                if (HitFlagPlacer(hit))
                {
                    ChooseFlagPlacer(hit);
                    SetFlagPlacerToCastle(hit);

                    return;
                }

                if (HitPointToBuild(hit))
                {
                    PlaceFlag(hit);

                    if (castle != null)
                        castle.LostFlagPlacer();
                }
            }
        }

        private static bool CastleHasFlagPlacer(Castle castle) =>
            castle.FlagPlacer != null;

        private static void LoseFlagPlacerInCastle(Castle castle) =>
            castle.LostFlagPlacer();

        private static bool HitFlagPlacer(RaycastHit hit) =>
            hit.collider.TryGetComponent(out FlagPlacer flagPlacer);

        private void ChooseFlagPlacer(RaycastHit hit) =>
            _flagPlacer = hit.collider.GetComponent<FlagPlacer>();

        private void SetFlagPlacerToCastle(RaycastHit hit)
        {
            Castle castle = hit.collider.GetComponent<Castle>();
            castle.BecomeFlagPlacer(_flagPlacer);
        }

        private bool HitPointToBuild(RaycastHit hit) =>
            hit.collider.TryGetComponent(out Collider collider) && _flagPlacer != null;

        private void PlaceFlag(RaycastHit hit)
        {
            _flagPlacer.Place(hit.point);
            _flagPlacer = null;
        }
    }
}