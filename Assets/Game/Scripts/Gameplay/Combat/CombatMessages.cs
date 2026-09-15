using Configs;

namespace Gameplay.Combat
{
    public readonly struct DamageApplied

    {
        public readonly Unit Target;
        public readonly Unit Source;
        public readonly float Amount;
        public readonly bool IsCritical;

        public DamageApplied(Unit target, Unit source, float amount, bool isCritical)
        {
            Target = target;
            Source = source;
            Amount = amount;
            IsCritical = isCritical;
        }
    }
    
    public readonly struct EntityDied
    {
        public readonly Unit Target;

        public EntityDied(Unit target)
        {
            Target = target;
        }
    }
    
    public readonly struct AbilityUsed
    {
        public readonly Unit Unit;
        public readonly AbilityConfig AbilityConfig;

        public AbilityUsed(Unit unit, AbilityConfig abilityConfig)
        {
            Unit = unit;
            AbilityConfig = abilityConfig;
        }
    }
}