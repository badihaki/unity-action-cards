using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirSuperState : PlayerState
{
    public PlayerInAirSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public Vector2 moveInput { get; private set; }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _PlayerCharacter._LocomotionController.ApplyGravity();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (_PlayerCharacter._CheckGrounded.IsGrounded()) _StateMachine.ChangeState(_PlayerCharacter._IdleState);
    }

    public override void CheckInputs()
    {
        base.CheckInputs();

        moveInput = _PlayerCharacter._Controls._MoveInput;
    }
}
