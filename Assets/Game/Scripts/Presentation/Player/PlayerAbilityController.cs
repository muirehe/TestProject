using Infrastructure.Input;
using Presentation.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using VContainer;

namespace Presentation.Player
{
    public class PlayerAbilityController : AbilityController
    {
        private InputAction[] _keys;

        [Inject] private GameInput _gameInput;

        protected override int TargetMask => LayerUtils.EnemyMask;

        protected override Vector3 AbilityDirection
        {
            get
            {
                var input = _gameInput.Player.Move.ReadValue<Vector2>();
                var direction = tr.right * input.x + tr.forward * input.y;
                return direction.sqrMagnitude > 0.01f ? direction.normalized : tr.forward;
            }
        }

        private void Start() => _keys = new[] { _gameInput.Player.UseAbility1, _gameInput.Player.UseAbility2 };

        private void Update()
        {
            for (var i = 0; i < _keys.Length && i < Unit.Config.Abilities.Count; i++)
            {
                if (_keys[i].WasPressedThisFrame())
                    TryUseAbility(Unit.Config.Abilities[i]);
            }
        }
    }
}