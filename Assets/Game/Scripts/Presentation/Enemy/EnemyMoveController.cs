using Configs;
using Gameplay.Combat;
using Presentation.Player;
using UnityEngine;
using VContainer;

namespace Presentation.Enemy
{
    public class EnemyMoveController : MonoBehaviour
    {
        [SerializeField] private Transform tr;
        [SerializeField] private CharacterController characterController;
        private Unit _unit;

        public void Bind(Unit unit) => _unit = unit;

        [Inject] private PlayerView _playerView;

        private void Update()
        {
            if (_playerView.Unit.IsDead || _unit.IsDead) return;
            Look();
            Move();
        }

        private void Look()
        {
            var dir = _playerView.Tr.position - tr.position;
            dir.y = 0;
            tr.rotation = Quaternion.LookRotation(dir.normalized);
        }

        private void Move()
        {
            var attackDistance = _unit.Get(StatType.AttackDistance);
            if ((_playerView.Tr.position - tr.position).sqrMagnitude > attackDistance * attackDistance)
            {
                characterController.SimpleMove(tr.forward * _unit.Get(StatType.MoveSpeed));
            }
        }
    }
}