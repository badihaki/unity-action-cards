using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/New State Library", fileName = "StateLibrary_")]
public class NPCStateLibrary : ScriptableObject
{
	public NPCIdleState _IdleState;
	public NPCIdleState _IdleAggressiveState;
	public NPCMoveState _MoveState;
	public NPCMoveState _PatrolState;
	public NPCChaseState _ChaseState;
	public NPCHitState _HitState;
	public NPCAirHitState _AirHitState;
	public NPCStaggerState _StaggerState;
	public NPCKnockbackState _KnockbackState;
	public NPCLaunchState _LaunchState;
	public NPCFallingState _FallingState;

	public List<NPCAttackActionSO> _Attacks;

	public NPCStateLibrary() {}

	/*
	 - Defense States - Struct of different defenses
		- Dodge States Scriptable Objects - List<> of scriptable objects this entity can use to dodge
			- Dodge Power
			- Some dodges lead directly into attacks
		- Block State
	 */

	public void InitializeAllStates(NonPlayerCharacter _npc)
	{
		NPCStateLibrary copyLib = _npc._NPCharacterSheet.StateLibrary;
		// idle
		//Type tIdle = _IdleStates._IdleState.GetType();
		//_IdleState = ScriptableObject.CreateInstance<NPCIdleState>();
		_IdleState = Instantiate(copyLib._IdleState);
		_IdleState.InitState(_npc, _npc._StateMachine, "idle");
		_IdleState.name = $"{_npc.name}-Idle";
		//_IdleAggressiveState = ScriptableObject.CreateInstance<NPCIdleAggressiveState>();
		_IdleAggressiveState = Instantiate(copyLib._IdleAggressiveState);
		_IdleAggressiveState.InitState(_npc, _npc._StateMachine, "idle");
		_IdleAggressiveState.name = $"{_npc.name}-IdleAggressive";
		// move
		//_MoveState = ScriptableObject.CreateInstance<NPCMoveState>();
		_MoveState = Instantiate(copyLib._MoveState);
		_MoveState.InitState(_npc, _npc._StateMachine, "move");
		_MoveState.name = $"{_npc.name}-Move";
		//_PatrolState = ScriptableObject.CreateInstance<NPCMoveState>();
		_PatrolState = Instantiate(copyLib._PatrolState);
		_PatrolState.InitState(_npc, _npc._StateMachine, "move");
		_PatrolState.name = $"{_npc.name}-Patrol";
		//_ChaseState = ScriptableObject.CreateInstance<NPCMoveState>();
		_ChaseState = Instantiate(copyLib._ChaseState);
		_ChaseState.InitState(_npc, _npc._StateMachine, "move");
		_ChaseState.name = $"{_npc.name}-Chase";
		// attacks
		// handled in the move set
		//NPCMoveSet moveSet = _npc._MoveSet;
		//foreach (NPCAttackActionSO attackAction in _Attacks)
		//{
		//	attackAction.attackState.InitState(_npc, _npc._StateMachine, attackAction.animationName);
		//}
		// hurt states
		//_HitState = ScriptableObject.CreateInstance<NPCHitState>();
		_HitState = Instantiate(copyLib._HitState);
		_HitState.InitState(_npc, _npc._StateMachine, "hit");
		_HitState.name = $"{_npc.name}-Hit";
		//_AirHitState = ScriptableObject.CreateInstance<NPCAirHitState>();
		_AirHitState = Instantiate(copyLib._AirHitState);
		_AirHitState.InitState(_npc, _npc._StateMachine, "airHit");
		_AirHitState.name = $"{_npc.name}-AirHit";
		//_StaggerState = ScriptableObject.CreateInstance<NPCStaggerState>();
		_StaggerState = Instantiate(copyLib._StaggerState);
		_StaggerState.InitState(_npc, _npc._StateMachine, "stagger");
		_StaggerState.name = $"{_npc.name}-Stagger";
		//_KnockbackState = ScriptableObject.CreateInstance<NPCKnockbackState>();
		_KnockbackState = Instantiate(copyLib._KnockbackState);
		_KnockbackState.InitState(_npc, _npc._StateMachine, "knockback");
		_KnockbackState.name = $"{_npc.name}-Knockback";
		//_LaunchState = ScriptableObject.CreateInstance<NPCLaunchState>();
		_LaunchState = Instantiate(copyLib._LaunchState);
		_LaunchState.InitState(_npc, _npc._StateMachine, "launch");
		_LaunchState.name = $"{_npc.name}-Launch";
		// air states
		//_FallingState = ScriptableObject.CreateInstance<NPCFallingState>();
		_FallingState = Instantiate(copyLib._FallingState);
		_FallingState.InitState(_npc, _npc._StateMachine, "falling");
		_FallingState.name = $"{_npc.name}-Falling";
		if (GameManagerMaster.GameMaster.GMSettings.logExtraNPCData)
			Debug.Log($">>>>>>>>>>>>>>> initializing states <<<<<<<<<<<<<<<<<<");
	}

	// end of the line
}
