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

	/*
	 - Defense States - Struct of different defenses
		- Dodge States Scriptable Objects - List<> of scriptable objects this entity can use to dodge
			- Dodge Power
			- Some dodges lead directly into attacks
		- Block State
	 */
}
