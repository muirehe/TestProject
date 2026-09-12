using Infrastructure.GameStateMachine;
using Infrastructure.GameStateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Presentation.Menu
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Button playButton;

        [Inject] private IGameStateMachine _gameStateMachine;

        private void Start()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            //playButton.interactable = false;
            _gameStateMachine.Enter<GameplayState>();
        }
    }
}