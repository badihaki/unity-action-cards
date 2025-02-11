using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Hurt/Hit", fileName = "Get Hit")]
public class NPCHitState : NPCHurtSuperState
{
	public override void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
	{
		base.InitState(npc, stateMachine, animationName);
	}
}
