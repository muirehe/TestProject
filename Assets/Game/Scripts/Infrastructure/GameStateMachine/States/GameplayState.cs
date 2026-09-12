using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Input;
using Infrastructure.SceneSystem;
using UnityEngine;

namespace Infrastructure.GameStateMachine.States
{
    public class GameplayState : IState, IDisposable
    {
        private CancellationTokenSource _ctx = new();
        private readonly SceneLoader _sceneLoader;
        private readonly GameInput _gameInput;
        
        public GameplayState(SceneLoader sceneLoader, GameInput gameInput)
        {
            _sceneLoader = sceneLoader;
            _gameInput = gameInput;
        }
        
        public void Enter()
        {
            _sceneLoader.LoadAsync(SceneLoader.Gameplay, _ctx.Token).Forget();
            _gameInput.Player.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Exit()
        {
            _gameInput.Player.Disable();
            Cursor.lockState = CursorLockMode.None;
        }

        public void Dispose()
        {
            _ctx.Cancel();
            _ctx.Dispose();
        }
    }
}