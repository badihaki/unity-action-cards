using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedSuperState : PlayerState
{
    public PlayerGroundedSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public Vector2 moveInput { get; private set; }
    public Vector2 aimInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool cardInput { get; private set; }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _PlayerCharacter._CameraController.CameraRotation(aimInput);
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (!_PlayerCharacter._CheckGrounded.IsGrounded()) _StateMachine.ChangeState(_PlayerCharacter._FallingState);
        if (jumpInput) _StateMachine.ChangeState(_PlayerCharacter._JumpState);
        if (cardInput) _StateMachine.ChangeState(_PlayerCharacter._GroundedCardState);
    }
    public override void CheckInputs()
    {
        base.CheckInputs();
        moveInput = _PlayerCharacter._Controls._MoveInput;
        aimInput = _PlayerCharacter._Controls._AimInput;
        jumpInput = _PlayerCharacter._Controls._JumpInput;
        cardInput = _PlayerCharacter._Controls._CardsInput;
    }
}
