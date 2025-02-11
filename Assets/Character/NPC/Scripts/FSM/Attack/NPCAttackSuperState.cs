using UnityEngine;

public class NPCAttackSuperState : NPCState
{
	protected NPCAttackController _AttackController;
	
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
		if (GameManagerMaster.GameMaster.GMSettings.logExtraNPCData)
			Debug.Log(_NPC.name + " is entering state " + _NPC._MoveSet.GetCurrentAttack().animationName);

		//_AttackController.SetAttackParameters(false, false);
		_AttackController.SetAttackParameters();
		_NPC._MoveController.ZeroOutMovement();
		_AttackController.UseAttackTicket();
	}

	public override void ExitState()
	{
		_IsExitingState = true;
		_NPC._AnimationController.SetBool(_NPC._MoveSet.GetCurrentAttack().animationName, false);

		_AttackController.EndAttackAddWait(_NPC._MoveSet.GetCurrentAttack().waitTime);
		_AttackController.ResetAttackParameters();

		_NPC._MoveSet.SetAttackIndexRandomly();
	}

	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();

		if (_AnimationIsFinished)
		{
			_StateMachine.LogFromState($"going from attack state {name} to state {_StateMachine._StateLibrary._IdleAggressiveState.name}");
			_StateMachine.ChangeState(_StateMachine._StateLibrary._IdleAggressiveState);
		}
	}
	// end
}
