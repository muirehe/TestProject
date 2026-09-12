using Infrastructure.GameStateMachine;
using Infrastructure.GameStateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Presentation.Windows
{
    public class ResultsWindow : WindowBase
    {
        [SerializeField] private Button toMenuButton;

        [Inject] private IGameStateMachine _gameStateMachine;
        
        private void Start()
        {
            toMenuButton.onClick.AddListener(OnToMenuButtonClicked);
        }

        private void OnToMenuButtonClicked()
        {
            _gameStateMachine.Enter<MenuState>();
        }

        public override void Show(params object[] args)
        {
        }

        public override void Hide()
        {
        }
    }
}
