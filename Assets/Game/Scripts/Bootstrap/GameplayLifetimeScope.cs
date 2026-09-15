using Gameplay.Battle;
using Gameplay.Combat;
using Gameplay.Player;
using Presentation.Level;
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
            builder.Register<PlayerModel>(Lifetime.Scoped);
        }
    }
}