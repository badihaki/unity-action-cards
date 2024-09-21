using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReadySpellState : PlayerCombatSuperState
{
    public PlayerReadySpellState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    bool interactionInput;

    public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._CameraController.ResetCinemachineTargetTransform();
        // _PlayerCharacter._CameraController.SwitchCam(_PlayerCharacter._CameraController._PlayerAimCamController);
        _PlayerCharacter._AnimationController.SetBool(_PlayerCharacter._AttackController._CurrentWeapon._WeaponType.ToString(), false);
        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
        // _PlayerCharacter._PlayerSpells.ShowCrosshair();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _PlayerCharacter._CameraController.ControlCameraRotation(aimInput);
        _PlayerCharacter._PlayerSpells.UpdateCrosshair();

        if (attackInput)_PlayerCharacter._PlayerSpells.UseSpell();
        if (interactionInput)
        {
            _PlayerCharacter._Controls.UseInteract();
            _PlayerCharacter._PlayerSpells.ChangeSpellIndex();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        _PlayerCharacter._LocomotionController.ApplyGravity(1);
        // _PlayerCharacter._LocomotionController.RotateCharacterWhileAiming(moveInput); // for some reason I was aiming in accordance to where I was moving. This may be wrong

        /*
         */
        if(moveInput != Vector2.zero)
        {
            _PlayerCharacter._LocomotionController.DetectMove(moveInput);
            // _PlayerCharacter._LocomotionController.MoveWhileAiming(); // change this to move based on moveinput
        }
        else
        {
            _PlayerCharacter._LocomotionController.ZeroOutVelocity();
        }
        // _PlayerCharacter._LocomotionController.ZeroOutVelocity();
    }

    public override void ExitState()
    {
        base.ExitState();

        _PlayerCharacter._CameraController.SwitchCam(_PlayerCharacter._CameraController._PlayerCamController);
        _PlayerCharacter._AnimationController.SetBool(_PlayerCharacter._AttackController._CurrentWeapon._WeaponType.ToString(), true);
        _PlayerCharacter._PlayerSpells.HideCrosshair();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (!readySpellInput)
        {
            if (_PlayerCharacter._CheckGrounded.IsGrounded()) _StateMachine.ChangeState(_PlayerCharacter._IdleState);
            else _StateMachine.ChangeState(_PlayerCharacter._FallingState);
        }
    }

    public override void CheckInputs()
    {
        base.CheckInputs();

        interactionInput = _PlayerCharacter._Controls._InteractInput;
    }

    // end
}
