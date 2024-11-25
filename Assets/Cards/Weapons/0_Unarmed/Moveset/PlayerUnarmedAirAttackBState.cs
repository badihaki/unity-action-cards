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

        _AttackController.SetAttackParameters(0.15f, 1.75f);
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (canCombo && attackInput)
        {
            _StateMachine.ChangeState(_AttackController._AirAttackA);
        }
    }
}
