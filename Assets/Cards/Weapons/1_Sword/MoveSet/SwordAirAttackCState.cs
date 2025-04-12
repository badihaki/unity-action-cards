using UnityEngine;

[CreateAssetMenu(fileName = "Sword Air Attack C", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Air Attack C")]
public class SwordAirAttackCState : PlayerAirCombatSuperState
{
	public SwordAirAttackCState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}
	public override void EnterState()
	{
		base.EnterState();

		//_AttackController.SetAttackParameters(true, true, 1);
		_AttackController.SetAttackParameters(responsesToDamage.launch, 1, 0.15f);
	}

	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();

		if (canCombo)
		{
			if (specialInput)
				_StateMachine.ChangeState(_AttackController._AirSpecial);

			switch (_PlayerCharacter._Controls.PollForDesiredInput())
			{
				case InputProperties.InputType.special:
					_PlayerCharacter._Controls.UseSpecialAttack();
					_StateMachine.ChangeState(_AttackController._AirSpecial);
					break;
				default:
					break;
			}
		}
	}

	public override void TriggerSideEffect()
	{
		_PlayerCharacter._MoveController.Jump(0.125f);
	}
}
