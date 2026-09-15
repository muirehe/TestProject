using System;
using Configs;
using Gameplay.Combat;
using UnityEngine;

namespace Presentation.Abilities
{
    public readonly struct AbilityContext
    {
        public readonly Unit Unit;
        public readonly Transform Transform;
        public readonly CharacterController CharacterController;
        public readonly Vector3 Direction;
        public readonly int TargetMask;
        
        public AbilityContext(Unit unit, Transform transform, CharacterController characterController, Vector3 direction, int targetMask)
        {
            Unit = unit;
            Transform = transform;
            CharacterController = characterController;
            Direction = direction;
            TargetMask = targetMask;
        }
    }

    public interface IAbilityExecutor
    {
        Type ConfigType { get; }
        void Execute(AbilityConfig config, in AbilityContext context);
    }
    
    public abstract class AbilityExecutor<TConfig> : IAbilityExecutor where TConfig : AbilityConfig
    {
        public Type ConfigType => typeof(TConfig);
        public void Execute(AbilityConfig config, in AbilityContext context) => Execute((TConfig)config, context);
        protected abstract void Execute(TConfig config, in AbilityContext context);
    }
    
    public class DashExecutor : AbilityExecutor<DashAbilityConfig>
    {
        protected override void Execute(DashAbilityConfig config, in AbilityContext context)
        {
            context.CharacterController.Move(context.Direction * context.Unit.Modify(config.Distance,
                StatType.DashDistance));
        }
    }
    
    public class AoeExecutor : AbilityExecutor<AoeAbilityConfig> 
    {
        private readonly DamageService _damageService;
        private readonly Collider[] _results = new Collider[16];

        public AoeExecutor(DamageService damageService)
        {
            _damageService = damageService;
        }

        protected override void Execute(AoeAbilityConfig config, in AbilityContext context)
        {
            var size = Physics.OverlapSphereNonAlloc(context.Transform.position, config.Radius, _results,
                context.TargetMask, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < size; i++)
            {
                var hit = _results[i];
                var view = hit.GetComponentInParent<UnitView>();
                if (view && view.Unit != context.Unit && !view.Unit.IsDead)
                    _damageService.TryDealDamage(context.Unit, view.Unit, config.Damage);
            }
        }
    }
}
