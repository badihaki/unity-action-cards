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
        _NPC._NavigationController.AddToPriorNodes();
	}

	public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();
        
        if (_NPC._AttackController._IsAggressive)
        {
            if (distanceFromPlayer < _NPC._NavigationController._MaxDistance) _StateMachine.ChangeState(_NPC._IdleAggressiveState);
        }
        _StateMachine.LogFromState(Vector3.Distance(_NPC._NPCActor.transform.position, _NPC._NavigationController._CurrentNavNode.transform.position).ToString());
        if (Vector3.Distance(_NPC._NPCActor.transform.position, _NPC._NavigationController._CurrentNavNode.transform.position) <= 0.5f)
        {
            if (_NPC._AttackController._IsAggressive) _StateMachine.ChangeState(_NPC._IdleAggressiveState);
            else _StateMachine.ChangeState(_NPC._IdleState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
        _NPC._MoveController.MoveToCurrentNavNode();
        _NPC._MoveController.RotateTowardsTarget(_NPC._NavigationController._CurrentNavNode.transform);
	}

	// end of the line
}
