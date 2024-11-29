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
	
    // hurt
    public NPCHitState _HitState { get; private set; }
    public NPCKnockbackState _KnockBackState { get; private set; }
    public NPCLaunchState _LaunchState { get; private set; }

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
			_IdleState = ScriptableObject.CreateInstance<NPCIdleState>();
		}
		_IdleState.InitState(npc, this, "idle");

		if (!_IdleAggressiveState)
		{
			_IdleAggressiveState = ScriptableObject.CreateInstance<NPCIdleAggressiveState>();
		}
		_IdleAggressiveState.InitState(npc, this, "idle");

		if (!_MoveState)
		{
			_MoveState = ScriptableObject.CreateInstance<NPCMoveState>();
		}
		_MoveState.InitState(npc, this, "move");

		if (!_FallingState)
		{
			_FallingState = ScriptableObject.CreateInstance<NPCFallingState>();
		}
		_FallingState.InitState(npc, this, "falling");

		InitHurtStates(npc);
	}

	private void InitHurtStates(NonPlayerCharacter npc)
	{
		if (!_HitState)
		{
			_HitState = ScriptableObject.CreateInstance<NPCHitState>();
		}
		_HitState.InitState(npc, this, "hit");
		if (!_KnockBackState)
		{
			_KnockBackState = ScriptableObject.CreateInstance<NPCKnockbackState>();
		}
		_KnockBackState.InitState(npc, this, "hit");
		if (!_LaunchState)
		{
			_LaunchState = ScriptableObject.CreateInstance<NPCLaunchState>();
		}
		_LaunchState.InitState(npc, this, "hit");
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
