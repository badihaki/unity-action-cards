using UnityEngine;

public class PlayerHitState : PlayerHurtSuperState
{
	public PlayerHitState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}
}
