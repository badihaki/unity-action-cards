using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
    [field: SerializeField] public NPCState _CurrentState { get; private set; }
    private bool _Ready = false;
    
    public void InitializeStateMachine(NPCState state)
    {
        _CurrentState = state;
        _CurrentState.EnterState();
        _Ready = true;
    }

    public void ChangeState(NPCState state)
    {
        _CurrentState.ExitState();
        _CurrentState = state;
        _CurrentState.EnterState();
    }

    public void Update()
    {
        if (_Ready && GameManagerMaster.GameMaster)
        {
            _CurrentState.LogicUpdate();
        }
    }

    public void FixedUpdate()
    {
        if (_Ready && GameManagerMaster.GameMaster)
        {
            _CurrentState.PhysicsUpdate();
        }
    }

    public void LogFromState(string input)
    {
        print($"{input} >>> :: <<< Logged from {_CurrentState.ToString()}");
    }
}
