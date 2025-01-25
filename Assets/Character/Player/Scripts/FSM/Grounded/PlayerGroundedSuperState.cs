using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedSuperState : PlayerState
{
    public Vector2 moveInput { get; private set; }
    public Vector2 aimInput { get; private set; }
    public bool rushInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool cardInput { get; private set; }
    public bool spellInput { get; private set; }
    public bool attackInput { get; private set; }
    public bool specialInput { get; private set; }
    public bool defenseInput { get; private set; }
    public int spellSelectDirection { get; private set; }
    protected PlayerAttackController _AttackController;
    public PlayerGroundedSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
        _AttackController = pc._AttackController as PlayerAttackController;
    }

	public override void InitializeState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine)
	{
		base.InitializeState(pc, animationName, stateMachine);

		_AttackController = pc._AttackController as PlayerAttackController;
	}

	public override void LogicUpdate()
    {
        base.LogicUpdate();

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
        if (!cardInput)
        {
            _PlayerCharacter._CameraController.ControlCameraRotation(aimInput);
        }
        _PlayerCharacter._LocomotionController.ApplyGravity(1);
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();
        if (!cardInput)
        {
            _PlayerCharacter._PlayerCards.PutAwayHand();
            if (!_PlayerCharacter._CheckGrounded.IsGrounded()) _StateMachine.ChangeState(_StateMachine._FallingState); // if not grounded, fall
            if (jumpInput) _StateMachine.ChangeState(_StateMachine._JumpState); // if jump input is detected, go to jump
            if (spellInput) _StateMachine.ChangeState(_StateMachine._SpellslingState); // if spell input detected, go to spell
            if (defenseInput) _StateMachine.ChangeState(_AttackController._DefenseAction); // if def input detected, go to def

            // attacks
            if (rushInput)
            {
                if(_PlayerCharacter._LocomotionController.movementSpeed > 10.0f)
                {
                    if (attackInput || specialInput)
                    {
                        _PlayerCharacter._Controls.UseAttack();
                        _PlayerCharacter._Controls.UseSpecialAttack();
                        _StateMachine.ChangeState(_AttackController._RushAttack);
                    }
                }
                else
                {
                    if (attackInput)
                    {
                        _PlayerCharacter._Controls.UseAttack();
                        _StateMachine.ChangeState(_AttackController._AttackA);
                    }
                    if (specialInput)
                    {
                        _PlayerCharacter._Controls.UseSpecialAttack();
                        _StateMachine.ChangeState(_AttackController._Special);
                    }
                }
            }
            else
            {
                if (attackInput)
                {
                    _PlayerCharacter._Controls.UseAttack();
                    _StateMachine.ChangeState(_AttackController._AttackA);
                }
                if (specialInput)
                {
                    _PlayerCharacter._Controls.UseSpecialAttack();
                    _StateMachine.ChangeState(_AttackController._Special);
                }
            }
        }
        else
        {
            _PlayerCharacter._PlayerCards.ShowHand();
        }
    }
    public override void CheckInputs()
    {
        base.CheckInputs();
        moveInput = _PlayerCharacter._Controls._MoveInput;
        aimInput = _PlayerCharacter._Controls._AimInput;
        rushInput = _PlayerCharacter._Controls._RushInput;
        jumpInput = _PlayerCharacter._Controls._JumpInput;
        cardInput = _PlayerCharacter._Controls._CardsInput;
        spellInput = _PlayerCharacter._Controls._SpellslingInput;
        attackInput = _PlayerCharacter._Controls._AttackInput;
        specialInput = _PlayerCharacter._Controls._SpecialAttackInput;
        defenseInput = _PlayerCharacter._Controls._DefenseInput;
        spellSelectDirection = _PlayerCharacter._Controls._SelectSpellInput;
    }
}
