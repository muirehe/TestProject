using Gameplay.Combat;
using Gameplay.Player;
using Gameplay.Statistics;
using Presentation.Abilities;
using Presentation.Level;
using Presentation.Level.Battle;
using Presentation.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Bootstrap
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private LevelView levelView;
        [SerializeField] private PlayerView playerView; 
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(levelView);
            builder.RegisterInstance(playerView);
            
            builder.Register<BattleController>(Lifetime.Scoped).AsSelf().As<IInitializable>();
            builder.Register<DamageService>(Lifetime.Scoped);
            builder.Register<BattleStatisticsService>(Lifetime.Scoped).As<IInitializable>();
            builder.Register<PlayerModel>(Lifetime.Scoped);
            RegisterExecutors(builder);
        }

        private void RegisterExecutors(IContainerBuilder builder)
        {
            builder.Register<DashExecutor>(Lifetime.Scoped).As<IAbilityExecutor>();
            builder.Register<AoeExecutor>(Lifetime.Scoped).As<IAbilityExecutor>();
            builder.Register<MeleeAttackExecutor>(Lifetime.Scoped).As<IAbilityExecutor>();
        }
    }
}