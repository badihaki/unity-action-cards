using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerInAirSuperState
{
    public PlayerFallingState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _PlayerCharacter._LocomotionController.MoveTowardsCam(moveInput);
        _PlayerCharacter._LocomotionController.ApplyGravity();

        if (moveInput != Vector2.zero)
            _PlayerCharacter._LocomotionController.MoveTowardsCam(moveInput);
        else
            _PlayerCharacter._LocomotionController.SlowDown();
    }

    public override void CheckStateTransitions()
    {
        if (_PlayerCharacter._CheckGrounded) _StateMachine.ChangeState(_PlayerCharacter._LandingState);
    }
}
