using UnityEngine;

[CreateAssetMenu(fileName = "Sword Special", menuName = "Create Attacks/01_Sword/Sword Special")]
public class SwordSpecialState : PlayerSpecialSuperState
{
	public SwordSpecialState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}

	public override void EnterState()
	{
		base.EnterState();
		_PlayerCharacter._AttackController.SetAttackParameters(0.78f, 1.75f, 2);
	}
}
