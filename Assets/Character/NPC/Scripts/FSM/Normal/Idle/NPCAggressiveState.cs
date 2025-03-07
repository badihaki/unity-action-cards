using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Idle/Aggressive", fileName = "Aggressive Idle")]
public class NPCAggressiveState : NPCIdleState
{
    private float distanceFromTarget;
    private NPCAttackController attackController;
	[SerializeField]
	protected bool readyToAttack;

	public override void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
	{
		base.InitState(npc, stateMachine, animationName);

        attackController = npc._AttackController as NPCAttackController;
	}

	public override void EnterState()
    {
		if (waitTime <= 0)
		{
			waitTime = CreateNewWaitTime(0.55f, 1.0f);
		}
		readyToAttack = false;
        base.EnterState();
    }

    public override void LogicUpdate()
    {
		if (!_IsExitingState)
		{
			CheckStateTransitions();
		}
		if (waitTime > 0)
			RunWaitTimer();
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

		if (attackController._ActiveTarget != null)
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
			//_StateMachine.LogFromState($"entity {_NPC.name} is moving towards target. Distance from target exceeded {distanceFromTarget}");
            _StateMachine.ChangeState(_StateMachine._StateLibrary._ChaseState);
        }
        if (readyToAttack)
        {
			if (attackController._ActiveTarget != null)
			{
				_StateMachine.ChangeState(_NPC._MoveSet.GetCurrentAttackState());
				return;
			}
			_StateMachine.ChangeState(_StateMachine._StateLibrary._IdleState);
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
        if (waitTime <= 0)
		{
			if (GameManagerMaster.GameMaster.GMSettings.logNPCUtilData)
				_StateMachine.LogFromState($"done with wait");
			readyToAttack = true;
		}
    }

    private float CreateNewWaitTime(float min, float max)
    {
        return GameManagerMaster.GameMaster.Dice.RollRandomDice(min, max);
    }
}
