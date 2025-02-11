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

        if(canCombo)
        {
            if (attackInput)
                _StateMachine.ChangeState(_AttackController._AirAttackB);
            if(specialInput)
				_StateMachine.ChangeState(_AttackController._AirSpecial);
		}
    }
}
