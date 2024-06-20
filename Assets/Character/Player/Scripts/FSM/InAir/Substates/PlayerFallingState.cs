using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerInAirSuperState
{
    public PlayerFallingState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _PlayerCharacter._CameraController.ControlCameraRotation(aimInput);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (moveInput != Vector2.zero)
            _PlayerCharacter._LocomotionController.MoveTowardsCamWithGravity(moveInput, false);
        else
            {
                Console.WriteLine(moveInput);
                _PlayerCharacter._LocomotionController.SlowDown();
                _PlayerCharacter._LocomotionController.ApplyGravity();
            }
    }

    public override void CheckStateTransitions()
    {
        if (_PlayerCharacter._CheckGrounded.IsGrounded()) _StateMachine.ChangeState(_PlayerCharacter._LandingState);
    }
}
