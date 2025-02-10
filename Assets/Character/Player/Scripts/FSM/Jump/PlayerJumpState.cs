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
    private bool attackInput;
    private bool specialInput;
    private bool jumpInput;
    private bool canTakeAction;
    private PlayerAttackController attackController;

	public override void InitializeState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine)
	{
		base.InitializeState(pc, animationName, stateMachine);
        attackController = pc._AttackController as PlayerAttackController;
	}

	public override void EnterState()
    {
        base.EnterState();

        canTakeAction = false;
		_PlayerCharacter._LocomotionController.RotateInstantly(_PlayerCharacter._Controls._MoveInput);
		_PlayerCharacter._Controls.UseJump();
        _PlayerCharacter._LocomotionController.Jump();
        _PlayerCharacter._Controls.UseRush();
        // _PlayerCharacter._LocomotionController.ApplyGravity(0.1f);
        _PlayerCharacter._LocomotionController.MoveWithVerticalVelocity();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // _PlayerCharacter._LocomotionController.ApplyGravity(1);
        _PlayerCharacter._LocomotionController.MoveWithVerticalVelocity();
        _PlayerCharacter._CameraController.ControlCameraRotation(aimInput);
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		_PlayerCharacter._CameraController.MakeCameraFollowPlayerActor();
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
        if (canTakeAction)
        {
            if (attackInput)
                _StateMachine.ChangeState(attackController._AirAttackA);
            if (specialInput)
                _StateMachine.ChangeState(attackController._AirSpecial);
            if (jumpInput)
                _StateMachine.ChangeState(_StateMachine._AirJumpState);
		}
    }

    public override void CheckInputs()
    {
        base.CheckInputs();
        aimInput = _PlayerCharacter._Controls._AimInput;
        attackInput = _PlayerCharacter._Controls._AttackInput;
        specialInput = _PlayerCharacter._Controls._SpecialAttackInput;
        jumpInput = _PlayerCharacter._Controls._JumpInput;
	}

	public override void TriggerSideEffect()
	{
		base.TriggerSideEffect();

        canTakeAction = true;
	}

	// end
}
