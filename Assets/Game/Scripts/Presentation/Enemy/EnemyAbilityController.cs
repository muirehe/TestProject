using Gameplay.Player;
using Presentation.Abilities;
using Utils;
using VContainer;

namespace Presentation.Enemy
{
    public class EnemyAbilityController : AbilityController
    {
        [Inject] private PlayerModel _playerModel;
        
        private void Update()
        {
            if (_playerModel.Unit.IsDead) return;

            foreach (var ability in Unit.Config.Abilities)
            {
                if (Unit.GetAbilityCooldown(ability) <= 0)
                {
                    TryUseAbility(ability);
                }
            }
        }

        protected override int TargetMask => LayerUtils.PlayerMask;
    }
}