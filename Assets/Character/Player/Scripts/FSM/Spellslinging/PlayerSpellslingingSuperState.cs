using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellslingingSuperState : PlayerState
{
    public PlayerSpellslingingSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }
	public int spellSelectDirection { get; protected set; }
	public bool spellslingInput { get; private set; }
	public Vector2 aimInput { get; private set; }


	public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._LocomotionController.ZeroOutVelocity();
        _PlayerCharacter._AnimationController.SetBool(_PlayerCharacter._WeaponController._CurrentWeapon._WeaponType.ToString(), false);

        _PlayerCharacter._CameraController.ResetCinemachineTargetTransform();
        _PlayerCharacter._CameraController.SwitchCam(_PlayerCharacter._CameraController._PlayerSpellCamController);
        
        AttemptShootSpell();
        
        spellSelectDirection = 0;
        _PlayerCharacter._Controls.ResetSelectSpell();
    }

    protected void AttemptShootSpell()
    {
        Vector3 target = _PlayerCharacter._PlayerSpells.DetectRangedTargets();
        //_PlayerCharacter.LogFromState(target == Vector3.zero ? "No target || playerSpellslingingState" : $"Target found!! >>{target} || playerSpellslingingState");
        _PlayerCharacter._Controls.UseSpell();
        _PlayerCharacter._PlayerSpells.UseSpell(target);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _PlayerCharacter._CameraController.ControlCameraRotation(aimInput * 0.25f);
		_PlayerCharacter._CameraController.MakeCameraFollowPlayerActor();

		_PlayerCharacter._PlayerSpells.RotateSpellTarget();

        if (spellslingInput)
        {
            AttemptShootSpell();
        }
        if(spellSelectDirection != 0)
        {
            _PlayerCharacter._PlayerSpells.ChangeSpell(spellSelectDirection);
            spellSelectDirection = 0;
            _PlayerCharacter._Controls.ResetSelectSpell();
        }
        if(spellSelectDirection != 0)
        {
            _PlayerCharacter._Controls.ResetSelectSpell();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        _PlayerCharacter._LocomotionController.ApplyGravity(0.15f);
        _PlayerCharacter._LocomotionController.SlowDown();
    }

	public override void LateUpdate()
	{
		base.LateUpdate();
        _PlayerCharacter._LocomotionController.RotateWhileAiming(aimInput);
	}

	public override void ExitState()
    {
        base.ExitState();

        _PlayerCharacter._CameraController.SwitchCam(_PlayerCharacter._CameraController._PlayerCamController);
        _PlayerCharacter._AnimationController.SetBool(_PlayerCharacter._WeaponController._CurrentWeapon._WeaponType.ToString(), true);
        // _PlayerCharacter._PlayerSpells.HideCrosshair();
        _PlayerCharacter._PlayerSpells.ResetSpellTargetRotation();
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
        aimInput = _PlayerCharacter._Controls._AimInput;
		spellslingInput = _PlayerCharacter._Controls._SpellslingInput;
	}

    // end
}
