using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Air Attack A", menuName = "Characters/Player/Create Attacks/00_Unarmed/Unarmed Air Attack A")]
public class PlayerUnarmedAirAttackAState : PlayerAirCombatSuperState
{
    public PlayerUnarmedAirAttackAState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
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

    //    {
    //        if (attackInput)
    //            _StateMachine.ChangeState(_AttackController._AirAttackB);
    //        if(specialInput)
				//_StateMachine.ChangeState(_AttackController._AirSpecial);
        if(canCombo)
			switch (_PlayerCharacter._Controls.PollForDesiredInput())
			{
				case InputProperties.InputType.jump:
					//_StateMachine.ChangeState(_StateMachine._AirJumpState);
					break;
				case InputProperties.InputType.attack:
					_PlayerCharacter._Controls.UseAttack();
					_StateMachine.ChangeState(_AttackController._AirAttackB);
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
