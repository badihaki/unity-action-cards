using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatSuperState : PlayerState
{
    public PlayerCombatSuperState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine) : base(pc, animationName, stateMachine)
    {
    }

    public Vector2 moveInput { get; private set; }
    public Vector2 aimInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool attackInput { get; private set; }
    public bool specialInput { get; private set; }
    public bool spellslingInput { get; private set; }
    public bool defenseInput { get; private set; }

    public override void EnterState()
    {
        base.EnterState();
        // _PlayerCharacter._AttackController.SetRootMotion(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        // _PlayerCharacter._AttackController.SetRootMotion(false);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        // _PlayerCharacter._PlayerActor.ApplyRootMotion();
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
    }

    protected void ShowOrHideWeapon(bool showWeapon)
    {
        if (showWeapon)
        {
            _PlayerCharacter._AttackController._WeaponR?.SetActive(true);
            _PlayerCharacter._AttackController._WeaponL?.SetActive(true);
        }
        else
        {
            _PlayerCharacter._AttackController._WeaponR?.SetActive(false);
            _PlayerCharacter._AttackController._WeaponL?.SetActive(false);
        }
    }
}
