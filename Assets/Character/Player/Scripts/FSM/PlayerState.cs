using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public PlayerState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine)
    {
        _PlayerCharacter = pc;
        _StateAnimationName = animationName;
        _StateMachine = stateMachine;
    }

    // state setup variables
    // ref to player character
    public PlayerCharacter _PlayerCharacter { get; private set; }
    public string _StateAnimationName { get; private set; }
    public PlayerStateMachine _StateMachine { get;private set; }

    // the time the state starts
    public float _StateEnterTime { get; protected set; }
    
    // boolean triggers
    public bool _IsExitingState { get; protected set; }
    public bool _AnimationIsFinished { get; private set; }
    public bool _SideEffectTrigger { get; protected set; }

    #region Enter/Exit Functions
    public virtual void EnterState()
    {
        _StateEnterTime = Time.time;
        _IsExitingState = false;
        _SideEffectTrigger = false;
        _AnimationIsFinished = false;

        // Debug.Log("Entering new state: " + _StateAnimationName + " at " + Time.time);
    }
    public virtual void ExitState()
    {
        _IsExitingState = true;

        // Debug.Log("Leaving state " + _StateAnimationName + " at " + Time.time);
    }
    #endregion

    #region Update Functions
    public virtual void LogicUpdate()
    {
        CheckStateTransitions();
        CheckInputs();
    }
    public virtual void PhysicsUpdate()
    {
        //
    }
    #endregion

    #region MISC Functions
    public virtual void AnimationFinished()
    {
        _AnimationIsFinished = true;
    }
    public virtual void TriggerSideEffect()
    {
        // trigger any side effect logic during the animation
    }
    public virtual void CheckStateTransitions()
    {
        // check for transitions to other states
    }
    public virtual void CheckInputs()
    {
        // use this to hold whatever inputs you want to check for
    }
    #endregion

    // end
}
