using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Movement/Base", fileName = "Base Move")]
public class NPCMoveState : NPCState
{
    private float distanceFromPlayer;
    protected NPCAttackController _AttackController;

	public override void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
	{
		base.InitState(npc, stateMachine, animationName);
        _AttackController = npc._AttackController as NPCAttackController;
	}

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
        
        if (_NPC._NPCActor._AggressionManager.isAggressive && _AttackController._ActiveTarget != null)
        {
            // if aggressive
            //_StateMachine.LogFromState($"player-to-target distance -> {distanceFromPlayer.ToString()}");
            if (distanceFromPlayer < _NPC._MoveSet.GetCurrentAttack().desiredDistance)
                _StateMachine.ChangeState(_StateMachine._IdleAggressiveState);
        }
        else if (!_NPC._NPCActor._AggressionManager.isAggressive)
        {
            // not aggressive
            // check distance between current node from nav controller and actor
            if (Vector3.Distance(_NPC._NPCActor.transform.position, _NPC._NavigationController._CurrentNavNode.transform.position) <= _NPC._NavigationController._MaxDistance)
            {
                _StateMachine.ChangeState(_StateMachine._IdleState);
            }
        }
    }

    public override void LogicUpdate()
    {
        if (_NPC._NPCActor._AggressionManager.isAggressive && _AttackController._ActiveTarget != null)
        {
            distanceFromPlayer = _AttackController.GetDistanceFromTarget();
        }
        else
        {
            distanceFromPlayer = 1.75f;
        }
        base.LogicUpdate();
    }

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

        if(_NPC._NPCActor._AggressionManager.isAggressive)
        {
            _NPC._MoveController.SetAgentDestination(_AttackController._ActiveTarget.position);
			_NPC._MoveController.MoveToTarget();
            _NPC._MoveController.RotateTowardsTarget(_AttackController._ActiveTarget);
        }
        else
        {
            _NPC._MoveController.MoveToCurrentNavNode();
            _NPC._MoveController.RotateTowardsTarget(_NPC._NavigationController._CurrentNavNode.transform);
        }
	}

	// end of the line
}
