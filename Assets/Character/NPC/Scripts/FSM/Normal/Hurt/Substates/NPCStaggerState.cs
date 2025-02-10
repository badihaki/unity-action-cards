using UnityEngine;

[CreateAssetMenu(menuName = "Characters/NPC/FSM/Hurt/Stagger", fileName = "Stagger")]
public class NPCStaggerState : NPCHurtSuperState
{
	public override void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
	{
		base.InitState(npc, stateMachine, animationName);
	}
}
