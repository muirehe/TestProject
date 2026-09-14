using Gameplay.Battle;
using Gameplay.Combat;
using Gameplay.Player;
using Presentation.Level;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Bootstrap
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private LevelView levelView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(levelView);
            builder.Register<BattleController>(Lifetime.Scoped).AsSelf().As<IInitializable>();
            builder.Register<DamageService>(Lifetime.Scoped);
            builder.Register<PlayerModel>(Lifetime.Scoped);
        }
    }
}