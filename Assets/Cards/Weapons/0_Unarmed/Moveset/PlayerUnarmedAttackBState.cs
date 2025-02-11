using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Attack B", menuName = "Characters/Player/Create Attacks/00_Unarmed/Unarmed Attack B")]
public class PlayerUnarmedAttackBState : PlayerAttackSuperState
{
    public PlayerUnarmedAttackBState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
		//_AttackController.SetAttackParameters(false, false, 1);
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
        if (canCombo && attackInput) _StateMachine.ChangeState(_AttackController._AttackC);
        if (canCombo && specialInput) _StateMachine.ChangeState(_AttackController._FinisherA);
        if (canCombo && jumpInput) _StateMachine.ChangeState(_AttackController._LauncherAttack);

        base.CheckStateTransitions();
    }

    // end
}
