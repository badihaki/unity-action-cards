using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Idle/Aggressive", fileName = "Aggressive Idle")]
public class NPCIdleAggressiveState : NPCIdleState
{
    private float distanceFromPlayer;
    private NPCAttackController attackController;

	public override void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
	{
		base.InitState(npc, stateMachine, animationName);

        attackController = npc._AttackController as NPCAttackController;
	}

	public override void EnterState()
    {
        waitTime = CreateNewWait(0.2f, _NPC._MoveSet.GetCurrentAttack().waitTime * 2.15f);
        base.EnterState();
    }

    public override void LogicUpdate()
    {
        // distanceFromPlayer = Vector3.Distance(_NPC.transform.position, _NPC._NavigationController._Target.position);
		if (_NPC._NPCActor._AggressionManager.isAggressive && attackController._ActiveTarget != null)
		{
			distanceFromPlayer = attackController.GetDistanceFromTarget();
		}
		else
		{
			distanceFromPlayer = 1.75f;
		}
		base.LogicUpdate();

        if (waitTime > 0) RunWaitTimer();
        else waitTime = 0;
        
    }

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		_NPC._MoveController.RotateTowardsTarget(attackController._ActiveTarget);
	}

	public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();
        if (distanceFromPlayer > _NPC._MoveSet.GetCurrentAttack().maxinumDistance)
        {
            // Debug.Log($"entity {_NPC.name} is moving towards target. Distance from target exceeded {distanceFromPlayer}");
            _StateMachine.ChangeState(_StateMachine._StateLibrary._MoveState);
        }
        if (waitTime <= 0)
        {
            _StateMachine.ChangeState(_NPC._MoveSet.GetCurrentAttackState());
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
