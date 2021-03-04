namespace Player.States
{
    public class JumpState : PlayerBaseState<PlayerController> {

        public override void OnStateEnter(PlayerController owner) {
            owner.VerticalVelocity = owner.GlobalTransform.basis.y * owner.JumpForce;
            owner.PopState();
        }
    }
}