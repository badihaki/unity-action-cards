using UnityEngine;

public class NPCFallingState : NPCState
{
	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();

		if (_NPC._CheckGrounded.IsGrounded())
		{
			_StateMachine.LogFromState("are we grounded?");
			if (!_NPC._NPCActor._AggressionManager.isAggressive)
			{
				_StateMachine.LogFromState("yes, go to idle");
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
