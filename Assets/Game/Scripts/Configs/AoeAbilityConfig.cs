using Presentation.Abilities;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "AoeAbilityConfig", menuName = "Configs/AoeAbilityConfig")]
    public class AoeAbilityConfig : AbilityConfig
    {
        [field: SerializeField] public float Radius { get; private set; } = 3f;
        [field: SerializeField] public float Damage { get; private set; } = 20f;
        [field: SerializeField] public AoeVfx Vfx { get; private set; }
    }
}