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
    private PlayerAttackController _AttackController;

	public override void InitializeState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine)
	{
		base.InitializeState(pc, animationName, stateMachine);
        _AttackController = pc._AttackController as PlayerAttackController;
	}

	public override void EnterState()
    {
        base.EnterState();

        canTakeAction = false;
		_PlayerCharacter._MoveController.RotateInstantly(_PlayerCharacter._Controls._MoveInput);
		_PlayerCharacter._Controls.UseJump();
        _PlayerCharacter._MoveController.Jump();
        _PlayerCharacter._Controls.UseRush();
        _PlayerCharacter._MoveController.MoveWithVerticalVelocity();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _PlayerCharacter._MoveController.MoveWithVerticalVelocity();
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
			if (!_PlayerCharacter._CheckGrounded.IsGrounded()) // in air
            {
                _StateMachine.ChangeState(_StateMachine._FallingState);
            }
            else // grounded
			{
                _PlayerCharacter._MoveController.SetDoubleJump(true);
                _PlayerCharacter._MoveController.SetAirDash(true);
				_StateMachine.ChangeState(_StateMachine._IdleState);
			}
		}
        if (canTakeAction)
        {
            //if (attackInput)
            //    _StateMachine.ChangeState(attackController._AirAttackA);
            //if (specialInput)
            //    _StateMachine.ChangeState(attackController._AirSpecial);
            //if (jumpInput)
            //    _StateMachine.ChangeState(_StateMachine._AirJumpState);
            switch (_PlayerCharacter._Controls.PollForDesiredInput())
            {
				case InputProperties.InputType.jump:
                    _PlayerCharacter._Controls.UseJump();
					_StateMachine.ChangeState(_StateMachine._AirJumpState);
					break;
				case InputProperties.InputType.attack:
					_PlayerCharacter._Controls.UseAttack();
					_StateMachine.ChangeState(_AttackController._AirAttackA);
					break;
				case InputProperties.InputType.special:
					_PlayerCharacter._Controls.UseSpecialAttack();
					_StateMachine.ChangeState(_AttackController._AirSpecial);
					break;
				case InputProperties.InputType.defense:
					break;  
                default:
                    break;
			}
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
