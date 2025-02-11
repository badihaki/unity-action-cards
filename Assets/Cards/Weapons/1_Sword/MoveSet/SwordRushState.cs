using UnityEngine;

[CreateAssetMenu(fileName = "Sword Rush", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Rush")]
public class SwordRushState : PlayerRushAttackSuperState
{
	public SwordRushState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}

	public override void EnterState()
	{
		base.EnterState();
		//_AttackController.SetAttackParameters(true, false, 1);
		_AttackController.SetAttackParameters(responsesToDamage.stagger, 1, 1.5f);
	}
}
