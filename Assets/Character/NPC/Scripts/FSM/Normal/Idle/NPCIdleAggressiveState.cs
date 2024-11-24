using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleAggressiveState : NPCState
{
    private float waitTime;
    private float distanceFromPlayer;
    public override void EnterState()
    {
        waitTime = CreateNewWait(1.2f, 3.2f);
        base.EnterState();
    }

    public override void LogicUpdate()
    {
        // distanceFromPlayer = Vector3.Distance(_NPC.transform.position, _NPC._NavigationController._Target.position);
		if (_NPC._NPCActor._AggressionManager.isAggressive && _NPC._AttackController._ActiveTarget != null)
		{
			distanceFromPlayer = _NPC._AttackController.GetDistanceFromTarget();
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

		_NPC._MoveController.RotateTowardsTarget(_NPC._AttackController._ActiveTarget);
	}

	public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();
        if (distanceFromPlayer > _NPC._MoveSet.GetCurrentAttack().maxinumDistance)
        {
            // Debug.Log($"entity {_NPC.name} is moving towards target. Distance from target exceeded {distanceFromPlayer}");
            _StateMachine.ChangeState(_StateMachine._MoveState);
        }
        if (waitTime <= 0)
        {
            _StateMachine.ChangeState(_NPC._MoveSet.GetCurrentAttackState());
        }
    }

    private void RunWaitTimer()
    {
        waitTime -= Time.deltaTime;
        if (waitTime <= 0) _StateMachine.LogFromState($"done with wait");
    }

    private float CreateNewWait(float min, float max)
    {
        return GameManagerMaster.GameMaster.Dice.RollRandomDice(min, max);
    }
}
