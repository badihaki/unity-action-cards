using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSuperState : PlayerCombatSuperState
{
    public PlayerAttackSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._AnimationController.SetBool("attack", true);
        _PlayerCharacter._Controls.UseAttack();
        _PlayerCharacter._AttackController.DetectMeleeTargets();
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
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
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
