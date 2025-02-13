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
        if (_PlayerCharacter._MoveController.movementSpeed <= 0.1f && moveInput == Vector2.zero || cardInput) _StateMachine.ChangeState(_StateMachine._IdleState);

        base.CheckStateTransitions();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!_IsExitingState)
        {
            if (moveInput == Vector2.zero)
                _PlayerCharacter._MoveController.SlowDown();
            else
            {
                _PlayerCharacter._MoveController.DetectMove(moveInput);
                _PlayerCharacter._MoveController.RotateCharacter(moveInput);
                _PlayerCharacter._MoveController.MoveWithVerticalVelocity();
            }
        }
    }
}
