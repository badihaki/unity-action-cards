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

	// stuff that manages state response to getting hit
	#region Hurt functions
	public virtual void GetHurt(responsesToDamage damageResponse)
    {
        NPCState nextState = _StateMachine._StateLibrary._AirHitState;
		if (GameManagerMaster.GameMaster.GMSettings.logNPCCombat)
			_StateMachine.LogFromState($"{_NPC.name} is recieving the damage response {damageResponse}");
		switch (damageResponse)
        {
            case responsesToDamage.hit:
                //if (GameManagerMaster.GameMaster.GMSettings.logNPCCombat)
                //    _StateMachine.LogFromState($"{_NPC.name} is going to hit state");
				GetHit();
				break;
            case responsesToDamage.stagger:
				//if (GameManagerMaster.GameMaster.GMSettings.logNPCCombat)
				//	_StateMachine.LogFromState($"{_NPC.name} is going to stagger state");
				GetStaggered();
				break;
			case responsesToDamage.launch:
				//if (GameManagerMaster.GameMaster.GMSettings.logNPCCombat)
				//	_StateMachine.LogFromState($"{_NPC.name} is going to launch state");
				GetLaunched();
				break;
			case responsesToDamage.knockBack:
				//if (GameManagerMaster.GameMaster.GMSettings.logNPCCombat)
				//	_StateMachine.LogFromState($"{_NPC.name} is going to knockback state");
				GetKnockedBack();
				break;
		}
    }

	public virtual void GetHit()
	{
        //if (GameManagerMaster.GameMaster.GMSettings.logNPCCombat)
        //    _StateMachine.LogFromState($"{_NPC.name} is grounded? --> {_NPC._CheckGrounded.IsGrounded()}");
		if (!_NPC._CheckGrounded.IsGrounded())
		{
			_StateMachine.ChangeState(_StateMachine._StateLibrary._AirHitState);
			return;
		}
		_StateMachine.ChangeState(_StateMachine._StateLibrary._HitState);
	}
	public virtual void GetStaggered()
	{
		_StateMachine.ChangeState(_StateMachine._StateLibrary._StaggerState);
	}
	public virtual void GetLaunched()
	{
		_StateMachine.ChangeState(_StateMachine._StateLibrary._LaunchState);
	}
	public virtual void GetKnockedBack()
	{
		_StateMachine.ChangeState(_StateMachine._StateLibrary._KnockbackState);
	}
	#endregion
	// end of the line
}
