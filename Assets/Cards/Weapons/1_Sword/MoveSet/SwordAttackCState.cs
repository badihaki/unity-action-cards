using UnityEngine;

[CreateAssetMenu(fileName = "Sword Attack C", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Attack C")]
public class SwordAttackCState : PlayerAttackSuperState
{
	public SwordAttackCState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}

	public override void EnterState()
	{
		base.EnterState();
		_PlayerCharacter._LocomotionController.ZeroOutVelocity();
		_AttackController.SetAttackParameters(1.025f, 1.35f, 2);
		ShowOrHideWeapon(true);
	}
}
