using NUnit.Framework;
using System;
using System.Collections.Generic;

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

	public List<NPCAttackActionSO> Attacks;

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
		_IdleStates._IdleState.InitState(_npc, _npc._StateMachine, "idle");
		_IdleStates._IdleAggressiveState.InitState(_npc, _npc._StateMachine, "idle");
		// move
		_MoveStates._MoveState.InitState(_npc, _npc._StateMachine, "move");
		_MoveStates._PatrolState.InitState(_npc, _npc._StateMachine, "move");
		_MoveStates._ChaseState.InitState(_npc, _npc._StateMachine, "move");
	}

	// end of the line
}
