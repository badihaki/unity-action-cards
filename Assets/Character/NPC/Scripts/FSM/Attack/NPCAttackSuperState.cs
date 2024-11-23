using UnityEngine;

public class NPCAttackSuperState : NPCState
{
	public NPCAttackSuperState()
	{
	}

	public override void AnimationEndTrigger()// you can safely completely override this method
	{
		base.AnimationEndTrigger();

		_StateMachine.ChangeState(_StateMachine._IdleAggressiveState);
	}
}
