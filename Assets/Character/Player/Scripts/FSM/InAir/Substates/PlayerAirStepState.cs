using UnityEngine;

public class PlayerAirStepState : PlayerState
{
    public PlayerAirStepState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    private bool canShmoove;
    private bool activateFirstStep;


    private Vector2 aimInput;
    private bool jumpInput;
    private bool rushInput;

    public override void EnterState()
    {
        base.EnterState();

        activateFirstStep = false;
        canShmoove = false;

        _PlayerCharacter._LocomotionController.RotateInstantly(_PlayerCharacter._Controls._MoveInput);
        _PlayerCharacter._Controls.UseRush();
        _PlayerCharacter._LocomotionController.SetAirDash(false);
		_PlayerCharacter._LocomotionController.Jump();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (_AnimationIsFinished)
        {
            _StateMachine.ChangeState(_StateMachine._FallingState);
        }

        if (canShmoove)
        {
			// check for jump and rush input here
			if (jumpInput)
            {
				_PlayerCharacter._Controls.UseJump();
				_StateMachine.ChangeState(_StateMachine._AirJumpState);
			}
            if (rushInput)
            {
				_PlayerCharacter._Controls.UseRush();
				_StateMachine.ChangeState(_StateMachine._FallingState);
			}
			//if (_PlayerCharacter._LocomotionController.canDoubleJump)
   //         {
   //             if (jumpInput)
   //                 _StateMachine.ChangeState(_StateMachine._AirJumpState);
   //         }
   //         else
   //         {
   //             if (jumpInput || rushInput)
   //             {
   //                 _PlayerCharacter._Controls.UseJump();
   //                 _PlayerCharacter._Controls.UseRush();
   //                 _StateMachine.ChangeState(_StateMachine._FallingState);
   //             }
   //         }

        }
    }

    public override void TriggerSideEffect()
    {
        base.TriggerSideEffect();

        if (!activateFirstStep)
        {
            activateFirstStep = true;
            _PlayerCharacter._LocomotionController.Jump(0.189f);
        }
        else
        {
			_PlayerCharacter._LocomotionController.Jump(0.825f);
		}
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
                
        if(!canShmoove && Time.time > _StateEnterTime + 0.18f)
        {
            canShmoove = true;
        }
	}

	public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

		_PlayerCharacter._LocomotionController.MoveWithVerticalVelocity();

        _PlayerCharacter._LocomotionController.ApplyGravity(0.34f);
		_PlayerCharacter._CameraController.ControlCameraRotation(aimInput);
		_PlayerCharacter._LocomotionController.RotateWhileAiming(aimInput);
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		_PlayerCharacter._CameraController.MakeCameraFollowPlayerActor();
	}

	public override void CheckInputs()
	{
		base.CheckInputs();

        aimInput = _PlayerCharacter._Controls._AimInput;
        jumpInput = _PlayerCharacter._Controls._JumpInput;
        rushInput = _PlayerCharacter._Controls._RushInput;
	}

	public override void AnimationFinished()
    {
        base.AnimationFinished();

        _PlayerCharacter._StateMachine.ChangeState(_StateMachine._FallingState);
    }
}
