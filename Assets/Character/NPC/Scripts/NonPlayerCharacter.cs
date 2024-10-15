using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : Character, IDestroyable
{
    [field: SerializeField] public NPCMovementController _MoveController { get; private set; }
    [field: SerializeField] public NPCNavigator _NavigationController { get; private set; }
    [field: SerializeField] public NPCAttack _AttackController { get; private set; }
    [field: SerializeField] public NPCActor _NPCActor { get; private set; }

    // State Machine
    public NPCStateMachine _StateMachine { get; private set; }
    public NPCIdleState _IdleState { get; private set; }
    public NPCIdleAggressiveState _IdleAggressiveState { get; private set; }
    public NPCMoveState _MoveState { get; private set; }
    public NPCHurtSuperState _HurtState { get; private set; }
    [field: SerializeField] private string hitAnimationString;
    
    public override void Initialize()
    {
        base.Initialize();

        _NPCActor = _Actor as NPCActor;
        _NPCActor.Initialize(this);

        // movement
        _MoveController = GetComponent<NPCMovementController>();
        _MoveController.InitializeNPCMovement(this);

        // ui
        if (_UI != null) _UI.InitializeUI(false);

        // navigator
        _NavigationController = GetComponent<NPCNavigator>();
        _NavigationController.InitializeNavigator(this);

        // attack controller
        _AttackController = GetComponent<NPCAttack>();
        _AttackController.InitiateAttack(this);

        // state machine
        _StateMachine = GetComponent<NPCStateMachine>();
        // set up states
        SetUpStateMachine();
        // Initialize state machine
        _StateMachine.InitializeStateMachine(_IdleState);
    }

    public virtual void SetUpStateMachine()
    {
        if (!_IdleState)
        {
            _IdleState = NPCIdleState.CreateInstance<NPCIdleState>();
        }
        _IdleState.InitState(this, _StateMachine, "idle");

        if (!_IdleAggressiveState)
        {
            _IdleAggressiveState = NPCIdleAggressiveState.CreateInstance<NPCIdleAggressiveState>();
        }
        _IdleAggressiveState.InitState(this, _StateMachine, "idle");

        if (!_MoveState)
        {
            _MoveState = NPCMoveState.CreateInstance<NPCMoveState>();
        }
        _MoveState.InitState(this, _StateMachine, "move");

        if (!_HurtState)
        {
            _HurtState = NPCHurtSuperState.CreateInstance<NPCHurtSuperState>();
        }
        _HurtState.InitState(this, _StateMachine, "hurt");
    }

    protected override void TriggerhitAnimation(string hitType)
    {
        hitAnimationString = hitType;
        _AnimationController.SetBool(hitAnimationString, true);
        _StateMachine.ChangeState(_HurtState);

        if (_AttackController && !_AttackController._IsAggressive)
        {
            int roll = GameManagerMaster.GameMaster.Dice.RollD6();
            // print($"aggression rol = {roll}");
            if (roll >= 4)
            {
                print("going aggressive");
            }
        }
    }
    public void EndHurtAnimation()
    {
        _AnimationController.SetBool(hitAnimationString, false);
        hitAnimationString = "";
    }

    #region State Triggers
    public void StateSideEffect() => _StateMachine._CurrentState.SideEffectTrigger();
    public void StateVisFX() => _StateMachine._CurrentState.VFXTrigger();
    public void StateSoundFX() => _StateMachine._CurrentState.SFXTrigger();
    public void StateAnimEnd() => _StateMachine._CurrentState.AnimationEndTrigger();
    #endregion
}
