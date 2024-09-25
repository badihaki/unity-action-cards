using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedSuperState
{
    public PlayerIdleState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (moveInput != Vector2.zero && !cardInput) _StateMachine.ChangeState(_PlayerCharacter._MoveState);
    }

    public override void PhysicsUpdate()
    {
        if(!_IsExitingState)
        {
            if (_PlayerCharacter._LocomotionController.movementSpeed > 0.05f) _PlayerCharacter._LocomotionController.SlowDown();
            else _PlayerCharacter._LocomotionController.ZeroOutVelocity();

            base.PhysicsUpdate();
            _PlayerCharacter._LocomotionController.DetectMove(moveInput);
            _PlayerCharacter._LocomotionController.MoveWithVerticalVelocity();
        }
    }

    public override void EnterState()
    {
        base.EnterState();

        // why did I need to comment this out
        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
    }
}
