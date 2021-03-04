using InputSystem;

namespace Player.States
{
    public class IdleState : PlayerBaseState<PlayerController> {

        public override void HandleInput(PlayerController owner) {
            if(InputManager.Instance.MoveVertical() != 0 || InputManager.Instance.MoveHorizontal() != 0) {
                owner.PushState(PlayerState.WalkState);
            }
            else if(InputManager.Instance.Jump()) {
                owner.PushState(PlayerState.JumpState);
            }
        }

        public override void UpdateState(PlayerController owner) {
            if(!owner.IsOnFloor()) {
                owner.PushState(PlayerState.FallState);
            }
        }
    }
}