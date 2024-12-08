using UnityEngine;

public class PlayerHurtSuperState : PlayerState
{
	public PlayerHurtSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}

	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();

		if(Time.time > _StateEnterTime + 0.85f)
		{
			_StateMachine.ChangeState(_StateMachine._IdleState);
		}
	}

	public override void AnimationFinished()
	{
		base.AnimationFinished();
		_StateMachine.ChangeState(_StateMachine._IdleState);
	}
}
