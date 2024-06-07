using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSuperState : PlayerCombatSuperState
{
    public PlayerAttackSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    protected bool canCombo;
    public override void EnterState()
    {
        base.EnterState();

        canCombo = false;
        _PlayerCharacter._AnimationController.SetBool("attack", true);
        _PlayerCharacter._Controls.UseAttack();
        _PlayerCharacter._AttackController.DetectNearbyTargets();
    }

    public override void ExitState()
    {
        base.ExitState();

        _PlayerCharacter._AnimationController.SetBool("attack", false);
        _PlayerCharacter._AttackController.ResetAttackParameters();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void TriggerSideEffect()
    {
        base.TriggerSideEffect();

        canCombo = true;
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (_AnimationIsFinished) _StateMachine.ChangeState(_PlayerCharacter._IdleState);
        if(Time.time > _StateEnterTime + 3.5f) _StateMachine.ChangeState(_PlayerCharacter._IdleState);
    }

    public override void CheckInputs()
    {
        base.CheckInputs();

        if (attackInput) HandleAttack();
    }

    public virtual void HandleAttack()
    {
        _PlayerCharacter._Controls.UseAttack();
    }

    // end
}
