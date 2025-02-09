using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Idle/Aggressive", fileName = "Aggressive Idle")]
public class NPCAggressiveState : NPCIdleState
{
    private float distanceFromTarget;
    private NPCAttackController attackController;

	public override void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
	{
		base.InitState(npc, stateMachine, animationName);

        attackController = npc._AttackController as NPCAttackController;
	}

	public override void EnterState()
    {
        if (waitTime <= 0)
            waitTime = CreateNewWait(0.2f, 1.0f);
        base.EnterState();
    }

    public override void LogicUpdate()
    {
		if (!_IsExitingState)
		{
			CheckStateTransitions();
		}
		if (waitTime > 0) RunWaitTimer();
        else waitTime = 0;
        // distanceFromPlayer = Vector3.Distance(_NPC.transform.position, _NPC._NavigationController._Target.position);
		if (_NPC._NPCActor._AggressionManager.isAggressive && attackController._ActiveTarget != null)
		{
			distanceFromTarget = attackController.GetDistanceFromTarget();
		}
		else
		{
			distanceFromTarget = 1.75f;
		}
    }

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		_NPC._MoveController.RotateTowardsTarget(attackController._ActiveTarget);
	}

	public override void CheckStateTransitions()
    {
        //base.CheckStateTransitions();
		// if not on ground
		if (!_NPC._CheckGrounded.IsGrounded())
		{
			_StateMachine.ChangeState(_StateMachine._StateLibrary._FallingState);
		}
		if (distanceFromTarget > _NPC._MoveSet.GetCurrentAttack().maxinumDistance)
        {
			_StateMachine.LogFromState($"entity {_NPC.name} is moving towards target. Distance from target exceeded {distanceFromTarget}");
            _StateMachine.ChangeState(_StateMachine._StateLibrary._ChaseState);
        }
        if (waitTime <= 0)
        {
			_StateMachine.LogFromState($"going to action {_NPC._MoveSet.GetCurrentAttackState().name} from state {name}");
			waitTime = CreateNewWait(1.0f, _NPC._MoveSet.GetCurrentAttack().waitTime * 2.15f);
			_StateMachine.ChangeState(_NPC._MoveSet.GetCurrentAttackState());
        }
		// if aggressive
		if (!_NPC._NPCActor._AggressionManager.isAggressive)
		{
			_StateMachine.ChangeState(_StateMachine._StateLibrary._IdleState);
		}
	}

    private void RunWaitTimer()
    {
        waitTime -= Time.deltaTime;
        //if (waitTime <= 0) _StateMachine.LogFromState($"done with wait");
    }

    private float CreateNewWait(float min, float max)
    {
        return GameManagerMaster.GameMaster.Dice.RollRandomDice(min, max);
    }
}
