using Godot;
using Utilities;
using System.Collections.Generic;
using Player.States;
using InputSystem;

namespace Player
{
	public enum PlayerState {
		IdleState,
		WalkState,
		RunState,
		FallState,
		JumpState
	}

	//3D character controller that works with a pushdown automata state machine
	public class PlayerController : KinematicBody 
	{
		[Export] public float WalkSpeed     { get; private set; }
		[Export] public float RunSpeed      { get; private set; }
		[Export] public float JumpForce     { get; private set; }
		[Export] public float Gravity       { get; private set; }
		[Export] public float Acceleration  { get; private set; }
		
		public Vector3 Velocity				{ get; set; }
		public Vector3 VerticalVelocity 	{ get; set; }
		public Vector3 TotalVelocity		{ get; private set;}

		private StateMachine<PlayerController, PlayerBaseState<PlayerController>> stateMachine;
		private Stack<PlayerBaseState<PlayerController>> stateStack;
		private Dictionary<PlayerState, PlayerBaseState<PlayerController>> states;

		//Initialize state machine with player states
		public override void _Ready() {
			stateMachine = new StateMachine<PlayerController, PlayerBaseState<PlayerController>>(this);
			stateStack = new Stack<PlayerBaseState<PlayerController>>();

			states = new Dictionary<PlayerState, PlayerBaseState<PlayerController>>() {
				{PlayerState.IdleState, new IdleState()},
				{PlayerState.WalkState, new WalkState()},
				{PlayerState.JumpState, new JumpState()},
				{PlayerState.FallState, new FallState()},
				{PlayerState.RunState,  new RunState()},
			};

			PushState(PlayerState.IdleState);
			Input.SetMouseMode(Input.MouseMode.Captured);
		}

		//Process character movement every physics step
		public override void _PhysicsProcess(float delta) {
			TotalVelocity = MoveAndSlide(TotalVelocity, Vector3.Up);
		}

		//Update velocity and current state
		public override void _Process(float delta) {
			if(InputManager.Instance.Escape()) {
				Input.SetMouseMode(Input.MouseMode.Visible);
			}

			TotalVelocity = TotalVelocity.LinearInterpolate(Velocity + VerticalVelocity, Acceleration * delta);
			stateMachine.GetCurrentState()?.UpdateState(this);
		}

		//Call input event in current state
		public override void _UnhandledInput(InputEvent @event) {
			stateMachine.GetCurrentState()?.HandleInput(this);

			@event.Dispose();
		}

		//Push new state to stack
		public void PushState(PlayerState state) {
			stateStack.Push(states[state]);
			stateMachine.ChangeState(states[state]);
		}

		//Pop state from stack
		public void PopState() {
			if(stateStack.Count > 1) {
				stateStack.Pop();
				stateMachine.ChangeState(stateStack.Peek());
			}
		}
	}
}
