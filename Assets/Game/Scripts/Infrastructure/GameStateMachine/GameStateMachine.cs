using Infrastructure.GameStateMachine.States;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.GameStateMachine
{
    public interface IGameStateMachine
    {
        public void Enter<TState>() where TState : IState;
    }

    public class GameStateMachine : IGameStateMachine, IInitializable
    {
        private IState _current;

        private readonly IObjectResolver _objectResolver;

        public GameStateMachine(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public void Initialize()
        {
            Enter<BootstrapState>();
        }

        public void Enter<TState>() where TState : IState
        {
            var state = Switch<TState>();
            state.Enter();
        }

        private TState Switch<TState>() where TState : IState
        {
            _current?.Exit();
            var state = _objectResolver.Resolve<TState>();
            _current = state;
            return state;
        }
    }
}