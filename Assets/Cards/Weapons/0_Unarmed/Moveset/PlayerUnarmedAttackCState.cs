using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Attack C", menuName = "Characters/Player/Create Attacks/00_Unarmed/Unarmed Attack C")]
public class PlayerUnarmedAttackCState : PlayerAttackSuperState
{
    public PlayerUnarmedAttackCState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._MoveController.ZeroOutVelocity();
        //_AttackController.SetAttackParameters(false, false);
        _AttackController.SetAttackParameters(responsesToDamage.hit, 1);
		_PlayerCharacter._WeaponController.SetShowWeapons(true);
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        //if (canCombo && specialInput) _StateMachine.ChangeState(_AttackController._FinisherA);
        //if (canCombo && jumpInput) _StateMachine.ChangeState(_AttackController._LauncherAttack);

        if (canCombo)
			switch (_PlayerCharacter._Controls.PollForDesiredInput())
			{
				case InputProperties.InputType.jump:
					_PlayerCharacter._Controls.UseJump();
					_StateMachine.ChangeState(_AttackController._LauncherAttack);
					break;
				//case InputProperties.InputType.attack:
				//	_PlayerCharacter._Controls.UseAttack();
				//	_StateMachine.ChangeState(attackController._AirAttackA);
				//	break;
				case InputProperties.InputType.special:
					_PlayerCharacter._Controls.UseSpecialAttack();
					_StateMachine.ChangeState(_AttackController._FinisherA);
					break;
				//case InputProperties.InputType.defense:
				//	break;
				default:
					break;
			}
	}

    public override void ExitState()
    {
        base.ExitState();
		_PlayerCharacter._WeaponController.SetShowWeapons(false);
	}
}
