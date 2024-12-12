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

        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
        _AttackController.SetAttackParameters(false, false);
        ShowOrHideWeapon(true);
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (canCombo && specialInput) _StateMachine.ChangeState(_AttackController._FinisherA);
        if (canCombo && jumpInput) _StateMachine.ChangeState(_AttackController._LauncherAttack);
    }

    public override void ExitState()
    {
        base.ExitState();
        ShowOrHideWeapon(false);
    }
}
