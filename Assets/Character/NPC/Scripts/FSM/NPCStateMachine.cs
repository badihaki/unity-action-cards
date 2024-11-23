using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
	public NPCIdleState _IdleState { get; private set; }
	public NPCIdleAggressiveState _IdleAggressiveState { get; private set; }
	public NPCMoveState _MoveState { get; private set; }
    public NPCFallingState _FallingState { get; private set; }
	public NPCHurtSuperState _HurtState { get; private set; }

	[field: SerializeField]
    public NPCState _CurrentState { get; private set; }
    
    
    private bool _Ready = false;
    

    public void InitializeStateMachine(NonPlayerCharacter npc)
    {
        SetUpStateMachine(npc);
        _CurrentState = _IdleState;
        _CurrentState.EnterState();
        _Ready = true;
    }

	public virtual void SetUpStateMachine(NonPlayerCharacter npc)
	{
		if (!_IdleState)
		{
			_IdleState = NPCIdleState.CreateInstance<NPCIdleState>();
		}
		_IdleState.InitState(npc, this, "idle");

		if (!_IdleAggressiveState)
		{
			_IdleAggressiveState = NPCIdleAggressiveState.CreateInstance<NPCIdleAggressiveState>();
		}
		_IdleAggressiveState.InitState(npc, this, "idle");

		if (!_MoveState)
		{
			_MoveState = NPCMoveState.CreateInstance<NPCMoveState>();
		}
		_MoveState.InitState(npc, this, "move");

        if (!_FallingState)
        {
            _FallingState = NPCFallingState.CreateInstance<NPCFallingState>();
        }
        _FallingState.InitState(npc, this, "falling");

		if (!_HurtState)
		{
			_HurtState = NPCHurtSuperState.CreateInstance<NPCHurtSuperState>();
		}
		_HurtState.InitState(npc, this, "hurt");
	}

	public void ChangeState(NPCState state)
    {
        _CurrentState.ExitState();
        _CurrentState = state;
        _CurrentState.EnterState();
    }

    public void Update()
    {
        if (_Ready && GameManagerMaster.GameMaster)
        {
            _CurrentState.LogicUpdate();
        }
    }

    public void FixedUpdate()
    {
        if (_Ready && GameManagerMaster.GameMaster)
        {
            _CurrentState.PhysicsUpdate();
        }
    }

    public void LogFromState(string input)
    {
        print($">>> : {input} : <<< NPC :: {_CurrentState.ToString()}");
    }
}
