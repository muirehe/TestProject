using UnityEngine;

namespace Presentation.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator weaponAnimator;
        [SerializeField] private string attackAnimKey = "Attack";

        private int _attackHash; 

        private void Awake()
        {
            _attackHash = Animator.StringToHash(attackAnimKey);
        }
        
        public void PlayAttack()
        {
            weaponAnimator.SetTrigger(_attackHash);
        }

    }
}