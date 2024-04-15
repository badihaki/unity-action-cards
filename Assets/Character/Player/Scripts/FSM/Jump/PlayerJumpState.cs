using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._Controls.UseJump();
        _PlayerCharacter._LocomotionController.Jump();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        /*_PlayerCharacter._LocomotionController.ApplyGravity();*/
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (_PlayerCharacter._CheckGrounded.IsGrounded() && Time.time > _StateEnterTime + 0.85f) _StateMachine.ChangeState(_PlayerCharacter._IdleState);
        if (_AnimationIsFinished) _StateMachine.ChangeState(_PlayerCharacter._FallingState);
    }

    // end
}
