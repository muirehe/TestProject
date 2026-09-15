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

        private Dictionary<Type, IAbilityExecutor> _executors;

        [Inject]
        private void Inject(IReadOnlyList<IAbilityExecutor> executors)
        {
            _executors = executors.ToDictionary(executor => executor.ConfigType, executor => executor);
        }

        [Inject] private IPublisher<AbilityUsed> _publisher;

        protected abstract int TargetMask { get; }
        protected virtual Vector3 AbilityDirection => tr.forward;

        protected Unit Unit;
        public void Bind(Unit unit) => Unit = unit;

        public void TryUseAbility(AbilityConfig abilityConfig)
        {
            if (!_executors.TryGetValue(abilityConfig.GetType(), out var executor))
            {
                Debug.LogWarning($"[Ability] no executor for {abilityConfig.Id}.");
                return;
            }

            if (!Unit.TryStartCooldownAbility(abilityConfig)) return;

            executor.Execute(abilityConfig,
                new AbilityContext(Unit, tr, characterController, AbilityDirection, TargetMask));
            _publisher.Publish(new AbilityUsed(Unit, abilityConfig));
        }
    }
}