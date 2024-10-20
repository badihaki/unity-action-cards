using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialSuperState : PlayerCombatSuperState
{
    public PlayerSpecialSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }
    
    public override void EnterState()
    {
        base.EnterState();
        // _PlayerCharacter._AnimationController.SetBool("special", true);
        _PlayerCharacter._Controls.UseAttack();
        _PlayerCharacter._AttackController.DetectNearbyTargets();
    }

    public override void ExitState()
    {
        base.ExitState();

        // _PlayerCharacter._AnimationController.SetBool("special", false);
        _PlayerCharacter._AttackController.ResetAttackParameters();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (_AnimationIsFinished) _StateMachine.ChangeState(_StateMachine._IdleState);
        if (Time.time > _StateEnterTime + 3.5f) _StateMachine.ChangeState(_StateMachine._IdleState);
    }

    // end of the line
}
