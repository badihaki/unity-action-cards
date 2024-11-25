using UnityEngine;

public class NPCAttackSuperState : NPCState
{
	protected NPCAttackController _AttackController;
	public NPCAttackSuperState()
	{
	}
	public override void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
	{
		base.InitState(npc, stateMachine, animationName);
		_AttackController = npc._AttackController as NPCAttackController;
	}
	public override void EnterState()
	{
		_AnimationIsFinished = false;
		_IsExitingState = false;
		_StateEnterTime = Time.time;
		
		_NPC._AnimationController.SetBool(_NPC._MoveSet.GetCurrentAttack().animationName, true);
		
		if (GameManagerMaster.GameMaster.logExtraNPCData)
			Debug.Log(_NPC.name + " is entering state " + _NPC._MoveSet.GetCurrentAttack().animationName);

		_AttackController.SetAttackParameters(0, 0);
		_AttackController.UseAttackTicket();
	}

	public override void ExitState()
	{
		_IsExitingState = true;
		_NPC._AnimationController.SetBool(_NPC._MoveSet.GetCurrentAttack().animationName, false);

		_AttackController.EndAttackAddWait(_NPC._MoveSet.GetCurrentAttack().waitTime);
		_AttackController.ResetAttackParameters();
	}

	public override void AnimationEndTrigger()// you can safely completely override this method
	{
		base.AnimationEndTrigger();

		_StateMachine.ChangeState(_StateMachine._IdleAggressiveState);
	}
	// end
}
