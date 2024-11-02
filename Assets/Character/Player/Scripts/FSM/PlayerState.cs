using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerState : ScriptableObject
{
    public PlayerState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine)
    {
        _PlayerCharacter = pc;
        _StateAnimationName = animationName;
        _StateMachine = stateMachine;
    }
    public virtual void InitializeState(PlayerCharacter pc, string animationName, PlayerStateMachine stateMachine)
    {
        _PlayerCharacter = pc;
        _StateAnimationName = animationName;
        _StateMachine = stateMachine;
    }

    // state setup variables
    // ref to player character
    public PlayerCharacter _PlayerCharacter { get; protected set; }
    [field: SerializeField] public string _StateAnimationName { get; protected set; }
    public PlayerStateMachine _StateMachine { get; protected set; }

    // the time the state starts
    public float _StateEnterTime { get; protected set; }
    
    // boolean triggers
    public bool _IsExitingState { get; protected set; }
    public bool _AnimationIsFinished { get; private set; }
    [field: SerializeField] public bool _SideEffectTrigger { get; protected set; }

    #region Enter/Exit Functions
    public virtual void EnterState()
    {
        _StateEnterTime = Time.time;
        _IsExitingState = false;
        _SideEffectTrigger = false;
        _AnimationIsFinished = false;
        _PlayerCharacter._AnimationController.SetBool(_StateAnimationName, true);

        // Debug.Log("Entering new state: " + _StateAnimationName + " at " + Time.time);
    }
    public virtual void ExitState()
    {
        _IsExitingState = true;
        _PlayerCharacter._AnimationController.SetBool(_StateAnimationName, false);

        // Debug.Log("Leaving state " + _StateAnimationName + " at " + Time.time);
    }
    #endregion

    #region Update Functions
    public virtual void LogicUpdate()
    {
        if (!_IsExitingState)
        {
            CheckStateTransitions();
        }
        CheckInputs();
    }
    public virtual void PhysicsUpdate()
    {
        //
    }
    public virtual void LateUpdate()
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
    public virtual void TriggerVisualEffect()
    {
        // triiger any visual effects we need to trigger
    }
    public virtual void TriggerSoundEffect()
    {
        // trigger any sound effects we may need
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
