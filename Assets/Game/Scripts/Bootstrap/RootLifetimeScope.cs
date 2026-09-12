using Infrastructure.ConfigSystem;
using Infrastructure.GameStateMachine;
using Infrastructure.GameStateMachine.States;
using Infrastructure.Input;
using Infrastructure.SaveSystem;
using Infrastructure.SceneSystem;
using MessagePipe;
using Modules;
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
            if (windowManager != null)
            {
                var uiRoot = Instantiate(windowManager);
                DontDestroyOnLoad(uiRoot);
                builder.RegisterComponent(uiRoot);
                builder.RegisterBuildCallback(resolver => resolver.InjectGameObject(uiRoot.gameObject));
            }

            RegisterMessagePipe(builder);
            RegisterModules(builder);
            RegisterStates(builder);
            RegisterServices(builder);
        }

        private void RegisterMessagePipe(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
            //builder.RegisterMessageBroker<>(options);
        }

        private void RegisterModules(IContainerBuilder builder)
        {
            builder.Register<PlayerModule>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }

        private void RegisterStates(IContainerBuilder builder)
        {
            builder.Register<IGameStateMachine, GameStateMachine>(Lifetime.Singleton).As<IInitializable>();
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<MenuState>(Lifetime.Singleton);
            builder.Register<GameplayState>(Lifetime.Singleton);
            builder.Register<ResultsState>(Lifetime.Singleton);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton);
            builder.Register<GameInput>(Lifetime.Singleton);
            builder.Register<ConfigProvider>(Lifetime.Singleton);
            builder.Register<SaveService>(Lifetime.Singleton);
        }
    }
}