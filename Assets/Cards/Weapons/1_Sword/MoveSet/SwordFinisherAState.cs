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

	private bool attackPropertiesChanged;

	public override void EnterState()
	{
		base.EnterState();
		attackPropertiesChanged = false;
		_AttackController.SetAttackParameters(true, false);
	}

	public override void TriggerSideEffect()
	{
		if (!attackPropertiesChanged)
		{
			_AttackController.SetAttackParameters(true, true, 2);
			attackPropertiesChanged = true;
		}
		else
			base.TriggerSideEffect();
	}
}
