using Configs;
using Gameplay.Combat;
using Infrastructure.Input;
using UnityEngine;
using Utils;
using VContainer;

namespace Presentation.Player
{
    public class PlayerAttackController : MonoBehaviour
    {
        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private Transform cameraRoot;

        private Unit _unit;
        private float _attackCooldown;

        private DamageService _damageService;
        private GameInput _gameInput;

        [Inject]
        private void Inject(DamageService damageService, GameInput gameInput)
        {
            _damageService = damageService;
            _gameInput = gameInput;
        }
        
        public void Bind(Unit unit) => _unit = unit;

        private void Update()
        {
            TryAttack();
        }

        private void TryAttack()
        {
            if (_attackCooldown > 0f)
                _attackCooldown -= Time.deltaTime;

            if (_gameInput.Player.Attack.WasPressedThisFrame() && _attackCooldown <= 0f)
            {
                _attackCooldown = _unit.Get(StatType.AttackCooldown);
                playerAnimator.PlayAttack();
                if (!Physics.Raycast(cameraRoot.position, cameraRoot.forward, out RaycastHit hit,
                        _unit.Get(StatType.AttackDistance),
                        LayerUtils.PlayerHitTargets, QueryTriggerInteraction.Ignore)) return;

                var unitView = hit.collider.GetComponentInParent<UnitView>();
                if (unitView)
                {
                    _damageService.TryDealDamage(_unit, unitView.Unit, _unit.Config.Weapon.Damage);
                }
            }
        }
    }
}