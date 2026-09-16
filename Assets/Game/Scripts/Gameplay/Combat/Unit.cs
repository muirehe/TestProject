using System.Collections.Generic;
using Configs;
using UnityEngine;

namespace Gameplay.Combat
{
    public class Unit
    {
        private readonly Dictionary<AbilityConfig, float> _abilitiesReadyTime = new();

        public Unit(UnitConfig unitConfig)
        {
            Config = unitConfig;
            Health = MaxHealth;
            InitAbilities();
        }

        private void InitAbilities()
        {
            if (Config.Weapon) _abilitiesReadyTime[Config.Weapon] = 0f;
            foreach (var abilityConfig in Config.Abilities)
                _abilitiesReadyTime[abilityConfig] = 0f;
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

        public float GetAbilityCooldown(AbilityConfig config)
        {
            if (_abilitiesReadyTime.TryGetValue(config, out float readyTime))
                return Mathf.Max(0f, readyTime - Time.time);
            return float.MaxValue;
        }

        public bool TryStartCooldownAbility(AbilityConfig config, float cooldown)
        {
            if (IsDead || !_abilitiesReadyTime.TryGetValue(config, out float readyTime) ||
                Time.time < readyTime) return false;
            _abilitiesReadyTime[config] = Time.time + cooldown;
            return true;
        }
    }
}