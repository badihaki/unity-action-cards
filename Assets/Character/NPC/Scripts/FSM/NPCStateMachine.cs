using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
    [field: SerializeField] public NPCState _CurrentState { get; private set; }
    
    public void InitializeStateMachine(NPCState state)
    {
        _CurrentState = state;
        _CurrentState.EnterState();
    }

    public void ChangeState(NPCState state)
    {
        _CurrentState.ExitState();
        _CurrentState = state;
        _CurrentState.EnterState();
    }
}
