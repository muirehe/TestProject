using Infrastructure.Input;
using UnityEngine;
using VContainer;

namespace Presentation.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] public Animator weaponAnimator;
        [SerializeField] public string attackAnimKey = "Attack";

        private int AttackHash => Animator.StringToHash(attackAnimKey);

        [Inject] private GameInput _gameInput;
        
        private void Update()
        {
            if (_gameInput.Player.Attack.WasPressedThisFrame())
                weaponAnimator.SetTrigger(AttackHash);
        }

    }
}