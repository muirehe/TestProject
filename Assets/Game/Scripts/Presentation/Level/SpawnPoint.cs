using UnityEngine;

namespace Presentation.Level
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Transform tr;
        
        public Transform Transform => tr;
        private void OnDrawGizmos()
        {
            if (tr == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(tr.position, 0.5f);
        }
    }
}