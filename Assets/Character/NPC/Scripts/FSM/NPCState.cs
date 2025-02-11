using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCState : ScriptableObject
{
    public virtual void InitState(NonPlayerCharacter npc, NPCStateMachine stateMachine, string animationName)
    {
        _NPC = npc;
        _StateMachine = stateMachine;
        _StateAnimationName = animationName;
    }

	[field: SerializeField] public NonPlayerCharacter _NPC { get; protected set; }
    [field: SerializeField] public string _StateAnimationName { get; protected set; }
	[field: SerializeField] public NPCStateMachine _StateMachine { get; protected set; }

	// the time the state starts
	[field: SerializeField] public float _StateEnterTime { get; protected set; }

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

        if (GameManagerMaster.GameMaster.GMSettings.logExtraNPCData)
            Debug.Log(_NPC.name + " is entering state " + name);
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

    // stuff thats checkin for stuff
    #region Checker functions
    public virtual void CheckStateTransitions()
    {
        // 
    }
    #endregion

    // stuff that triggers stuff
    #region Effect Triggers
    public virtual void SideEffectTrigger()
    {
        // side effect trigger
    }

    public virtual void VFXTrigger()
    {
        // trigger for vis FX
    }

    public virtual void SFXTrigger()
    {
        // trigger for sound fx
    }

    public virtual void AnimationEndTrigger()
    {
        //Debug.Log("ending animation");
        // end of animation
        _AnimationIsFinished = true;
    }
	#endregion

	public void GetHurt(responsesToDamage damageResponse)
    {
        switch (damageResponse)
        {
            case responsesToDamage.hit:
                if (!_NPC._CheckGrounded.IsGrounded())
                {
                    _StateMachine.ChangeState(_StateMachine._StateLibrary._AirHitState);
					break;
				}
				_StateMachine.ChangeState(_StateMachine._StateLibrary._HitState);
				break;
            case responsesToDamage.stagger:
				_StateMachine.ChangeState(_StateMachine._StateLibrary._StaggerState);
				break;
			case responsesToDamage.launch:
				_StateMachine.ChangeState(_StateMachine._StateLibrary._LaunchState);
				break;
			case responsesToDamage.knockBack:
				_StateMachine.ChangeState(_StateMachine._StateLibrary._KnockbackState);
				break;
		}
    }

    public virtual void GetHit()
    {
    }

	// end of the line
}
