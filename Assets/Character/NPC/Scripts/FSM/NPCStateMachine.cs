using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
	[field: SerializeField]
	public NPCStateLibrary _StateLibrary { get; private set; }
	[field: SerializeField]
    public NPCState _CurrentState { get; private set; }
	[field: SerializeField]
	public string _StateAnimationName { get; private set; }

	private bool _Ready = false;
    

    public void InitializeStateMachine(NonPlayerCharacter npc)
    {
		NPCSheetScriptableObj characterSheet = npc._CharacterSheet as NPCSheetScriptableObj;
		//SetUpStateMachine(npc);
		_StateLibrary = ScriptableObject.CreateInstance<NPCStateLibrary>();
		_StateLibrary.InitializeAllStates(npc);

		// set current state
        _CurrentState = _StateLibrary._IdleState;
		_StateAnimationName = _CurrentState._StateAnimationName;
        _CurrentState.EnterState();
        _Ready = true;
    }

	/*
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
			_KnockBackState = ScriptableObject.CreateInstance<NPCStaggerState>();
		}
		_KnockBackState.InitState(npc, this, "knockback");
		if (!_LaunchState)
		{
			_LaunchState = ScriptableObject.CreateInstance<NPCLaunchState>();
		}
		_LaunchState.InitState(npc, this, "launch");
		if(!_AirHitState)
		{
			_AirHitState = ScriptableObject.CreateInstance<NPCAirHitState>();
		}
		_AirHitState.InitState(npc, this, "airHit");
		if (!_FarKnockBackState)
		{
			_FarKnockBackState = ScriptableObject.CreateInstance<NPCKnockbackState>();
		}
		_FarKnockBackState.InitState(npc, this, "farKnockBack");
	}
	*/

	public void ChangeState(NPCState state)
    {
        _CurrentState.ExitState();
        _CurrentState = state;
		_StateAnimationName = _CurrentState._StateAnimationName;
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
