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
		_PlayerCharacter._MoveController.ZeroOutVelocity();
		//_AttackController.SetAttackParameters(false, false, 2);
		_AttackController.SetAttackParameters(responsesToDamage.hit, 2);
		ShowOrHideWeapon(true);
	}

	public override void CheckStateTransitions()
	{
		if (canCombo)
		{
			//if (jumpInput) _StateMachine.ChangeState(_AttackController._LauncherAttack);
			//if (specialInput) _StateMachine.ChangeState(_AttackController._FinisherC);

			switch (_PlayerCharacter._Controls.PollForDesiredInput())
			{
				case InputProperties.InputType.jump:
					_PlayerCharacter._Controls.UseJump();
					_StateMachine.ChangeState(_AttackController._LauncherAttack);
					break;
				case InputProperties.InputType.special:
					_PlayerCharacter._Controls.UseSpecialAttack();
					_StateMachine.ChangeState(_AttackController._FinisherC);
					break;
				default:
					break;
			}
		}

		base.CheckStateTransitions();
	}
}
