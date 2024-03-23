using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSuperState : PlayerCombatSuperState
{
    public PlayerAttackSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }
    public void ManualSetUp(PlayerCharacter pc, string animName,PlayerStateMachine stateMachine)
    {
        _PlayerCharacter = pc;
        _StateAnimationName = animName;
        _StateMachine = stateMachine;
    }
    protected bool canCombo;

    public override void EnterState()
    {
        base.EnterState();

        canCombo = false;
        _PlayerCharacter._Controls.UseAttack();
    }

    public override void ExitState()
    {
        base.ExitState();

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
        if(_StateEnterTime + 3.5f > _StateEnterTime) _StateMachine.ChangeState(_PlayerCharacter._IdleState);
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
