using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Attack B", menuName = "Create Attacks/00_Unarmed/Unarmed Attack B")]
public class PlayerUnarmedAttackBState : PlayerAttackSuperState
{
    public PlayerUnarmedAttackBState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("entering unarmed attack B state at " + Time.time);
        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
        _PlayerCharacter._AttackController.SetAttackParameters(1, 2, 1.35f);
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("exiting attack state b at " + Time.time);
    }

    public override void CheckStateTransitions()
    {
        if (canCombo && attackInput) _StateMachine.ChangeState(_PlayerCharacter._AttackController._AttackC);

        base.CheckStateTransitions();
    }

    // end
}
