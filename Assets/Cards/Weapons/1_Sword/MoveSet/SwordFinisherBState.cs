using UnityEngine;

[CreateAssetMenu(fileName = "Sword Finisher B", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Finisher B")]
public class SwordFinisherBState : PlayerSpecialSuperState
{
	public SwordFinisherBState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}

	// this is a retreating slash

	public override void EnterState()
	{
		base.EnterState();
		//_AttackController.SetAttackParameters(true, false, 1);
		_AttackController.SetAttackParameters(responsesToDamage.stagger, 1, 0.2f);
	}
}
