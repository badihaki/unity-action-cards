using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandingState : PlayerState
{
    public PlayerLandingState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    private Vector2 aimInput;

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._MoveController.ZeroOutVelocity();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _PlayerCharacter._MoveController.ApplyGravity(1);
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

        if (_AnimationIsFinished) _StateMachine.ChangeState(_StateMachine._IdleState);
    }

    public override void CheckInputs()
    {
        base.CheckInputs();
        aimInput = _PlayerCharacter._Controls._AimInput;
    }
}
