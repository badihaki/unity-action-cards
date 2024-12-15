using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlsInput : MonoBehaviour
{
    [field: SerializeField] public Vector2 _MoveInput { get; private set; }
    [field: SerializeField] public Vector2 _AimInput { get; private set; }
    [field: SerializeField] public bool _LockOnInput { get; private set; }
    [field: SerializeField] public bool _JumpInput { get; private set; }
    [field: SerializeField] public bool _RushInput { get; private set; }
    [field: SerializeField] public bool _CardsInput { get; private set; }
    [field: SerializeField] public bool _SpellslingInput { get; private set; }
    [field: SerializeField] public bool _AttackInput { get; private set; }
    [field: SerializeField] public bool _SpecialAttackInput { get; private set; }
    [field: SerializeField] public bool _InteractInput { get; private set; }
    [field: SerializeField] public bool _DefenseInput { get; private set; }
    [field: SerializeField] public int _SelectSpellInput { get; private set; }


    public void OnMove(InputValue val)
    {
        ProcessMoveInput(val.Get<Vector2>());
    }
    private void ProcessMoveInput(Vector2 input)
    {
        Vector2 moveVector = input;
        if (moveVector.x > 0) moveVector.x = 1;
        else if (moveVector.x < 0) moveVector.x = -1;
        if (moveVector.y > 0) moveVector.y = 1;
        else if (moveVector.y < 0) moveVector.y = -1;
        _MoveInput = moveVector;
    }
    public void OnAim(InputValue val)
    {
        ProcessAimInput(val.Get<Vector2>());
    }
    private void ProcessAimInput(Vector2 input)
    {
        _AimInput = input.normalized;
    }

    public void OnLockOn(InputValue val)
    {
        ProcessLockOnInput(val.isPressed);
    }
    private void ProcessLockOnInput(bool inputState) => _LockOnInput = inputState;

    public void OnJump(InputValue val)
    {
        ProcessJumpInput(val.isPressed);
    }
    private void ProcessJumpInput(bool inputState)
    {
        _JumpInput = inputState;
    }
    public void UseJump() => _JumpInput = false;

    public void OnRush(InputValue val)
    {
        ProcessRushInput(val.isPressed);
    }

    private void ProcessRushInput(bool inputState)
    {
        _RushInput = inputState;
    }
    public void UseRush() => _RushInput = false;
    public void OnCards(InputValue val)
    {
        ProcessCards(val.isPressed);
    }
    private void ProcessCards(bool inputState)
    {
        _CardsInput = inputState;
    }
    public void CardSelected() => _CardsInput = false;

    public void OnAttack(InputValue val)
    {
        ProcessAttack(val.isPressed);
    }
    private void ProcessAttack(bool inputState)
    {
        _AttackInput = inputState;
    }
    public void UseAttack() => _AttackInput = false;
    public void OnSpecialAttack(InputValue val)
    {
        ProcessSpecial(val.isPressed);
    }
    private void ProcessSpecial(bool inputState)
    {
        _SpecialAttackInput = inputState;
        if (_SpecialAttackInput) _AttackInput = false;
    }
    public void UseSpecialAttack() => _SpecialAttackInput = false;

    public void OnSpell(InputValue val)
    {
        ProcessSpellsling(val.isPressed);
    }
    private void ProcessSpellsling(bool inputState)
    {
        _SpellslingInput = inputState;
    }
    public void UseSpell() => _SpellslingInput = false;
    
    public void OnInteract(InputValue val)
    {
        ProcessInteract(val.isPressed);
    }
    private void ProcessInteract(bool inputState)
    {
        _InteractInput = inputState;
    }
    public void UseInteract() => _InteractInput = false;

    public void OnDefenseAction(InputValue val)
    {
        ProcessDefense(val.isPressed);
    }
    private void ProcessDefense(bool inputState)
    {
        _DefenseInput = inputState;
    }
    public void UseDefense()
    {
        _DefenseInput = false;
    }

    public void OnSelectSpell(InputValue val)
    {
        float value = val.Get<float>();
        _SelectSpellInput = value > 0 ? 1 : -1;
    }
    public void ResetSelectSpell() => _SelectSpellInput = 0;

    public void OnQuit(InputValue val)
    {
        if (val.isPressed)
            GameManagerMaster.GameMaster.QuitGame();
    }

    // end
}
