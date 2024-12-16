using UnityEngine;

namespace CodeBase.CollectorsBases
{
    public class BaseDropPlace : MonoBehaviour, IBaseDropPlace
    {
        public Vector3 DropPlacePoint => transform.position;
    }
}