using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedSuperState
{
    public PlayerMoveState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _PlayerCharacter._AnimationController.SetBool(_PlayerCharacter._AttackController._CurrentWeapon._WeaponType.ToString(), false);
    }

    public override void ExitState()
    {
        base.ExitState();
        _PlayerCharacter._AnimationController.SetBool(_PlayerCharacter._AttackController._CurrentWeapon._WeaponType.ToString(), true);
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        /*if (moveInput == Vector2.zero) _StateMachine.ChangeState(_PlayerCharacter._IdleState);*/
        if(_PlayerCharacter._LocomotionController.movementSpeed < 0.1f)
        {
            _StateMachine.ChangeState(_PlayerCharacter._IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (moveInput != Vector2.zero)
            _PlayerCharacter._LocomotionController.MoveTowardsCam(moveInput);
        else
            _PlayerCharacter._LocomotionController.SlowDown();
    }
}
