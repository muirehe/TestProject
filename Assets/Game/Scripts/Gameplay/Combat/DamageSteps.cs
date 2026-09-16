using Configs;
using UnityEngine;

namespace Gameplay.Combat
{
    public struct DamageContext
    {
        public Unit Source;
        public Unit Target;
        public float Amount;
        public bool IsCritical;
    }

    public interface IDamageStep
    {
        void Apply(ref DamageContext damageContext);
    }

    public class DamageModifierStep : IDamageStep
    {
        public void Apply(ref DamageContext damageContext)
        {
            damageContext.Amount = damageContext.Source.Modify(damageContext.Amount, StatType.Damage);
        }
    }

    public class CritStep : IDamageStep
    {
        public void Apply(ref DamageContext damageContext)
        {
            damageContext.IsCritical = Random.value < damageContext.Source.Get(StatType.CriticalChance);
            if (damageContext.IsCritical)
                damageContext.Amount *= damageContext.Source.Get(StatType.CriticalDamageMultiplier);
        }
    }

    public class ArmorStep : IDamageStep
    {
        public void Apply(ref DamageContext damageContext)
        {
            damageContext.Amount *= 1 - Mathf.Clamp01(damageContext.Target.Get(StatType.Armor));
        }
    }
}