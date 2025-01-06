using UnityEngine;

public class NPCFallingState : NPCState
{
	public override void EnterState()
	{
		base.EnterState();

		_NPC._MoveController.SetAgentUpdates(false);
	}
	public override void ExitState()
	{
		base.ExitState();

		_NPC._MoveController.SetAgentUpdates(true);
	}
	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();
		
		if (_NPC._CheckGrounded.IsGrounded())
		{
			if (!_NPC._NPCActor._AggressionManager.isAggressive)
			{
				_StateMachine.ChangeState(_StateMachine._IdleState);
			}
			else
			{
				_StateMachine.ChangeState(_StateMachine._IdleAggressiveState);
			}
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		_NPC._MoveController.ApplyGravity();
		_NPC._MoveController.ApplyExternalForces();
	}
}
