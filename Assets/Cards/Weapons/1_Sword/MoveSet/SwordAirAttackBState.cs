using UnityEngine;

[CreateAssetMenu(fileName = "Sword Air Attack B", menuName = "Characters/Player/Create Attacks/01_Sword/Sword Air Attack B")]
public class SwordAirAttackBState : PlayerAirCombatSuperState
{
	public SwordAirAttackBState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
	{
	}

	public override void EnterState()
	{
		base.EnterState();

		//_AttackController.SetAttackParameters(false, false, 2);
		_AttackController.SetAttackParameters(responsesToDamage.hit, 1, 0.1f);
	}

	public override void CheckStateTransitions()
	{
		base.CheckStateTransitions();

		if (canCombo)
		{
			//if (attackInput)
			//	_StateMachine.ChangeState(_AttackController._AirAttackC);
			//if (specialInput)
			//	_StateMachine.ChangeState(_AttackController._AirSpecial);
			switch (_PlayerCharacter._Controls.PollForDesiredInput())
			{
				case InputProperties.InputType.attack:
					_PlayerCharacter._Controls.UseAttack();
					_StateMachine.ChangeState(_AttackController._AirAttackC);
					break;
				case InputProperties.InputType.special:
					_PlayerCharacter._Controls.UseSpecialAttack();
					_StateMachine.ChangeState(_AttackController._AirSpecial);
					break;
				default:
					break;
			}
		}
	}
}
