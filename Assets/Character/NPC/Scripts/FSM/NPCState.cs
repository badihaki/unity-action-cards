using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCState : ScriptableObject
{
    public virtual void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
    {
        _NPC = npc;
        _StateMachine = stateMachine;
        _StateAnimationName = animationName;
    }

    public NonPlayerCharacter _NPC { get; protected set; }
    [field: SerializeField] public string _StateAnimationName { get; protected set; }
    public NPCStateMachine _StateMachine { get; protected set; }
    
    // the time the state starts
    public float _StateEnterTime { get; protected set; }

    // state boolean triggers
    public bool _IsExitingState { get; protected set; }
    public bool _AnimationIsFinished { get; protected set; }

    #region Enter/Exit Functions
    public virtual void EnterState()
    {
        _AnimationIsFinished = false;
        _IsExitingState = false;
        _StateEnterTime = Time.time;
        _NPC._AnimationController.SetBool(_StateAnimationName, true);

        Debug.Log(_NPC.name + " is entering state " + this._StateAnimationName);
    }
    public virtual void ExitState()
    {
        _IsExitingState = true;
        _NPC._AnimationController.SetBool(_StateAnimationName, false);
    }
    #endregion

    #region Update Functions
    public virtual void LogicUpdate()
    {
        if (!_IsExitingState)
        {
            CheckStateTransitions();
        }
    }

    public virtual void PhysicsUpdate()
    {
        //
    }

    #endregion

    #region Checker functions
    public virtual void CheckStateTransitions()
    {
        // 
    }
    #endregion

    // end of the line
}
