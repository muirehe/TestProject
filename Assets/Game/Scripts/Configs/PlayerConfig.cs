using System.Collections.Generic;
using Infrastructure.ConfigSystem;
using UnityEngine;

namespace Configs
{
    public abstract class UnitConfig : DefinitionConfig
    {
        [field: SerializeField] public float MaxHealth { get; private set; } = 100f;
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;
        [field: Range(0f, 1f)][field: SerializeField] public float Armor { get; private set; } = 0f;
        [field: SerializeField] public WeaponConfig Weapon { get; private set; }
        [field: SerializeField] public List<ModifierConfig> Modifiers { get; private set; }
    }

    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : UnitConfig, ISingleDefinition
    {
    }
}