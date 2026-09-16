using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/WeaponConfig")]
    public class WeaponConfig : AbilityConfig
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float AttackDistance { get; private set; }
        [field: SerializeField] public float CriticalChance { get; private set; }
        [field: SerializeField] public float CriticalDamageMultiplier { get; private set; }
    }
}