using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Gameplay.Combat;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace Presentation.Abilities
{
    public abstract class AbilityController : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] protected Transform tr;
        [SerializeField] protected WeaponAnimator weaponAnimator;

        private Dictionary<Type, IAbilityExecutor> _executors;

        [Inject]
        private void Inject(IReadOnlyList<IAbilityExecutor> executors)
        {
            _executors = executors.ToDictionary(executor => executor.ConfigType, executor => executor);
        }
        [Inject] private IPublisher<AbilityUsed> _publisher;

        protected abstract int TargetMask { get; }
        protected virtual Vector3 AbilityDirection => tr.forward;
        protected virtual Transform AimOrigin => tr;

        protected Unit Unit;
        public void Bind(Unit unit) => Unit = unit;

        public bool TryUseAbility(AbilityConfig abilityConfig)
        {
            if (!_executors.TryGetValue(abilityConfig.GetType(), out var executor))
            {
                Debug.LogWarning($"[Ability] no executor for {abilityConfig.Id}.");
                return false;
            }

            if (!Unit.TryStartCooldownAbility(abilityConfig, executor.GetCooldown(abilityConfig, Unit))) return false;

            var aim = new Ray(AimOrigin.position, AimOrigin.forward);
            executor.Execute(abilityConfig,
                new AbilityContext(Unit, tr, characterController, AbilityDirection, aim, TargetMask));
            _publisher.Publish(new AbilityUsed(Unit, abilityConfig));
            return true;
        }

        protected void TryAttack()
        {
            if (Unit.Config.Weapon && TryUseAbility(Unit.Config.Weapon))
                weaponAnimator.PlayAttack();
        }
    }
}