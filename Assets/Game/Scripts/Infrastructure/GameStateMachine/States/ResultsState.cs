using Presentation;
using Presentation.Windows;

namespace Infrastructure.GameStateMachine.States
{
    public class ResultsState : IState
    {
        private readonly WindowManager _windowManager;

        public ResultsState(WindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public void Enter()
        {
            _windowManager.Show<ResultsWindow>();
        }

        public void Exit()
        {
            _windowManager.Hide<ResultsWindow>();
        }
    }
}