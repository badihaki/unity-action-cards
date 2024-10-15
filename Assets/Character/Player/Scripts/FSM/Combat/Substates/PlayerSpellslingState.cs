using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellslingState : PlayerCombatSuperState
{
    public PlayerSpellslingState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        // _PlayerCharacter._CameraController.ResetCinemachineTargetTransform();
        // _PlayerCharacter._CameraController.SwitchCam(_PlayerCharacter._CameraController._PlayerAimCamController);
        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
        _PlayerCharacter._AnimationController.SetBool(_PlayerCharacter._AttackController._CurrentWeapon._WeaponType.ToString(), false);
        AttemptShootSpell();
        spellSelectDirection = 0;
        _PlayerCharacter._Controls.ResetSelectSpell();
        // _PlayerCharacter._PlayerSpells.ShowCrosshair();
    }

    protected void AttemptShootSpell()
    {
        Vector3 target = _PlayerCharacter._PlayerSpells.DetectRangedTargets();
        _PlayerCharacter.LogFromState(target == Vector3.zero ? "No target" : $"Target found!! >>{target}");
        if (target == Vector3.zero) target = _PlayerCharacter._CameraController._Camera.transform.forward;
        _PlayerCharacter._Controls.UseSpell();
        _PlayerCharacter._PlayerSpells.UseSpell(target);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _PlayerCharacter._CameraController.ControlCameraRotation(aimInput);
        // _PlayerCharacter._PlayerSpells.UpdateCrosshair();

        if (spellslingInput)
        {
            /*Vector3 target = _PlayerCharacter._PlayerSpells.DetectRangedTargets();
            _PlayerCharacter._Controls.UseSpell();
            _PlayerCharacter._PlayerSpells.UseSpell(target);*/
            AttemptShootSpell();
        }
        if(spellSelectDirection != 0)
        {
            _PlayerCharacter._PlayerSpells.ChangeSpell(spellSelectDirection);
            spellSelectDirection = 0;
            _PlayerCharacter._Controls.ResetSelectSpell();
        }
        /*
        if (interactionInput)
        {
            _PlayerCharacter._Controls.UseInteract();
            _PlayerCharacter._PlayerSpells.ChangeSpellIndex();
        }
         */
        if(spellSelectDirection != 0)
        {
            _PlayerCharacter._Controls.ResetSelectSpell();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        _PlayerCharacter._LocomotionController.ApplyGravity(0.15f);
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
        // _PlayerCharacter._PlayerSpells.HideCrosshair();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (_AnimationIsFinished)
        {
            if (_PlayerCharacter._CheckGrounded.IsGrounded()) _StateMachine.ChangeState(_StateMachine._IdleState);
            else _StateMachine.ChangeState(_StateMachine._FallingState);
        }
    }

    public override void CheckInputs()
    {
        base.CheckInputs();

        spellSelectDirection = _PlayerCharacter._Controls._SelectSpellInput;
    }

    // end
}
