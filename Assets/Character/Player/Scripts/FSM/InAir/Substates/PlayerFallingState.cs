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

        _PlayerCharacter._CameraController.ControlCameraRotation(aimInput);

        fallTime += Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (moveInput != Vector2.zero)
            _PlayerCharacter._LocomotionController.DetectMove(moveInput);
        else
            {
                Console.WriteLine(moveInput);
                _PlayerCharacter._LocomotionController.SlowDown();
                _PlayerCharacter._LocomotionController.ApplyGravity();
            }
    }

    public override void CheckStateTransitions()
    {
        if (_PlayerCharacter._CheckGrounded.IsGrounded())
        {
            if (fallTime > 2.85f) _StateMachine.ChangeState(_PlayerCharacter._LandingState);
            else _StateMachine.ChangeState(_PlayerCharacter._IdleState);
        }
    }
}
