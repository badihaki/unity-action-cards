using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Attack A", menuName = "Characters/Player/Create Attacks/00_Unarmed/Unarmed Attack A")]
public class PlayerUnarmedAttackAState : PlayerAttackSuperState
{
    public PlayerUnarmedAttackAState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        
        _PlayerCharacter._MoveController.ZeroOutVelocity();
		_AttackController.SetAttackParameters();
		_PlayerCharacter._WeaponController.SetShowWeapons(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        _PlayerCharacter._WeaponController.SetShowWeapons(false);
    }

    public override void CheckStateTransitions()
    {
        //if (canCombo && attackInput) _StateMachine.ChangeState(_AttackController._AttackB);
        //if (canCombo && specialInput) _StateMachine.ChangeState(_AttackController._FinisherA);
        //if (canCombo && jumpInput) _StateMachine.ChangeState(_AttackController._LauncherAttack);

        if(canCombo)
			switch (_PlayerCharacter._Controls.PollForDesiredInput())
			{
				case InputProperties.InputType.jump:
					_PlayerCharacter._Controls.UseJump();
					_StateMachine.ChangeState(_AttackController._LauncherAttack);
					break;
				case InputProperties.InputType.attack:
					_PlayerCharacter._Controls.UseAttack();
					_StateMachine.ChangeState(_AttackController._AttackB);
					break;
				case InputProperties.InputType.special:
					_PlayerCharacter._Controls.UseSpecialAttack();
					_StateMachine.ChangeState(_AttackController._FinisherA);
					break;
				case InputProperties.InputType.defense:
					break;
				default:
					break;
			}

		base.CheckStateTransitions();
    }

    // end
}
