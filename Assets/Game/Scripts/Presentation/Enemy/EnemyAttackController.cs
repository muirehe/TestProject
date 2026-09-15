using Configs;
using Gameplay.Combat;
using Presentation.Player;
using UnityEngine;
using Utils;
using VContainer;

namespace Presentation.Enemy
{
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField] private WeaponAnimator weaponAnimator;
        [SerializeField] private Transform tr;

        private Unit _unit;
        private float _attackCooldown;

        [Inject] private DamageService _damageService;
        [Inject] private PlayerView _playerView;

        public void Bind(Unit unit) => _unit = unit;


        private void Update()
        {
            if (_playerView.Unit.IsDead || _unit.IsDead) return;
            TryAttack();
        }

        private void TryAttack()
        {
            if (_attackCooldown > 0f)
                _attackCooldown -= Time.deltaTime;

            var attackDistance = _unit.Get(StatType.AttackDistance);
            if (_attackCooldown <= 0f &&
                (_playerView.Tr.position - tr.position).sqrMagnitude <= attackDistance * attackDistance)
            {
                _attackCooldown = _unit.Get(StatType.AttackCooldown);
                weaponAnimator.PlayAttack();
                if (!Physics.Raycast(tr.position, tr.forward, out RaycastHit hit,
                        _unit.Get(StatType.AttackDistance),
                        LayerUtils.EnemyHitTargets, QueryTriggerInteraction.Ignore)) return;
                var playerView = hit.collider.GetComponentInParent<PlayerView>();
                if (playerView)
                    _damageService.TryDealDamage(_unit, playerView.Unit, _unit.Config.Weapon.Damage);
            }
        }
    }
}