using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : Character, IDestroyable
{
    [field: SerializeField] public NPCMovementController _MoveController { get; private set; }

    // State Machine
    public NPCStateMachine _StateMachine { get; private set; }
    
    public override void Initialize()
    {
        base.Initialize();

        // movement
        _MoveController = GetComponent<NPCMovementController>();
        _MoveController.InitializeNPCMovement(this);

        // ui
        if (_UI != null) _UI.InitializeUI(false);

        // state machine
        _StateMachine = GetComponent<NPCStateMachine>();
        // _StateMachine.InitializeStateMachine(_IdleState);
    }
}
