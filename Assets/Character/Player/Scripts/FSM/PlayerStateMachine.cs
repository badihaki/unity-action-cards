using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState _CurrentState { get; private set; }

    public void InitializeStateMachine(PlayerState state)
    {
        _CurrentState = state;
        Debug.Log("Starting state machine with " + state._StateAnimationName);
        _CurrentState.EnterState();
    }

    public void ChangeState(PlayerState state)
    {
        Debug.Log("Leaving state " + _CurrentState._StateAnimationName + " at " + Time.time);
        _CurrentState.ExitState();
        _CurrentState = state;
        Debug.Log("And enetering new state: " + _CurrentState._StateAnimationName);
        _CurrentState.EnterState();
    }
    // end
}
