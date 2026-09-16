using Configs;
using Infrastructure.GameStateMachine;
using Infrastructure.GameStateMachine.States;
using Meta;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Presentation.Menu
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private LevelConfig levelConfig;
        [SerializeField] private TextMeshProUGUI coinsCountText, expCountText;

        [Inject] private ResourceModule _resourceModule;
        [Inject] private IGameStateMachine _gameStateMachine;
        [Inject] private GameSession _gameSession;

        private void Start()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            SetupResources();
        }

        private void SetupResources()
        {
            coinsCountText.text = _resourceModule.Data.Coins.ToString();
            expCountText.text = _resourceModule.Data.Experience.ToString();
        }
        
        private void OnPlayButtonClicked()
        {
            playButton.interactable = false;
            _gameSession.LevelConfig = levelConfig;
            _gameStateMachine.Enter<GameplayState>();
        }
    }
}