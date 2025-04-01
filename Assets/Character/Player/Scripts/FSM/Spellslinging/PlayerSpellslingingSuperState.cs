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
	public bool attackInput { get; private set; }
	public Vector2 moveInput { get; private set; }
	public Vector2 aimInput { get; private set; }
    private Vector3 target;


	public override void EnterState()
    {
        base.EnterState();

        _PlayerCharacter._Controls.SetInputMap(2);

        _PlayerCharacter._MoveController.ZeroOutVelocity();
        _PlayerCharacter._AnimationController.SetBool(_PlayerCharacter._WeaponController._CurrentWeapon._WeaponType.ToString(), false);

        _PlayerCharacter._CameraController.ResetCinemachineTargetTransform();
        _PlayerCharacter._CameraController.SwitchCam(_PlayerCharacter._CameraController._PlayerSpellCamController);

        _PlayerCharacter._UIController.SetShowCrossHair(true);
        
        spellSelectDirection = 0;
        _PlayerCharacter._Controls.ResetSelectSpell();

        _PlayerCharacter._MoveController.RotateInstantly(aimInput);
		target = _PlayerCharacter._PlayerSpells.GetIntendedTarget();
	}

	public override void LogicUpdate()
    {
        base.LogicUpdate();

         target = _PlayerCharacter._PlayerSpells.GetIntendedTarget();

		_PlayerCharacter._PlayerSpells.RotateSpellTarget();
        _PlayerCharacter._UIController.UpdateCrosshairPos(target);

        if (attackInput)
        {
			_PlayerCharacter._PlayerSpells.UseSpell(target);
        }
        if (spellSelectDirection != 0)
        {
            _PlayerCharacter._UIController.ChangeSpell(spellSelectDirection);
            spellSelectDirection = 0;
            _PlayerCharacter._Controls.ResetSelectSpell();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        _PlayerCharacter._MoveController.ApplyGravity(0.15f);
		//_PlayerCharacter._MoveController.MoveWhileAiming(moveInput);
		_PlayerCharacter._MoveController.DetectMove(moveInput);
		_PlayerCharacter._MoveController.RotateCharacter(moveInput);
		_PlayerCharacter._MoveController.MoveWithVerticalVelocity();
		_PlayerCharacter._CameraController.ControlCameraRotation(aimInput * 0.225f, true);
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
        _PlayerCharacter._MoveController.RotateWhileAiming(aimInput);
		_PlayerCharacter._CameraController.MakeCameraFollowPlayerActor();
	}

	public override void ExitState()
    {
        base.ExitState();

        _PlayerCharacter._CameraController.SwitchCam(_PlayerCharacter._CameraController._PlayerCamController);
        _PlayerCharacter._AnimationController.SetBool(_PlayerCharacter._WeaponController._CurrentWeapon._WeaponType.ToString(), true);

		_PlayerCharacter._UIController.SetShowCrossHair(false);

		_PlayerCharacter._PlayerSpells.ResetSpellTargetRotation();
        
        _PlayerCharacter._Controls.SetInputMap(0);
	}

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (!_PlayerCharacter._Controls._SpellslingInput)
        {
            if (_PlayerCharacter._CheckGrounded.IsGrounded())
                _StateMachine.ChangeState(_StateMachine._IdleState);
            else
                _StateMachine.ChangeState(_StateMachine._FallingState);
        }
        if (!_PlayerCharacter._CheckGrounded.IsGrounded())
            _StateMachine.ChangeState(_StateMachine._FallingState);
    }

    public override void CheckInputs()
    {
        base.CheckInputs();

        spellSelectDirection = _PlayerCharacter._Controls._SelectSpellInput;
        moveInput = _PlayerCharacter._Controls._MoveInput;
        aimInput = _PlayerCharacter._Controls._AimInput;
		spellslingInput = _PlayerCharacter._Controls._SpellslingInput;
        attackInput = _PlayerCharacter._Controls._AttackInput;
	}

    // end
}
