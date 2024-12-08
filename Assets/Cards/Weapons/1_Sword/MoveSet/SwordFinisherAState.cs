using UnityEngine;

[CreateAssetMenu(fileName = "Sword Finisher A", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Finisher A")]
public class SwordFinisherAState : PlayerSpecialSuperState
{
	public SwordFinisherAState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}

	/*
	 * this is a charge then a swing
	 * the charge has a hitbox
	 * the swing has a hitbox with knockback properties
	 */

	public override void EnterState()
	{
		base.EnterState();
		_AttackController.SetAttackParameters(false, false, 2);
	}
}
