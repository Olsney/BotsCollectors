using UnityEngine;

namespace CodeBase.Castles
{
    public class CastleDropPlace : MonoBehaviour, ICastleDropPlace
    {
        public Vector3 DropPlacePoint => transform.position;
    }
}