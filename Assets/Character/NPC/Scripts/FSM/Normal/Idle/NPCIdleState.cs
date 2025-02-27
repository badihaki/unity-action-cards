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
    protected float minimumWait = 0.1f;
    protected float minimumWaitMax = 1.0f;
    protected float maximumWaitMin = 1.2f;
    protected float maximumWait = 5.0f;
    protected bool readyToMove = false;

    public override void EnterState()
    {
        base.EnterState();

        if (waitTime <= 0)
            RandomlySetWaitTime(1.75f, 2.250f);
        _NPC._MoveController.ZeroOutMovement();
        _NPC._NavigationController.SetTargetDesiredDistance(0.5f);
        readyToMove = false;
	}

    private void RandomlySetWaitTime(float min, float max)
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

        if (!readyToMove && _NPC._NavigationController._NavTarget != null)
            readyToMove = true;
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
        int rollToMoveInstantly = GameManagerMaster.GameMaster.Dice.RollD6();
        if (rollToMoveInstantly >= 5)
        {
            FindPlaceToGo();
            return;
        }

        // actually make a wait time
        float minWait = GameManagerMaster.GameMaster.Dice.RollRandomDice(minimumWait, minimumWaitMax);
        float maxWait = GameManagerMaster.GameMaster.Dice.RollRandomDice(maximumWaitMin, maximumWait);
        RandomlySetWaitTime(minWait, maxWait);
    }

    private void FindPlaceToGo()
    {
        if (_NPC._NavigationController.TryFindNewNavNode() && _NPC._NavigationController._NavTarget == null)
        {
			readyToMove = true;
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

        // we are ready to move
        if (readyToMove)
        {
            _NPC._StateMachine.ChangeState(_StateMachine._StateLibrary._MoveState);
        }
	}
}
