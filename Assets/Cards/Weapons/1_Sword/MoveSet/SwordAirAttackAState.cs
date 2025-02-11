using UnityEngine;

[CreateAssetMenu(fileName = "Sword Air Attack A", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Air Attack A")]
public class SwordAirAttackAState : PlayerAirCombatSuperState
{
	public SwordAirAttackAState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}

	public override void EnterState()
	{
		base.EnterState();

		//_AttackController.SetAttackParameters(false, false, 1);
		_AttackController.SetAttackParameters(responsesToDamage.hit, 0, 1.5f);
	}

	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();

		if (canCombo)
		{
			if (attackInput)
				_StateMachine.ChangeState(_AttackController._AirAttackB);
			if (specialInput)
				_StateMachine.ChangeState(_AttackController._AirSpecial);
		}
	}
}
