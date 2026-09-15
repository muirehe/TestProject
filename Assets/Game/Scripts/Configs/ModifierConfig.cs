using Infrastructure.ConfigSystem;
using UnityEngine;

namespace Configs
{
    public enum StatType
    {
        Damage,
        MaxHealth,
        MoveSpeed,
        AttackCooldown,
        CriticalChance,
        CriticalDamageMultiplier,
        Armor,
        AttackDistance,
        AbilityCooldown,
        DashDistance
    }
    
    public enum CalculationType
    {
        Abs,
        Pct,
    }

    [CreateAssetMenu(fileName = "ModifierConfig", menuName = "Configs/ModifierConfig")]
    public class ModifierConfig : DefinitionConfig, IDefinitionById
    {
        [field: SerializeField] public StatType StatType { get; private set; }
        [field: SerializeField] public float Value { get; private set; }    
        [field: SerializeField] public CalculationType CalculationType { get; private set; }
        public string Id => name;
    }
}