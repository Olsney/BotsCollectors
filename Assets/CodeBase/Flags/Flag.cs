using UnityEngine;

namespace CodeBase.Flags
{
    public class Flag : MonoBehaviour
    {
        public void Destroy() => 
            Destroy(gameObject);
    }
}