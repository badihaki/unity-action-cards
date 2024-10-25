using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Air Attack A", menuName = "Create Attacks/00_Unarmed/Unarmed Air Attack A")]
public class PlayerUnarmedAirAttackAState : PlayerAirCombatSuperState
{
    public PlayerUnarmedAirAttackAState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._AttackController.SetAttackParameters(1, 0.15f, 1.75f);
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if(canCombo && attackInput)
        {
            _StateMachine.ChangeState(_PlayerCharacter._AttackController._AirAttackB);
        }
    }
}
