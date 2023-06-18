using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedSuperState : PlayerState
{
    public PlayerGroundedSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public Vector2 moveInput { get; private set; }
    public bool jumpInput { get; private set; }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (!_PlayerCharacter._CheckGrounded.IsGrounded()) _StateMachine.ChangeState(_PlayerCharacter._FallingState);
        if (jumpInput) _StateMachine.ChangeState(_PlayerCharacter._JumpState);
    }
    public override void CheckInputs()
    {
        base.CheckInputs();
        moveInput = _PlayerCharacter._Controls._MoveInput;
        jumpInput = _PlayerCharacter._Controls._JumpInput;
    }
}
