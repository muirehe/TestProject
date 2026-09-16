using System;
using Infrastructure.SaveSystem;
using Meta;
using Presentation;
using Presentation.Windows;
using UnityEngine;

namespace Infrastructure.GameStateMachine.States
{
    public class ResultsState : IState
    {
        private readonly SaveService _saveService;
        private readonly GameSession _gameSession;
        private readonly ResourceModule _resourceModule;
        private readonly WindowManager _windowManager;

        public ResultsState(SaveService saveService, GameSession gameSession, ResourceModule resourceModule,
            WindowManager windowManager)
        {
            _saveService = saveService;
            _gameSession = gameSession;
            _resourceModule = resourceModule;
            _windowManager = windowManager;
        }

        public void Enter()
        {
            (_gameSession.BattleStats.RewardCoins, _gameSession.BattleStats.RewardExp) =
                _resourceModule.AddResourcesFromBattle(_gameSession.BattleStats, _gameSession.IsVictory);
            try
            {
                _saveService.SaveAll();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            _windowManager.Show<ResultsWindow>();
        }

        public void Exit()
        {
            _windowManager.Hide<ResultsWindow>();
        }
    }
}