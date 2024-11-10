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
        _NPC._NavigationController.StartMoveToDestination();
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

        //if (Math.Round(_NPC.transform.position.x, 0) == Math.Round(_NPC._NavigationController._TargetLocation.x, 0) && Math.Round(_NPC.transform.position.z) == Math.Round(_NPC._NavigationController._TargetLocation.z))
        if (Vector3.Distance(_NPC._NPCActor.transform.position, _NPC._NavigationController._TargetLocation) < 0.5f)
        {
            if (_NPC._AttackController._IsAggressive) _StateMachine.ChangeState(_NPC._IdleAggressiveState);
            else _StateMachine.ChangeState(_NPC._IdleState);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (_NPC._AttackController._IsAggressive)
        {
            distanceFromPlayer = Vector3.Distance(_NPC._NPCActor.transform.position, _NPC._NavigationController._Target.position);

            // Debug.Log($"distance from target: {Vector3.Distance(_NPC._AttackController._MainTarget.position, _NPC.transform.position)}");
            if (Vector3.Distance(_NPC._AttackController._MainTarget.position, _NPC.transform.position) > _NPC._AttackController._DesiredAttackDistance && _NPC._NavigationController.IsNavStopped())
            {
                Debug.Log($"is {_NPC._CharacterSheet.name} moving?? {_NPC._NavigationController.IsNavStopped()}. OUT OF RANGE");
                // Debug.Log($"OUT OF RANGE TO TARGET::setting where to move from movestate");
                _NPC._NavigationController.StartMoveToDestination();
            }
        }
    }

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

        //_StateMachine.LogFromState("Matching velocities");
        //_NPC._NavigationController.MatchVelocityWithCharController();
	}

	// end of the line
}
