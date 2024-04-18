using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unarmed Attack A", menuName = "Create Attacks/00_Unarmed/Unarmed Attack A")]
public class PlayerUnarmedAttackAState : PlayerAttackSuperState
{
    public PlayerUnarmedAttackAState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        // Debug.Log(_PlayerCharacter._AnimationController.GetBool("attack"));
        // Debug.Log(_PlayerCharacter._AnimationController.GetBool("attackA"));

        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
        _PlayerCharacter._AttackController.SetAttackParameters(1, 1.178f, 0.75f);
    }

    public override void ExitState()
    {
        base.ExitState();

        // Debug.Log(_PlayerCharacter._AnimationController.GetBool("attack"));
        // Debug.Log(_PlayerCharacter._AnimationController.GetBool("attackA"));
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (canCombo && attackInput) _StateMachine.ChangeState(_PlayerCharacter._AttackController._AttackB);
    }

    // end
}
