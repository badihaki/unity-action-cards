using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCMoveState : NPCState
{
    private float distanceFromPlayer;

    public override void EnterState()
    {
        base.EnterState();
        //_StateMachine.LogFromState("");
    }

	public override void ExitState()
	{
		base.ExitState();
	}

	public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();
        if (_NPC._AttackController._IsAggressive)
        {
            if (distanceFromPlayer < _NPC._NavigationController._MaxDistance) _StateMachine.ChangeState(_NPC._IdleAggressiveState);
        }

        _NPC._MoveController.MoveToCurrentNavNode();

        //if (Math.Round(_NPC.transform.position.x, 0) == Math.Round(_NPC._NavigationController._TargetLocation.x, 0) && Math.Round(_NPC.transform.position.z) == Math.Round(_NPC._NavigationController._TargetLocation.z))
        //if (Vector3.Distance(_NPC._NPCActor.transform.position, _NPC._NavigationController._TargetLocation) < 0.5f)
        //{
        //    if (_NPC._AttackController._IsAggressive) _StateMachine.ChangeState(_NPC._IdleAggressiveState);
        //    else _StateMachine.ChangeState(_NPC._IdleState);
        //}
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	// end of the line
}
