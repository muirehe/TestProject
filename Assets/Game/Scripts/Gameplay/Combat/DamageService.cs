using MessagePipe;
using UnityEngine;

namespace Gameplay.Combat
{
    public class DamageService
    {
        private readonly IDamageStep[] _damageSteps =
        {
            new DamageModifierStep(),
            new CritStep(),
            new ArmorStep()
        };

        private readonly IPublisher<DamageApplied> _damageAppliedPublisher;
        private readonly IPublisher<EntityDied> _entityDiedPublisher;

        public DamageService(IPublisher<DamageApplied> damageAppliedPublisher,
            IPublisher<EntityDied> entityDiedPublisher)
        {
            _damageAppliedPublisher = damageAppliedPublisher;
            _entityDiedPublisher = entityDiedPublisher;
        }

        public bool TryDealDamage(Unit source, Unit target, float damage)
        {
            if (target.IsDead) return false;

            var damageContext = new DamageContext()
            {
                Source = source,
                Target = target,
                Amount = damage
            };
            foreach (var step in _damageSteps)
                step.Apply(ref damageContext);
            var realDamage = Mathf.Min(damageContext.Amount, target.Health);
            if (realDamage <= 0)
                return false;
            target.ApplyDamage(realDamage);
            _damageAppliedPublisher.Publish(new DamageApplied(target, source, realDamage, damageContext.IsCritical));
            if (target.IsDead)
                _entityDiedPublisher.Publish(new EntityDied(target));
            return true;
        }
    }
}