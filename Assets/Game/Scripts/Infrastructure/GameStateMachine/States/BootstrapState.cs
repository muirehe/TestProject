using Infrastructure.SaveSystem;

namespace Infrastructure.GameStateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly SaveService _saveService;
        private readonly IGameStateMachine _gameStateMachine;
        
        public BootstrapState(IGameStateMachine gameStateMachine, SaveService saveService)       
        {
            _gameStateMachine = gameStateMachine;
            _saveService = saveService;
        }
        
        public void Enter()
        {
            _saveService.LoadAll();
            _gameStateMachine.Enter<MenuState>();
        }

        public void Exit()
        {
        }
    }
}