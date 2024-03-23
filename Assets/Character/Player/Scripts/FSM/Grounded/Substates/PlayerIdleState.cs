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

        if (moveInput != Vector2.zero) _StateMachine.ChangeState(_PlayerCharacter._MoveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        _PlayerCharacter._LocomotionController.ApplyGravity();
        if (_PlayerCharacter._LocomotionController.movementSpeed > 0.15f) _PlayerCharacter._LocomotionController.SlowDown();
    }

    public override void EnterState()
    {
        base.EnterState();

        /*_PlayerCharacter._LocomotionController.ZeroOutVelocity();*/
    }
}
