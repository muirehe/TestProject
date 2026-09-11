using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.SceneSystem;

namespace Infrastructure.GameStateMachine.States
{
    public class GameplayState : IState, IDisposable
    {
        private CancellationTokenSource _ctx = new();
        private readonly SceneLoader _sceneLoader;
        
        public GameplayState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        public void Enter()
        {
            _sceneLoader.LoadAsync(SceneLoader.Gameplay, _ctx.Token).Forget();
        }

        public void Exit()
        {
        }

        public void Dispose()
        {
            _ctx.Cancel();
            _ctx.Dispose();
        }
    }
}