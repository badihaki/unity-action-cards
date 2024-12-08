using UnityEngine;

public class NPCHitState : NPCHurtSuperState
{
	public override void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
	{
		base.InitState(npc, stateMachine, animationName);
	}
}
