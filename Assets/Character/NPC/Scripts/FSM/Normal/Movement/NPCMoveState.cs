using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Movement/Base", fileName = "Base Move")]
public class NPCMoveState : NPCState
{
	public override void EnterState()
    {
        base.EnterState();
    }

	public override void ExitState()
	{
		base.ExitState();
        _NPC._NavigationController.AddToPriorNodes();
	}

	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();
		if (_NPC._NavigationController._Target == null)
		{
			if (_NPC._NavigationController._CurrentNavNode != null)
			{
				if (Vector3.Distance(_NPC._NPCActor.transform.position, _NPC._NavigationController._CurrentNavNode.transform.position) <= _NPC._NavigationController._MaxDistance)

				{
					_StateMachine.ChangeState(_StateMachine._StateLibrary._IdleState);
				}
			}
		}
    }

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		if (_NPC._NavigationController._Target == null) // there is no target
		{
			if (_NPC._NavigationController._CurrentNavNode != null)
			{
				MoveToNavNode();
			}
		}
		else
		{
			MoveToTarget();
		}
		// if aggressive
		if (_NPC._NPCActor._AggressionManager.isAggressive)
		{
			_StateMachine.ChangeState(_StateMachine._StateLibrary._IdleAggressiveState);
		}
	}

	protected void MoveToTarget()
	{
		_NPC._MoveController.MoveToNavmeshDestination();
		_NPC._MoveController.RotateTowardsTarget(_NPC._NavigationController._Target);
	}

	protected void MoveToNavNode()
	{
		_NPC._MoveController.MoveToNavmeshDestination();
		_NPC._MoveController.RotateTowardsTarget(_NPC._NavigationController._CurrentNavNode.transform);
	}

	// end of the line
}
