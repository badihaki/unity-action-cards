using UnityEngine;

public class NPCAttackSuperState : NPCState
{
	public NPCAttackSuperState()
	{
	}

	public override void AnimationEndTrigger()
	{
		base.AnimationEndTrigger();

		_StateMachine.ChangeState(_StateMachine._IdleAggressiveState);
	}
}
