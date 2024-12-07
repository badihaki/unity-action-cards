using UnityEngine;

[CreateAssetMenu(fileName = "Sword Finisher C", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Finisher C")]
public class SwordFinisherCState : PlayerSpecialSuperState
{
	public SwordFinisherCState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}

	/*
	 * This is a 2 hit swirl with a finishing launcher.
	 * The swirls do knockback damage
	 */

	public override void EnterState()
	{
		base.EnterState();
		_AttackController.SetAttackParameters(true, false, 1);
	}
}
