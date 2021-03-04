namespace Utilities
{
    public class StateMachine<T, U> where U : IState<T>
    {
        private T owner;
        private U currentState;
      
        public StateMachine(T owner) {
            this.owner = owner;
            currentState = default(U);
        }

        public U GetCurrentState() {
            return currentState;
        }

        public void ChangeState(U newState) {
            currentState?.OnStateExit(owner);
            currentState = newState;
            currentState?.OnStateEnter(owner);    
        }
    }
}
