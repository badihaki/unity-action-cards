using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState _CurrentState { get; private set; }

    public void InitializeStateMachine(PlayerState state)
    {
        _CurrentState = state;
        _CurrentState.EnterState();
    }

    public void ChangeState(PlayerState state)
    {
        _CurrentState.ExitState();
        _CurrentState = state;
        _CurrentState.EnterState();
    }
    // end
}
