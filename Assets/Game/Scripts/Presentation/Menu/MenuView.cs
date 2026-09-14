using Configs;
using Infrastructure.GameStateMachine;
using Infrastructure.GameStateMachine.States;
using Meta;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Presentation.Menu
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private LevelConfig levelConfig;

        [Inject] private IGameStateMachine _gameStateMachine;
        [Inject] private GameSession _gameSession;

        private void Start()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            playButton.interactable = false;
            _gameSession.LevelConfig = levelConfig;
            _gameStateMachine.Enter<GameplayState>();
        }
    }
}