using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [field: SerializeField] public PlayerControlsInput _Controls { get; private set; }
    [field: SerializeField] public PlayerCamera _CameraController { get; private set; }
    [field: SerializeField] public PlayerMovement _LocomotionController { get; private set; }
    [field: SerializeField] public PlayerCards _PlayerCards { get; private set; }
    [field: SerializeField] public PlayerSpell _PlayerSpells { get; private set; }

    // state machine
    public PlayerStateMachine _StateMachine { get; private set; }
    public PlayerIdleState _IdleState { get; private set; }
    public PlayerMoveState _MoveState { get; private set; }
    public PlayerFallingState _FallingState { get; private set; }
    public PlayerJumpState _JumpState { get; private set; }
    public PlayerGroundedCardState _GroundedCardState { get; private set; }
    public PlayerInAirCardState _InAirCardState { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
        
        // get the inputs
        _Controls = GetComponent<PlayerControlsInput>();

        // start the camera
        _CameraController = GetComponent<PlayerCamera>();
        _CameraController.Initialize(this);

        // start locomotion --> movement && jumping
        _LocomotionController = GetComponent<PlayerMovement>();
        _LocomotionController.Initialize(this);

        // start the card stuff
        _PlayerCards = GetComponent<PlayerCards>();
        _PlayerCards.Initialize(this);

        // give the player the ability to use spells
        _PlayerSpells = GetComponent<PlayerSpell>();
        _PlayerSpells.Initialize(this);

        // initialize the statemachine
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        _StateMachine = new PlayerStateMachine();

        _IdleState = new PlayerIdleState(this, "idle", _StateMachine);
        _MoveState = new PlayerMoveState(this, "move", _StateMachine);
        _FallingState = new PlayerFallingState(this, "falling", _StateMachine);
        _JumpState = new PlayerJumpState(this, "jump", _StateMachine);
        _GroundedCardState = new PlayerGroundedCardState(this, "card", _StateMachine);
        _InAirCardState = new PlayerInAirCardState(this, "airCard", _StateMachine);

        _StateMachine.InitializeStateMachine(_IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        _StateMachine?._CurrentState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        _StateMachine?._CurrentState.PhysicsUpdate();
    }
}
