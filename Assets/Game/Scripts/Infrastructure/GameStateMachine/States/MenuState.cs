using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.SceneSystem;

namespace Infrastructure.GameStateMachine.States
{
    public class MenuState : IState, IDisposable
    {
        private CancellationTokenSource _ctx;
        private readonly SceneLoader _sceneLoader;

        public MenuState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _ctx = new CancellationTokenSource();
            _sceneLoader.LoadAsync(SceneLoader.Menu, null, _ctx.Token).Forget();
        }


        public void Exit()
        {
            _ctx?.Cancel();
            _ctx?.Dispose();
            _ctx = null;
        }

        public void Dispose()
        {
            _ctx?.Cancel();
            _ctx?.Dispose();
            _ctx = null;
        }
    }
}