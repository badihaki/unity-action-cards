using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : Character, IDestroyable
{
    [field: SerializeField, Header("~> Nonplayer Character <~")] public NPCActor _NPCActor { get; private set; }
    [field: SerializeField] public NPCMovementController _MoveController { get; private set; }
    [field: SerializeField] public NPCNavigator _NavigationController { get; private set; }
    [field: SerializeField] public NPCMoveSet _MoveSet { get; private set; }
	[field: SerializeField] public CharacterUIController _UI { get; protected set; }


	// State Machine
	public NPCStateMachine _StateMachine { get; private set; }

    [field: SerializeField] private string hitAnimationString;
    
    public override void Initialize()
    {
        base.Initialize();

        _NPCActor = _Actor as NPCActor;
        _NPCActor.Initialize(this);

		// navigator
		_NavigationController = GetComponent<NPCNavigator>();
        _NavigationController.InitializeNavigator(this);

        // movement
        _MoveController = GetComponent<NPCMovementController>();
        _MoveController.InitializeNPCMovement(this);

		// start UI
		_UI = GetComponent<CharacterUIController>();
        _UI.InitializeUI(false, this);

        // move set
        _MoveSet = GetComponent<NPCMoveSet>();

        // attack controller
        _AttackController = GetComponent<NPCAttackController>();
        _AttackController.Initialize(this);

        // state machine
        _StateMachine = GetComponent<NPCStateMachine>();
        
        // Initialize state machine
        _StateMachine.InitializeStateMachine(this);
        
        // init moveset
        _MoveSet.Initialize(this);
    }

    protected override void TriggerhitAnimation(string hitType)
    {
        hitAnimationString = hitType;
        //_AnimationController.SetBool(hitAnimationString, true);
        _AnimationController.SetTrigger(hitAnimationString);
        _StateMachine.ChangeState(_StateMachine._HitState);
    }
    public void EndHurtAnimation()
    {
        _AnimationController.SetBool(hitAnimationString, false);
        hitAnimationString = "";
    }

	public override void DestroyEntity()
	{
        _Actor.Die();

		base.DestroyEntity();
	}

	#region State Triggers
	public void StateSideEffect() => _StateMachine._CurrentState.SideEffectTrigger();
    public void StateVisFX() => _StateMachine._CurrentState.VFXTrigger();
    public void StateSoundFX() => _StateMachine._CurrentState.SFXTrigger();
    public void StateAnimEnd() => _StateMachine._CurrentState.AnimationEndTrigger();
    #endregion
}
