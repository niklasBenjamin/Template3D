using Godot;

namespace InputSystem.InputDevices
{
    public class GamepadDevice : IInputDevice
    {
        public float MoveVertical() {
            return Input.GetActionStrength("move_forward") - Input.GetActionStrength("move_backward");
        }

        public float MoveHorizontal() {
            return Input.GetActionStrength("move_left") - Input.GetActionStrength("move_right");
        }

        public bool Jump() {
            return Input.IsActionJustPressed("jump");
        }

        public bool Run() {
            return Input.IsActionPressed("run");
        }

        public bool Escape() {
            return Input.IsActionJustPressed("escape");
        }
    }
}
