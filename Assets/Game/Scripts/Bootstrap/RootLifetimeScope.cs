using Gameplay.Battle;
using Gameplay.Combat;
using Infrastructure.ConfigSystem;
using Infrastructure.GameStateMachine;
using Infrastructure.GameStateMachine.States;
using Infrastructure.Input;
using Infrastructure.SaveSystem;
using Infrastructure.SceneSystem;
using MessagePipe;
using Meta;
using Presentation;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Bootstrap
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private WindowManager windowManager;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(windowManager, Lifetime.Singleton).DontDestroyOnLoad();
            builder.RegisterBuildCallback(c => c.Resolve<WindowManager>());
            
            RegisterInfrastructure(builder);
            RegisterMeta(builder);
            RegisterMessages(builder);
            RegisterStates(builder);
        }

        private void RegisterInfrastructure(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton);
            builder.Register<GameInput>(Lifetime.Singleton);
            builder.Register<ConfigProvider>(Lifetime.Singleton);
            builder.Register<SaveService>(Lifetime.Singleton);
        }
        
        private void RegisterMeta(IContainerBuilder builder)
        {
            builder.Register<GameSession>(Lifetime.Singleton);
        }
        
        private void RegisterMessages(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<DamageApplied>(options);
            builder.RegisterMessageBroker<EntityDied>(options);
            builder.RegisterMessageBroker<BattleEnded>(options);
        }

        private void RegisterStates(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameStateMachine>().As<IGameStateMachine>();
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<MenuState>(Lifetime.Singleton);
            builder.Register<GameplayState>(Lifetime.Singleton);
            builder.Register<ResultsState>(Lifetime.Singleton);
        }
    }
}