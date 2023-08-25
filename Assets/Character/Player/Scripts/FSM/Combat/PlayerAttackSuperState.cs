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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_AnimationIsFinished) _StateMachine.ChangeState(_PlayerCharacter._IdleState);
    }

    public override void TriggerSideEffect()
    {
        base.TriggerSideEffect();

        canCombo = true;
    }

    // end
}
