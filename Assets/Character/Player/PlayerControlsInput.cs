using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlsInput : MonoBehaviour
{
    private PlayerCharacter Player;
    [field:SerializeField]public Vector2 _MoveInput { get; private set; }
    [field:SerializeField]public Vector2 _AimInput { get; private set; }
    [field: SerializeField] public bool _Jump { get; private set; }
    [field: SerializeField] public bool _Run { get; private set; }
    [field: SerializeField] public bool _Cards { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(PlayerCharacter controller)
    {
        Player = controller;
    }

    public void OnMove(InputValue val)
    {
        ProcessMoveInput(val.Get<Vector2>());
    }
    private void ProcessMoveInput(Vector2 input)
    {
        Vector2 moveVector = input;
        if(moveVector.x > 0) moveVector.x = 1;
        else if(moveVector.x < 0) moveVector.x = -1;
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

    public void OnJump(InputValue val)
    {
        ProcessJumpInput(val.isPressed);
    }
    private void ProcessJumpInput(bool inputState)
    {
        _Jump = inputState;
    }

    public void OnRun(InputValue val)
    {
        ProcessRunInput(val.isPressed);
    }

    private void ProcessRunInput(bool inputState)
    {
        _Run = inputState;
    }
    public void OnCards(InputValue val)
    {
        ProcessCards(val.isPressed);
    }
    private void ProcessCards(bool inputState)
    {
        _Cards = inputState;
    }

    // end
}
