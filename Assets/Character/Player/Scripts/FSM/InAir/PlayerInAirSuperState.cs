using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirSuperState : PlayerState
{
    public PlayerInAirSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public Vector2 moveInput { get; private set; }
    public Vector2 aimInput { get; private set; }
    public bool cardInput { get; private set; }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _PlayerCharacter._CameraController.ControlCameraRotation(aimInput);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (moveInput != Vector2.zero)
            _PlayerCharacter._LocomotionController.DetectMove(moveInput);
        _PlayerCharacter._LocomotionController.ApplyGravity(1);
        _PlayerCharacter._LocomotionController.MoveWithVerticalVelocity();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (_PlayerCharacter._CheckGrounded.IsGrounded()) _StateMachine.ChangeState(_PlayerCharacter._IdleState);
        // if (cardInput) _StateMachine.ChangeState(_PlayerCharacter._InAirCardState);
    }

    public override void CheckInputs()
    {
        base.CheckInputs();

        moveInput = _PlayerCharacter._Controls._MoveInput;
        aimInput = _PlayerCharacter._Controls._AimInput;
        cardInput = _PlayerCharacter._Controls._CardsInput;
    }
}
