using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerInAirSuperState
{
    public PlayerFallingState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    private float fallTime;

    public override void EnterState()
    {
        base.EnterState();

        fallTime = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        fallTime += Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();
        if (_PlayerCharacter._CheckGrounded.IsGrounded())
        {
            if (fallTime > 2.85f) _StateMachine.ChangeState(_StateMachine._LandingState);
            else _StateMachine.ChangeState(_StateMachine._IdleState);
        }
        if (jumpInput && _PlayerCharacter._MoveController.canDoubleJump)
        {
            _StateMachine.ChangeState(_StateMachine._AirJumpState);
        }
        if (_PlayerCharacter._MoveController.canAirDash && rushInput && Time.time >= _StateEnterTime + 0.35f)
        {
            _PlayerCharacter._MoveController.ZeroOutVertVelocity();
            _StateMachine.ChangeState(_StateMachine._AirDashState);
        }
    }
}
