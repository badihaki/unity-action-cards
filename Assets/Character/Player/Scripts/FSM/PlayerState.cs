using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public PlayerState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine)
    {
        _PC = pc;
        _StateAnimationName = animationName;
        _StateMachine = stateMachine;
    }

    // state setup variables
    // ref to player character
    public PlayerCharacter _PC { get; private set; }
    public string _StateAnimationName { get; private set; }
    public PlayerStateMachine _StateMachine { get;private set; }

    // the time the state starts
    public float _StateEnterTime { get; protected set; }
    
    // boolean triggers
    public bool _IsExitingState { get; protected set; }
    public bool _SideEffectTrigger { get; protected set; }

    #region Enter/Exit Functions
    public virtual void EnterState()
    {
        _StateEnterTime = Time.time;
    }
    public virtual void ExitState()
    {
        _IsExitingState = true;
    }
    #endregion

    #region Update Functions
    public virtual void LogicUpdate()
    {
        // check state transitions
    }
    public virtual void PhysicsUpdate()
    {
        //
    }
    #endregion

    #region MISC Functions
    public virtual void TriggerSideEffect()
    {
        // trigger any side effect logic during the animation
    }
    public virtual void CheckStateTransitions()
    {
        // check for transitions to other states
    }
    #endregion

    // end
}
