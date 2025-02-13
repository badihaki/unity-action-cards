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

        if (moveInput != Vector2.zero && !cardInput) _StateMachine.ChangeState(_StateMachine._MoveState);

    }

    public override void PhysicsUpdate()
    {
        if(!_IsExitingState)
        {
            if (_PlayerCharacter._MoveController.targetSpeed > 0.1f) _PlayerCharacter._MoveController.SlowDown();
            else _PlayerCharacter._MoveController.ZeroOutVelocity();

            base.PhysicsUpdate();
            _PlayerCharacter._MoveController.DetectMove(moveInput);
            _PlayerCharacter._MoveController.MoveWithVerticalVelocity();
        }
    }

    public override void EnterState()
    {
        base.EnterState();

        // why did I need to comment this out
        _PlayerCharacter._MoveController.ZeroOutVelocity();
    }
}
