using InputSystem;

namespace Player.States
{
    public class WalkState : MoveState {
        
        public override void HandleInput(PlayerController owner) {
            if(InputManager.Instance.Jump()) {
                owner.PushState(PlayerState.JumpState);
            }
            else if (InputManager.Instance.Run()) {
                owner.PushState(PlayerState.RunState);
            }
        }

        public override void UpdateState(PlayerController owner) {
            base.UpdateState(owner);
            owner.Velocity = input.Normalized() * owner.WalkSpeed;

            if(input.LengthSquared() == 0)
                owner.PopState();

            if(!owner.IsOnFloor()) {
                owner.PushState(PlayerState.FallState);
            }
        }
    }
}