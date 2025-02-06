using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static NPCStateLibrary;

[Serializable]
public struct NPCStateLibrary
{
	[Serializable]
	public struct IdleStates
	{
		public NPCIdleState _IdleState;
		public NPCIdleState _IdleAggressiveState;
	}
	public IdleStates _IdleStates;

	[Serializable]
	public struct MoveStates
	{
		public NPCMoveState _MoveState;
		public NPCMoveState _PatrolState;
		public NPCMoveState _ChaseState;
	}
	public MoveStates _MoveStates;

	public List<NPCAttackActionSO> _Attacks;

	[Serializable]
	public struct HurtStates
	{
		public NPCHurtSuperState _HitState;
		public NPCHurtSuperState _AirHitState;
		public NPCHurtSuperState _StaggerState;
		public NPCHurtSuperState _KnockbackState;
		public NPCHurtSuperState _LaunchState;
	}
	public HurtStates _HurtStates;

	/*
	 - Defense States - Struct of different defenses
		- Dodge States Scriptable Objects - List<> of scriptable objects this entity can use to dodge
			- Dodge Power
			- Some dodges lead directly into attacks
		- Block State
	 */

	public void InitializeAllStates(NonPlayerCharacter _npc)
	{
		// idle
		//Type tIdle = _IdleStates._IdleState.GetType();
		_IdleStates._IdleState = ScriptableObject.CreateInstance<NPCIdleState>();
		_IdleStates._IdleState.InitState(_npc, _npc._StateMachine, "idle");
		_IdleStates._IdleAggressiveState = ScriptableObject.CreateInstance<NPCIdleState>();
		_IdleStates._IdleAggressiveState.InitState(_npc, _npc._StateMachine, "idle");
		// move
		_MoveStates._MoveState = ScriptableObject.CreateInstance<NPCMoveState>();
		_MoveStates._MoveState.InitState(_npc, _npc._StateMachine, "move");
		_MoveStates._PatrolState = ScriptableObject.CreateInstance<NPCMoveState>();
		_MoveStates._PatrolState.InitState(_npc, _npc._StateMachine, "move");
		_MoveStates._ChaseState = ScriptableObject.CreateInstance<NPCMoveState>();
		_MoveStates._ChaseState.InitState(_npc, _npc._StateMachine, "move");
		// attacks
		// handled in the move set
		//NPCMoveSet moveSet = _npc._MoveSet;
		//foreach (NPCAttackActionSO attackAction in _Attacks)
		//{
		//	attackAction.attackState.InitState(_npc, _npc._StateMachine, attackAction.animationName);
		//	if (GameManagerMaster.GameMaster.GMSettings.logExtraNPCData)
		//		Debug.Log($">>>>>>>>>>>>>>> initializing state {attackAction.animationName} <<<<<<<<<<<<<<<<<<");
		//}
		// hurt states
		_HurtStates._HitState = ScriptableObject.CreateInstance<NPCHurtSuperState>();
		_HurtStates._HitState.InitState(_npc, _npc._StateMachine, "hit");
		_HurtStates._AirHitState = ScriptableObject.CreateInstance<NPCHurtSuperState>();
		_HurtStates._AirHitState.InitState(_npc, _npc._StateMachine, "airHit");
		_HurtStates._StaggerState = ScriptableObject.CreateInstance<NPCHurtSuperState>();
		_HurtStates._StaggerState.InitState(_npc, _npc._StateMachine, "stagger");
		_HurtStates._KnockbackState = ScriptableObject.CreateInstance<NPCHurtSuperState>();
		_HurtStates._KnockbackState.InitState(_npc, _npc._StateMachine, "knockback");
		_HurtStates._LaunchState = ScriptableObject.CreateInstance<NPCHurtSuperState>();
		_HurtStates._LaunchState.InitState(_npc, _npc._StateMachine, "launch");
	}

	// end of the line
}
