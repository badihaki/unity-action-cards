using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Air Attack B", menuName = "Create Attacks/00_Unarmed/Unarmed Air Attack B")]
public class PlayerUnarmedAirAttackBState : PlayerAirCombatSuperState
{
    public PlayerUnarmedAirAttackBState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (canCombo && attackInput)
        {
            _StateMachine.ChangeState(_PlayerCharacter._AttackController._AirAttackA);
        }
    }
}
