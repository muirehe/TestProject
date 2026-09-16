using Configs;
using Gameplay.Player;
using Presentation.Abilities;
using Presentation.Player;
using Utils;
using VContainer;

namespace Presentation.Enemy
{
    public class EnemyAbilityController : AbilityController
    {
        [Inject] private PlayerModel _playerModel;
        [Inject] private PlayerView _playerView;

        protected override int TargetMask => LayerUtils.PlayerMask;

        private void Update()
        {
            if (_playerModel.Unit.IsDead || Unit.IsDead) return;

            var distance = Unit.Get(StatType.AttackDistance);
            if ((_playerView.Tr.position - tr.position).sqrMagnitude < distance * distance)
                TryAttack();

            foreach (var ability in Unit.Config.Abilities)
            {
                if (Unit.GetAbilityCooldown(ability) <= 0)
                    TryUseAbility(ability);
            }
        }
    }
}