using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Battle;
using Infrastructure.Input;
using Infrastructure.SceneSystem;
using MessagePipe;
using Meta;
using Presentation;
using Presentation.Windows;
using UnityEngine;

namespace Infrastructure.GameStateMachine.States
{
    public class GameplayState : IState, IDisposable
    {
        private IDisposable _disposable;
        private CancellationTokenSource _ctx;
        private readonly WindowManager _windowManager;
        private readonly SceneLoader _sceneLoader;
        private readonly GameInput _gameInput;
        private readonly GameSession _gameSession;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISubscriber<BattleEnded> _battleEndedSubscriber;

        public GameplayState(WindowManager windowManager, SceneLoader sceneLoader, GameInput gameInput,
            IGameStateMachine gameStateMachine, ISubscriber<BattleEnded> battleEndedSubscriber, GameSession gameSession)
        {
            _sceneLoader = sceneLoader;
            _gameInput = gameInput;
            _windowManager = windowManager;
            _gameStateMachine = gameStateMachine;
            _battleEndedSubscriber = battleEndedSubscriber;
            _gameSession = gameSession;
        }

        public void Enter()
        {
            _ctx = new CancellationTokenSource();
            _disposable = DisposableBag.Create(_battleEndedSubscriber.Subscribe(OnBattleEnded));
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

        private void OnBattleEnded(BattleEnded e)
        {
            _gameSession.IsVictory = e.IsVictory;
            _gameStateMachine.Enter<ResultsState>();
        }

        public void Exit()
        {
            _ctx?.Cancel();
            _ctx?.Dispose();
            _ctx = null;
            _windowManager.Hide<LoadingWindow>();
            _gameInput.Player.Disable();
            Cursor.lockState = CursorLockMode.None;
            _disposable?.Dispose();
        }

        public void Dispose()
        {
            _gameInput.Player.Disable();
            _ctx?.Cancel();
            _ctx?.Dispose();
            _ctx = null;
            _disposable?.Dispose();
        }
    }
}