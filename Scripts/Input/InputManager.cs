using Godot;
using InputSystem.InputDevices;

namespace InputSystem 
{
    public class InputManager : Singleton<InputManager>
    {
        public bool UsingGamepad { get; private set; }

        public delegate void OnGamepadChanged(bool usingGamepad);
        public event OnGamepadChanged OnGamepadChangedEvent;

        private IInputDevice currentDevice;
        private IInputDevice keyboardDevice = new KeyboardDevice();
        private IInputDevice gamepadDevice = new GamepadDevice();

        public override void _Ready() {
            SetDevice(false);
        }

        public override void _Input(InputEvent inputEvent) {
            if(inputEvent is InputEventKey key && UsingGamepad) {
                SetDevice(false);
            }
            else if(inputEvent is InputEventJoypadButton button && !UsingGamepad)
                SetDevice(true);
        }

        private void SetDevice(bool usingGamepad) {
            UsingGamepad = usingGamepad;
            currentDevice = usingGamepad ? gamepadDevice : keyboardDevice;

            if(OnGamepadChangedEvent != null)
                OnGamepadChangedEvent(usingGamepad);
        }

        public float MoveVertical() {
            return currentDevice.MoveVertical();
        }

        public float MoveHorizontal() {
            return currentDevice.MoveHorizontal();
        }

        public bool Jump() {
            return currentDevice.Jump();
        }

        public bool Run() {
            return currentDevice.Run();
        }
        
        public bool Escape() {
            return currentDevice.Escape();
        }
    }
}

