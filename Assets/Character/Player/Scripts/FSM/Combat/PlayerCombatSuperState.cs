using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatSuperState : PlayerState
{
	protected PlayerAttackController _AttackController;
	public PlayerCombatSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }
	public override void InitializeState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine)
	{
		base.InitializeState(pc, animationName, stateMachine);

		_AttackController = pc._AttackController as PlayerAttackController;
	}

    public Vector2 moveInput { get; private set; }
    public Vector2 aimInput { get; private set; }
    public int spellSelectDirection { get; protected set; }
    public bool jumpInput { get; private set; }
    public bool attackInput { get; private set; }
    public bool specialInput { get; private set; }
    public bool spellslingInput { get; private set; }
    public bool defenseInput { get; private set; }

    protected bool canCombo;

    public override void EnterState()
    {
        base.EnterState();

        canCombo = false;
        _PlayerCharacter._MoveController.RotateInstantly(moveInput);
    }

    public override void ExitState()
    {
        base.ExitState();

        canCombo = false;
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();
        if (_PlayerCharacter._CheckGrounded.IsGrounded())
        {
            if (_AnimationIsFinished) _StateMachine.ChangeState(_StateMachine._IdleState);
        }
        else
        {
            if (_AnimationIsFinished) _StateMachine.ChangeState(_StateMachine._FallingState);
        }
        if (Time.time > _StateEnterTime + 1.5f) _StateMachine.ChangeState(_StateMachine._IdleState);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (spellSelectDirection != 0)
        {
            _PlayerCharacter._UIController.ChangeSpell(spellSelectDirection);
            spellSelectDirection = 0;
            _PlayerCharacter._Controls.ResetSelectSpell();
        }

        _PlayerCharacter._CameraController.MakeCameraFollowPlayerActor();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        _PlayerCharacter._CameraController.ControlCameraRotation(aimInput);
    }

	public override void CheckInputs()
    {
        base.CheckInputs();
        moveInput = _PlayerCharacter._Controls._MoveInput;
        aimInput = _PlayerCharacter._Controls._AimInput;
        jumpInput = _PlayerCharacter._Controls._JumpInput;
        attackInput = _PlayerCharacter._Controls._AttackInput;
        specialInput = _PlayerCharacter._Controls._SpecialAttackInput;
        spellslingInput = _PlayerCharacter._Controls._SpellslingInput;
        defenseInput = _PlayerCharacter._Controls._DefenseInput;
        spellSelectDirection = _PlayerCharacter._Controls._SelectSpellInput;
    }

    public override void TriggerSideEffect()
    {
        base.TriggerSideEffect();

        canCombo = true;
    }

	public override void TriggerVisualEffect()
	{
		base.TriggerVisualEffect();

        if(_PlayerCharacter._WeaponController._CurrentWeapon._WeaponAttackFX != null)
        {
            _PlayerCharacter._WeaponController.PlayWeaponVFX();

		}
	}

	protected void ShowOrHideWeapon(bool showWeapon)
    {
        if (showWeapon)
        {
            _PlayerCharacter._WeaponController._WeaponR?.SetActive(true);
            _PlayerCharacter._WeaponController._WeaponL?.SetActive(true);
        }
        else
        {
            _PlayerCharacter._WeaponController._WeaponR?.SetActive(false);
            _PlayerCharacter._WeaponController._WeaponL?.SetActive(false);
        }
    }
}
