using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHurtSuperState : NPCState
{
	public override void EnterState()
	{
		base.EnterState();

        _NPC._NavigationController.StopNavigation();
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

        _NPC._MoveController.ApplyExternalForces();
	}

	public override void AnimationEndTrigger()
    {
        base.AnimationEndTrigger();

        if (!_NPC._AttackController._IsAggressive)
        {
            _StateMachine.ChangeState(_NPC._IdleState);
        }
        else
        {
            _StateMachine.ChangeState(_NPC._IdleAggressiveState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        _NPC._MoveController.SetExternalForces(_NPC.transform, 0.0f, 0.0f);
    }
}
