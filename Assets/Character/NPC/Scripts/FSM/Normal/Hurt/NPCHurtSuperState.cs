using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHurtSuperState : NPCState
{
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	public override void EnterState()
	{
		base.EnterState();

        _NPC._MoveController.SetAgentUpdates(false);
	}

	public override void ExitState()
    {
        base.ExitState();

        _NPC._MoveController.SetExternalForces(_NPC.transform, 0.0f, 0.0f);
        _NPC._MoveController.SetAgentUpdates(true);
	}

	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();

		if (_AnimationIsFinished)
		{
			if (_NPC._CheckGrounded.IsGrounded())
			{
				if (!_NPC._NPCActor._AggressionManager.isAggressive)
				{
					_StateMachine.ChangeState(_StateMachine._StateLibrary._IdleState);
				}
				else
				{
					_StateMachine.ChangeState(_StateMachine._StateLibrary._IdleAggressiveState);
				}
			}
			else
			{
				_StateMachine.ChangeState(_StateMachine._StateLibrary._FallingState);
			}
		}
	}
}
