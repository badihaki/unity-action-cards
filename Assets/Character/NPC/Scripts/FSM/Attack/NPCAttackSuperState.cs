using UnityEngine;

public class NPCAttackSuperState : NPCState
{
	public NPCAttackSuperState()
	{
	}

	public override void EnterState()
	{
		_AnimationIsFinished = false;
		_IsExitingState = false;
		_StateEnterTime = Time.time;
		
		_NPC._AnimationController.SetBool(_NPC._MoveSet.GetCurrentAttack().animationName, true);
		
		if (GameManagerMaster.GameMaster.logExtraNPCData)
			Debug.Log(_NPC.name + " is entering state " + _NPC._MoveSet.GetCurrentAttack().animationName);

		_NPC._AttackController.UseAttackTicket();
	}

	public override void ExitState()
	{
		_IsExitingState = true;
		_NPC._AnimationController.SetBool(_NPC._MoveSet.GetCurrentAttack().animationName, false);

		_NPC._AttackController.EndAttackAddWait(_NPC._MoveSet.GetCurrentAttack().waitTime);
	}

	public override void AnimationEndTrigger()// you can safely completely override this method
	{
		base.AnimationEndTrigger();

		_StateMachine.ChangeState(_StateMachine._IdleAggressiveState);
	}
	// end
}
