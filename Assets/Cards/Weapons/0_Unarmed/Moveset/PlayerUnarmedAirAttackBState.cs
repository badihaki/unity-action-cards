using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Air Attack B", menuName = "Characters/Player/Create Attacks/00_Unarmed/Unarmed Air Attack B")]
public class PlayerUnarmedAirAttackBState : PlayerAirCombatSuperState
{
    public PlayerUnarmedAirAttackBState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        //_AttackController.SetAttackParameters(false, false);
        _AttackController.SetAttackParameters();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (canCombo)
        {
            //if (attackInput)
            //    _StateMachine.ChangeState(_AttackController._AirAttackA);
            //if (specialInput)
            //    _StateMachine.ChangeState(_AttackController._AirSpecial);
			switch (_PlayerCharacter._Controls.PollForDesiredInput())
			{
				//case InputProperties.InputType.jump:
				//	_PlayerCharacter._Controls.UseJump();
				//	_StateMachine.ChangeState(_StateMachine._AirJumpState);
				//	break;
				case InputProperties.InputType.attack:
					_PlayerCharacter._Controls.UseAttack();
					_StateMachine.ChangeState(_AttackController._AirAttackA);
					break;
				case InputProperties.InputType.special:
					_PlayerCharacter._Controls.UseSpecialAttack();
					_StateMachine.ChangeState(_AttackController._AirSpecial);
					break;
				case InputProperties.InputType.defense:
					break;
				default:
					break;
			}
		}
    }
}
