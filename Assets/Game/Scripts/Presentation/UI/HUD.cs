using Gameplay.Player;
using Infrastructure.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Presentation.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private HealthView healthView;
        [SerializeField] private TextMeshProUGUI abilityHintText;

        [Inject] private GameInput _gameInput;
        [Inject] private PlayerModel _playerModel;
        
        private void Start()
        {
            healthView.Bind(_playerModel.Unit);
            
            var keys = new[] { _gameInput.Player.UseAbility1, _gameInput.Player.UseAbility2 };
            var abilityHint = string.Empty;
            for (var i = 0; i < keys.Length && i < _playerModel.Unit.Config.Abilities.Count; i++)
            {
                abilityHint += (i > 0 ? "\n" : string.Empty) +
                $"{keys[i].GetBindingDisplayString()} to use {_playerModel.Unit.Config.Abilities[i].Id}";
            }
            abilityHintText.text = abilityHint;
        }
    }
}