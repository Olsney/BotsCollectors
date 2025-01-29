using System;
using CodeBase.Castles;
using CodeBase.Flags;
using CodeBase.Inputs;
using UnityEngine;

namespace CodeBase.Services
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(FlagPlacer))]

    public class FlagPlaceService : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private FlagPlacer _flagPlacer;
        
        private int CastleLayer;
        private int OtherLayer;
        
        private Castle _currentCastle;
        private int _castleLayerMaskNumber;
        private int _otherLayerMaskNumber;
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _flagPlacer = GetComponent<FlagPlacer>();
            
            CastleLayer = LayerMask.NameToLayer("Castle");
            OtherLayer = LayerMask.NameToLayer("Other");

            _castleLayerMaskNumber = 1 << CastleLayer;
            _otherLayerMaskNumber = 1 << OtherLayer;
        }

        private void OnEnable() => 
            _playerInput.RayCasted += Handle;

        private void OnDisable() => 
            _playerInput.RayCasted -= Handle;

        private void Handle(Ray ray)
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

        private bool TrySelectCastle(RaycastHit hit, out Castle castle)
        {
            castle = _currentCastle;

            int hitLayer = 1 << hit.collider.gameObject.layer;

            if (_castleLayerMaskNumber == hitLayer)
            {
                bool result = hit.collider.TryGetComponent(out castle);
                
                if(result && _currentCastle != null)
                    LoseFlagPlacerInCastle(_currentCastle);
                
                return result;
            }

            return false;
        }

        private bool TrySelectOther(RaycastHit hit)
        {
            int hitLayer = 1 << hit.collider.gameObject.layer;

            return hitLayer == _otherLayerMaskNumber;
        }

        private static void LoseFlagPlacerInCastle(Castle castle) =>
            castle.LoseFlagPlacer();

        private void SetFlagPlacerToCastle(Castle castle) => 
            castle.BecomeFlagPlacer(_flagPlacer);

        private void PlaceFlag(RaycastHit hit) => 
            _flagPlacer.Place(hit.point);
    }
}