using System.Collections;
using System.Collections.Generic;
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
		ShowOrHideWeapon(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        ShowOrHideWeapon(false);
    }

    public override void CheckStateTransitions()
    {
        if (canCombo && attackInput) _StateMachine.ChangeState(_AttackController._AttackB);
        if (canCombo && specialInput) _StateMachine.ChangeState(_AttackController._FinisherA);
        if (canCombo && jumpInput) _StateMachine.ChangeState(_AttackController._LauncherAttack);

        base.CheckStateTransitions();
    }

    // end
}
