using Configs;
using UnityEngine;

namespace Gameplay.Combat
{
    public class Unit
    {
        public Unit(UnitConfig unitConfig)
        {
            Config = unitConfig;
            Health = MaxHealth;
        }

        public UnitConfig Config { get; }
        public float Health { get; private set; }
        public float MaxHealth => Get(StatType.MaxHealth);

        public bool IsDead => Health <= 0f;

        public void ApplyDamage(float damage)
        {
            Health = Mathf.Max(0f, Health - damage);
        }
        
        public float Get(StatType statType) => ModifierCalculator.ProcessModifiers(Config, statType);

        public float Modify(float value, StatType statType) =>
            ModifierCalculator.ProcessModifiers(value, Config, statType);
    }
}