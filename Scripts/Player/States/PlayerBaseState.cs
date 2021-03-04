using Utilities;

public abstract class PlayerBaseState<T> : BaseState<T>, IStateUpdate<T>, IStateInput<T>
{
    public virtual void UpdateState(T owner) {}
    public virtual void HandleInput(T owner) {}
}
