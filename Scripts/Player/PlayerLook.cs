using Godot;

namespace Player
{
    public class PlayerLook : Camera
    {
		private KinematicBody player;
        [Export] public float Sensitivity   { get; private set; }
        

        public override void _Ready() {
            player = GetNode<KinematicBody>("../../Player");
        }
        
        public override void _UnhandledInput(InputEvent @event) {
			if(@event is InputEventMouseMotion mouseMotion)
				MouseLook(mouseMotion.Relative);

			@event.Dispose();
		}

        private void MouseLook(Vector2 mouseDelta) {
            float xDelta = -Sensitivity * mouseDelta.x;
            float yDelta = -Sensitivity * mouseDelta.y;

            if(xDelta == 0 && yDelta == 0)
                return;

            RotateX(Mathf.Deg2Rad(yDelta));
            player.RotateY(Mathf.Deg2Rad(xDelta));
            RotationDegrees = new Vector3(Mathf.Clamp(RotationDegrees.x, -90, 90), 0, 0);
        }
    }
}

