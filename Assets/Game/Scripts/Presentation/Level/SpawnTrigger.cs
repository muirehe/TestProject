using System;
using Presentation.Player;
using UnityEngine;

namespace Presentation.Level
{
    public class SpawnTrigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider boxCollider;

        public event Action PlayerEntered;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<PlayerView>() != null)
            {
                boxCollider.enabled = false;
                PlayerEntered?.Invoke();
            }
        }
    }
}