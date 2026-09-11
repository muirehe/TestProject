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

        private IState Switch<TState>() where TState : IState
        {
            _current?.Exit();
            _current = _objectResolver.Resolve<TState>();
            return _current;
        }
    }
}