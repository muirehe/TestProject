using Configs;

namespace Gameplay.Combat
{
    public static class ModifierCalculator
    {
        public static float ProcessModifiers(UnitConfig unitConfig, StatType statType) =>
            ProcessModifiers(GetBase(unitConfig, statType), unitConfig, statType);

        private static float GetBase(UnitConfig unitConfig, StatType statType)
        {
            var value = statType switch
            {
                StatType.MaxHealth => unitConfig.MaxHealth,
                StatType.AttackCooldown => unitConfig.Weapon.AttackCooldown,
                StatType.Damage => unitConfig.Weapon.Damage,
                StatType.MoveSpeed => unitConfig.MoveSpeed,
                StatType.CriticalChance => unitConfig.Weapon.CriticalChance,
                StatType.CriticalDamageMultiplier => unitConfig.Weapon.CriticalDamageMultiplier,
                StatType.Armor => unitConfig.Armor,
                StatType.AttackDistance => unitConfig.Weapon.AttackDistance,
                _ => throw new System.NotImplementedException()
            };
            return value;
        }

        public static float ProcessModifiers(float value, UnitConfig unitConfig, StatType statType)
        {
            foreach (var modifier in unitConfig.Modifiers)
            {
                if (modifier.StatType == statType && modifier.CalculationType == CalculationType.Abs)
                    ProcessCalculation(ref value, modifier.Value, modifier.CalculationType);
            }
            
            foreach (var modifier in unitConfig.Modifiers)
            {
                if (modifier.StatType == statType && modifier.CalculationType == CalculationType.Pct)
                    ProcessCalculation(ref value, modifier.Value, modifier.CalculationType);
            }

            return value;
        }
        
        private static void ProcessCalculation(ref float value, float modificator, CalculationType calculationType)
        {
            switch (calculationType)
            {
                case CalculationType.Abs:
                    value += modificator;
                    break;
                case CalculationType.Pct:
                    value *= 1f + modificator;
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }
    }
}