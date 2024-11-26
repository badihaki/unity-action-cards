using UnityEngine;

[CreateAssetMenu(fileName = "Sword Special", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Special")]
public class SwordSpecialState : PlayerSpecialSuperState
{
	public SwordSpecialState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}

	public override void EnterState()
	{
		base.EnterState();
		_AttackController.SetAttackParameters(false, false, 2);
	}
}
