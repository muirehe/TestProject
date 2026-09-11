namespace Infrastructure.GameStateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        
        public BootstrapState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }
        
        public void Enter()
        {
            _gameStateMachine.Enter<MenuState>();
        }

        public void Exit()
        {
        }
    }
}