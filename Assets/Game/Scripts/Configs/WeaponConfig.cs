using Infrastructure.ConfigSystem;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/WeaponConfig")]
    public class WeaponConfig : DefinitionConfig, IDefinitionById
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }
        [field: SerializeField] public float AttackDistance { get; private set; }
        [field: SerializeField] public float CriticalChance { get; private set; }
        [field: SerializeField] public float CriticalDamageMultiplier { get; private set; }
        public string Id => name;
    }
}