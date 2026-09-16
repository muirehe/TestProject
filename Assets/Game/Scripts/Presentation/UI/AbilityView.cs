using Configs;
using Gameplay.Combat;
using TMPro;
using UnityEngine;

namespace Presentation.UI
{
    public class AbilityView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI abilityNameText;
        [SerializeField] private TextMeshProUGUI cooldownText;
        
        public Unit Unit { get; private set; }
        public AbilityConfig AbilityConfig { get; private set; }
        
        public void Setup(Unit unit, AbilityConfig abilityConfig)
        {
            Unit = unit;
            AbilityConfig = abilityConfig;
            abilityNameText.text = AbilityConfig.Id;
        }
         
        private void Update()
        {
            cooldownText.text = $"Cooldown: {Unit.GetAbilityCooldown(AbilityConfig):F1}";
        }
    }
}