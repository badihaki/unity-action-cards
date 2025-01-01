using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirSuperState : PlayerState
{
    public PlayerInAirSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public Vector2 moveInput { get; private set; }
    public Vector2 aimInput { get; private set; }
    public int spellSelectDirection { get; private set; }
    public bool attackInput { get; private set; }
    public bool specialInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool rushInput { get; private set; }
    protected PlayerAttackController _AttackController;

	public override void InitializeState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine)
	{
		base.InitializeState(pc, animationName, stateMachine);

        _AttackController = pc._AttackController as PlayerAttackController;
	}

	public override void LogicUpdate()
    {
        base.LogicUpdate();

        _PlayerCharacter._CameraController.ControlCameraRotation(aimInput);
        if (spellSelectDirection != 0)
        {
            _PlayerCharacter._PlayerUIController.ChangeSpell(spellSelectDirection);
            spellSelectDirection = 0;
            _PlayerCharacter._Controls.ResetSelectSpell();
        }

		_PlayerCharacter._CameraController.MakeCameraFollowPlayerActor();
	}

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (moveInput != Vector2.zero)
        {
            _PlayerCharacter._LocomotionController.DetectMove(moveInput);
            _PlayerCharacter._LocomotionController.RotateCharacter(moveInput);
        }
        _PlayerCharacter._LocomotionController.ApplyGravity();
        _PlayerCharacter._LocomotionController.MoveWithVerticalVelocity();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();

        if (_PlayerCharacter._CheckGrounded.IsGrounded())
        {
            _StateMachine.ChangeState(_StateMachine._IdleState);
			_PlayerCharacter._LocomotionController.ResetAllAirSchmoovement();
		}
        if (attackInput)
        {
            _PlayerCharacter.LogFromState("attackin");
            _PlayerCharacter._Controls.UseAttack();
            _StateMachine.ChangeState(_AttackController._AirAttackA);
        }
        if (specialInput)
        {
            _StateMachine.ChangeState(_AttackController._AirSpecial);
            _PlayerCharacter._Controls.UseSpecialAttack();
        }
    }

    public override void CheckInputs()
    {
        base.CheckInputs();

        moveInput = _PlayerCharacter._Controls._MoveInput;
        aimInput = _PlayerCharacter._Controls._AimInput;
        spellSelectDirection = _PlayerCharacter._Controls._SelectSpellInput;
        attackInput = _PlayerCharacter._Controls._AttackInput;
        specialInput = _PlayerCharacter._Controls._SpecialAttackInput;
        jumpInput = _PlayerCharacter._Controls._JumpInput;
        rushInput = _PlayerCharacter._Controls._RushInput;
    }
}
