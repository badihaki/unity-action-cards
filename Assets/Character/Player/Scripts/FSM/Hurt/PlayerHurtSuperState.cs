using UnityEngine;

public class PlayerHurtSuperState : PlayerState
{
	protected PlayerMovement moveController { get; private set; }

	public PlayerHurtSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
		moveController = pc._MoveController;
	}
	public override void InitializeState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine)
	{
		base.InitializeState(pc, animationName, stateMachine);
		moveController = pc._MoveController;
	}
	public override void ExitState()
	{
		base.ExitState();
		moveController.ResetPushback();
	}

	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();

		if(Time.time > _StateEnterTime + 0.85f)
		{
			_StateMachine.ChangeState(_StateMachine._IdleState);
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		_PlayerCharacter._CameraController.MakeCameraFollowPlayerActor();
	}

	public override void AnimationFinished()
	{
		base.AnimationFinished();
		_StateMachine.ChangeState(_StateMachine._IdleState);
	}
}
