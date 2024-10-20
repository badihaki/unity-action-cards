using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    private Vector2 aimInput;

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._Controls.UseJump();
        _PlayerCharacter._LocomotionController.Jump();
        // _PlayerCharacter._LocomotionController.ApplyGravity(0.1f);
        _PlayerCharacter._LocomotionController.MoveWithVerticalVelocity();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _PlayerCharacter._CameraController.ControlCameraRotation(aimInput);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // _PlayerCharacter._LocomotionController.ApplyGravity(1);
        _PlayerCharacter._LocomotionController.MoveWithVerticalVelocity();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (_AnimationIsFinished)
        {
			// _PlayerCharacter.LogFromState("animation is finished");

			if (!_PlayerCharacter._CheckGrounded.IsGrounded())
            {
                // _PlayerCharacter.LogFromState("finishing, not on ground");
                _StateMachine.ChangeState(_StateMachine._FallingState);
            }
            else
			{
				// _PlayerCharacter.LogFromState("finishing, grounded");
                _PlayerCharacter._LocomotionController.SetDoubleJump(true);
                _PlayerCharacter._LocomotionController.SetAirDash(true);
				_StateMachine.ChangeState(_StateMachine._IdleState);
			}
		}
        
    }

    public override void CheckInputs()
    {
        base.CheckInputs();
        aimInput = _PlayerCharacter._Controls._AimInput;
    }

    // end
}
