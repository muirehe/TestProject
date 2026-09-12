using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Input;
using Infrastructure.SceneSystem;
using Presentation;
using Presentation.Windows;
using UnityEngine;

namespace Infrastructure.GameStateMachine.States
{
    public class GameplayState : IState, IDisposable
    {
        private CancellationTokenSource _ctx;
        private readonly WindowManager _windowManager;
        private readonly SceneLoader _sceneLoader;
        private readonly GameInput _gameInput;

        public GameplayState(WindowManager windowManager, SceneLoader sceneLoader, GameInput gameInput)
        {
            _sceneLoader = sceneLoader;
            _gameInput = gameInput;
            _windowManager = windowManager;
        }

        public void Enter()
        {
            _ctx = new CancellationTokenSource();
            EnterAsync(_ctx.Token).Forget();
        }

        private async UniTaskVoid EnterAsync(CancellationToken ctn)
        {
            var loadingWindow = _windowManager.Show<LoadingWindow>();
            var loaded = await _sceneLoader.LoadAsync(SceneLoader.Gameplay, loadingWindow, ctn);
            _windowManager.Hide<LoadingWindow>();
            if (!loaded) return;

            _gameInput.Player.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Exit()
        {
            _ctx?.Cancel();
            _ctx?.Dispose();
            _ctx = null;
            _windowManager.Hide<LoadingWindow>();
            _gameInput.Player.Disable();
            Cursor.lockState = CursorLockMode.None;
        }

        public void Dispose()
        {
            _gameInput.Player.Disable();
            _ctx?.Cancel();
            _ctx?.Dispose();
            _ctx = null;
        }
    }
}