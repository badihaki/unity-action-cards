using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandingState : PlayerState
{
    public PlayerLandingState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _PlayerCharacter._LocomotionController.ApplyGravity();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (_AnimationIsFinished) _StateMachine.ChangeState(_PlayerCharacter._IdleState);
    }
}
