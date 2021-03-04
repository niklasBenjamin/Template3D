using Godot;
using InputSystem;

namespace Player.States
{
    public abstract class MoveState : PlayerBaseState<PlayerController> {

        protected Vector3 input;

        public override void UpdateState(PlayerController owner) {
            var forward = owner.GlobalTransform.basis;
            input = Vector3.Zero;

            input -= InputManager.Instance.MoveVertical() * forward.z;
            input -= InputManager.Instance.MoveHorizontal() * forward.x;
        }
    }
}