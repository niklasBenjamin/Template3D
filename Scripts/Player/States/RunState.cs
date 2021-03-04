using InputSystem;

namespace Player.States 
{
    public class RunState : MoveState
    {
        public override void HandleInput(PlayerController owner) {
            if(InputManager.Instance.Jump()) {
                owner.PushState(PlayerState.JumpState);
            }
        }

        public override void UpdateState(PlayerController owner) {
            base.UpdateState(owner);
            owner.Velocity = input.Normalized() * owner.RunSpeed;

            if(input.LengthSquared() < 0.5f)
                owner.PopState();

            if(!owner.IsOnFloor()) {
                owner.PushState(PlayerState.FallState);
            }
        }
    }
}
