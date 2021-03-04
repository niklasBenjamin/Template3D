namespace Utilities
{
    public interface IState<T> {
        void OnStateEnter(T owner);
        void OnStateExit(T owner);
    }

    public interface IStateUpdate<T> {
        void UpdateState(T owner);
    }

    public interface IStateInput<T> {
        void HandleInput(T owner);
    }

    public abstract class BaseState<T> : IState<T> {   
        public virtual void OnStateEnter(T owner) {}
        public virtual void OnStateExit(T owner)  {}
    }
}
