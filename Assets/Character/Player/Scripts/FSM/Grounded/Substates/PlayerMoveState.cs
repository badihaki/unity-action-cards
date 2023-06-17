using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedSuperState
{
    public PlayerMoveState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (moveInput == Vector2.zero) _StateMachine.ChangeState(_PlayerCharacter._IdleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        _PlayerCharacter._LocomotionController.MoveTowardsCam(moveInput);
    }
}
