using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Idle/Base", fileName = "Basic Idle")]
public class NPCIdleState : NPCState
{
    [SerializeField]
    protected float waitTime;

    public override void EnterState()
    {
        base.EnterState();

        CreateNewWait(2.5f, 7.0f);
        _NPC._MoveController.ZeroOutMovement();
        _NPC._NavigationController.SetTargetDesiredDistance(0.5f);
    }

    private void CreateNewWait(float min, float max)
    {
        // Debug.Log(_NPC.name + " is creating a new wait time: " + min + ", " + max);
        if (GameManagerMaster.GameMaster) waitTime = GameManagerMaster.GameMaster.Dice.RollRandomDice(min, max);
        else
        {
            Debug.LogError($"{_NPC.name} is trying to use the GameMaster, but there is no GameMaster. Wait time will be set to 1");
            waitTime = 1.0f;
        }
        // Debug.Log(_NPC.name + "'s new wait time: " + waitTime);
    }

    public override void LogicUpdate()
	{
		base.LogicUpdate();

		RunWaitTimer();
	}

	private void RunWaitTimer()
	{
		waitTime -= Time.deltaTime;
		if (waitTime <= 0)
		{
			waitTime = 0;
			FindPlaceToGo();
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

        _NPC._MoveController.ApplyGravity();
        _NPC._MoveController.ApplyExternalForces();
	}

	private void RollForWait()
    {
        int roll = GameManagerMaster.GameMaster.Dice.RollD6();
        // Debug.Log(_NPC.name + " is Rolling to wait. Need 5+ to ignore. Roll?: " + roll);
        if (roll >= 5)
        {
            FindPlaceToGo();
            return;
        }
        // Debug.Log(_NPC.name + " is making a new wait");
        float minWait = GameManagerMaster.GameMaster.Dice.RollRandomDice(0.1f, 1.0f);
        float maxWait = GameManagerMaster.GameMaster.Dice.RollRandomDice(1.2f, 5.0f);
        CreateNewWait(minWait, maxWait);
    }

    private void FindPlaceToGo()
    {
        //Debug.Log(_NPC.name + " is Finding a place to move to");
        if (_NPC._NavigationController.TryFindNewPatrol())
        {
			_NPC._StateMachine.ChangeState(_StateMachine._StateLibrary._MoveState);
		}
        else
        {
            if (GameManagerMaster.GameMaster.GMSettings.logNPCNavData)
                _StateMachine.LogFromState("no place to patrol to, rolling for wait");
            RollForWait();
        }
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        // if aggressive
        if (_NPC._NPCActor._AggressionManager.isAggressive)
        {
            _StateMachine.ChangeState(_StateMachine._StateLibrary._IdleAggressiveState);
        }

        // if not on ground
        if (!_NPC._CheckGrounded.IsGrounded())
        {
            _StateMachine.ChangeState(_StateMachine._StateLibrary._FallingState);
        }
	}
}
