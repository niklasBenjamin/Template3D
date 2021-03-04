using Godot;

namespace Player.States
{
    public class FallState : MoveState {
        
        public override void OnStateExit(PlayerController owner) {
            owner.VerticalVelocity = Vector3.Down;
        }

        public override void UpdateState(PlayerController owner) {
            base.UpdateState(owner);
            owner.Velocity = input.Normalized() * owner.WalkSpeed;

            if(owner.IsOnFloor()) {
                owner.PopState();
            }
            owner.VerticalVelocity += Vector3.Down * owner.Gravity * 0.016f;
        }
    }
}