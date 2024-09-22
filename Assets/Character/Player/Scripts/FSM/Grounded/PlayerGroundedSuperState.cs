using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedSuperState : PlayerState
{
    public PlayerGroundedSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public Vector2 moveInput { get; private set; }
    public Vector2 aimInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool cardInput { get; private set; }
    public bool spellInput { get; private set; }
    public bool attackInput { get; private set; }
    public bool defenseInput { get; private set; }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!cardInput)
        {
            _PlayerCharacter._CameraController.ControlCameraRotation(aimInput);
        }
    }

    public override void PhysicsUpdate()
    {
        _PlayerCharacter._LocomotionController.ApplyGravity(1);
        _PlayerCharacter._LocomotionController.DetectMove(moveInput);
        _PlayerCharacter._LocomotionController.RotateCharacter(moveInput);
        _PlayerCharacter._LocomotionController.MoveWithVerticalVelocity();
    }

    public override void CheckStateTransitions()
    {
        base.CheckStateTransitions();
        if (!cardInput)
        {
            _PlayerCharacter._PlayerCards.PutAwayHand();
            if (!_PlayerCharacter._CheckGrounded.IsGrounded()) _StateMachine.ChangeState(_PlayerCharacter._FallingState);
            if (jumpInput) _StateMachine.ChangeState(_PlayerCharacter._JumpState);
            // if (cardInput) _StateMachine.ChangeState(_PlayerCharacter._GroundedCardState);
            if (spellInput) _StateMachine.ChangeState(_PlayerCharacter._SpellslingState);
            if (attackInput)
            {
                _PlayerCharacter._Controls.UseAttack();
                _StateMachine.ChangeState(_PlayerCharacter._AttackController._AttackA);
            }
            if (defenseInput) _StateMachine.ChangeState(_PlayerCharacter._AttackController._DefenseAction);
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
        jumpInput = _PlayerCharacter._Controls._JumpInput;
        cardInput = _PlayerCharacter._Controls._CardsInput;
        spellInput = _PlayerCharacter._Controls._SpellslingInput;
        attackInput = _PlayerCharacter._Controls._AttackInput;
        defenseInput = _PlayerCharacter._Controls._DefenseInput;
    }
}
