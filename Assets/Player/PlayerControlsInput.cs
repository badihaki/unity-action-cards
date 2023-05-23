using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlsInput : MonoBehaviour
{
    private Player Player;
    [field:SerializeField]public Vector2 moveInput { get; private set; }
    [field:SerializeField]public Vector2 aimInput { get; private set; }
    [field: SerializeField] public bool jump { get; private set; }
    [field: SerializeField] public bool run { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(Player controller)
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
        moveInput = moveVector;
    }
    public void OnAim(InputValue val)
    {
        ProcessAimInput(val.Get<Vector2>());
    }
    private void ProcessAimInput(Vector2 input)
    {
        aimInput = input.normalized;
    }

    public void OnJump(InputValue val)
    {
        ProcessJumpInput(val.isPressed);
    }
    private void ProcessJumpInput(bool inputState)
    {
        jump = inputState;
    }

    public void OnRun(InputValue val)
    {
        ProcessRunInput(val.isPressed);
    }

    private void ProcessRunInput(bool inputState)
    {
        run = inputState;
    }

    // end
}
