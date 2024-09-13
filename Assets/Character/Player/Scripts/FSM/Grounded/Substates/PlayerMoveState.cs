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
        // _PlayerCharacter._AnimationController.SetBool(_PlayerCharacter._AttackController._CurrentWeapon._WeaponType.ToString(), false);  // why we doin this/?
    }

    public override void ExitState()
    {
        base.ExitState();
        // _PlayerCharacter._AnimationController.SetBool(_PlayerCharacter._AttackController._CurrentWeapon._WeaponType.ToString(), true);  // why we do this??
    }

    public override void CheckStateTransitions()
    {
        if (_PlayerCharacter._LocomotionController.movementSpeed <= 0.1f || cardInput) _StateMachine.ChangeState(_PlayerCharacter._IdleState);

        base.CheckStateTransitions();
    }

    public override void PhysicsUpdate()
    {
        if (moveInput == Vector2.zero)
            _PlayerCharacter._LocomotionController.SlowDown();

        base.PhysicsUpdate();
    }
}
