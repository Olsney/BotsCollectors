using System;
using CodeBase.Castles;
using CodeBase.Flags;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Inputs
{
    [RequireComponent(typeof(FlagPlacer))]
    public class PlayerInput : MonoBehaviour
    {
        private const int LeftMouseButton = 0;

        private int CastleLayer;
        private int OtherLayer;

        [SerializeField] private LayerMask _layerMaskCastle;
        [SerializeField] private LayerMask _layerMaskOthers;

        private Camera _mainCamera;
        private FlagPlacer _flagPlacer;
        private Castle _currentCastle;
        private int _castleLayerMaskNumber;
        private int _otherLayerMaskNumber;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _flagPlacer = GetComponent<FlagPlacer>();

            CastleLayer = LayerMask.NameToLayer("Castle");
            OtherLayer = LayerMask.NameToLayer("Other");

            _castleLayerMaskNumber = 1 << CastleLayer;
            _otherLayerMaskNumber = 1 << OtherLayer;
        }

        private void Update()
        {
            HandleLeftButtonClick();
        }

        private void HandleLeftButtonClick()
        {
            if (IsLeftButtonClicked())
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

                HandleRaycast(ray);
            }
        }

        private static bool IsLeftButtonClicked() =>
            Input.GetMouseButtonDown(LeftMouseButton);

        private void HandleRaycast(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (TrySelectCastle(hit, out Castle castle))
                {
                    _currentCastle = castle;
                    SetFlagPlacerToCastle(_currentCastle);
                }
                else if (TrySelectOther(hit))
                {
                    if (_currentCastle == null)
                        return;
                    
                    PlaceFlag(hit);
                }
            }
        }

        private bool TrySelectOther(RaycastHit hit)
        {
            int hitLayer = 1 << hit.collider.gameObject.layer;

            Debug.Log("Try select other!");
            Debug.Log($"{hitLayer} - hit other");
            Debug.Log($"{_otherLayerMaskNumber} - otherLayerMask value");

            return hitLayer == _otherLayerMaskNumber;
        }

        private bool TrySelectCastle(RaycastHit hit, out Castle castle)
        {
            castle = _currentCastle;

            Debug.Log($"{_castleLayerMaskNumber} - castle layerMask number");
            int hitLayer = 1 << hit.collider.gameObject.layer;
            Debug.Log($"{hitLayer} - hit layerMask number");

            if (_castleLayerMaskNumber == hitLayer)
            {
                Debug.Log($"we are in if");

                bool result = hit.collider.TryGetComponent(out castle);
                
                if(result && _currentCastle != null)
                    LoseFlagPlacerInCastle(_currentCastle);
                
                return result;
            }

            return false;
        }

        private static void LoseFlagPlacerInCastle(Castle castle) =>
            castle.LoseFlagPlacer();

        private void SetFlagPlacerToCastle(Castle castle)
        {
            castle.BecomeFlagPlacer(_flagPlacer);
        }

        private void PlaceFlag(RaycastHit hit)
        {
            _flagPlacer.Place(hit.point);
            _flagPlacer = null;
        }
    }
}