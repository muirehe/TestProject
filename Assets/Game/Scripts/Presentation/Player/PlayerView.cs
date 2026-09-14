using Gameplay.Combat;
using Gameplay.Player;
using UnityEngine;
using VContainer;

namespace Presentation.Player
{
    public class PlayerView : UnitView
    {
        [SerializeField] private PlayerMoveController playerMoveController;
        [SerializeField] private PlayerAttackController playerAttackController;

        [Inject]
        private void Inject(PlayerModel playerModel) => Init(playerModel.Unit);

        protected override void OnInit(Unit unit)
        {
            playerMoveController.Bind(unit);
            playerAttackController.Bind(unit);
        }
    }
}