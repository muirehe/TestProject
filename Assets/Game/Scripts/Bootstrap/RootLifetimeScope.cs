using Game.Scripts.Presentation;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Bootstrap
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
            //builder.Register<PlayerModule>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
           }

        private void RegisterStates(IContainerBuilder builder)
        {
            //builder.Register<>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterServices(IContainerBuilder builder)
        {
        }
    }
}
