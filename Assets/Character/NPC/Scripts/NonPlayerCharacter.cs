using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : Character, IDestroyable
{
    [field: SerializeField] public NPCMovementController _MoveController { get; private set; }
    [field: SerializeField] public NPCNavigator _NavigationController { get; private set; }

    // State Machine
    public NPCStateMachine _StateMachine { get; private set; }
    public NPCIdleState _IdleState { get; private set; }
    public NPCMoveState _MoveState { get; private set; }
    
    public override void Initialize()
    {
        base.Initialize();

        // movement
        _MoveController = GetComponent<NPCMovementController>();
        _MoveController.InitializeNPCMovement(this);

        // ui
        if (_UI != null) _UI.InitializeUI(false);

        // navigator
        _NavigationController = GetComponent<NPCNavigator>();
        _NavigationController.InitializeNavigator(this);

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

        if (!_MoveState)
        {
            _MoveState = NPCMoveState.CreateInstance<NPCMoveState>();
        }
        _MoveState.InitState(this, _StateMachine, "move");
    }
}
