using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : Character
{
    public PlayerControlsInput _Controls { get; private set; }
    public PlayerCamera _CameraController { get; private set; }
    public PlayerMovement _LocomotionController { get; private set; }
    public PlayerCards _PlayerCards { get; private set; }
    public PlayerSpell _PlayerSpells { get; private set; }
    public PlayerAttack _AttackController { get; private set; }
    
    // Actor Stuff
    public PlayerActor _PlayerActor { get; private set; }
    [field: SerializeField] public bool _LoadNewOnStart { get; private set; }
    [field: SerializeField] public PlayerCharacterHitbox _Hitbox { get; private set; }

    // state machine
    public PlayerStateMachine _StateMachine { get; private set; }
    public PlayerIdleState _IdleState { get; private set; }
    public PlayerMoveState _MoveState { get; private set; }
    public PlayerFallingState _FallingState { get; private set; }
    public PlayerJumpState _JumpState { get; private set; }
    public PlayerLandingState _LandingState { get; private set; }
    public PlayerGroundedCardState _GroundedCardState { get; private set; }
    public PlayerInAirCardState _InAirCardState { get; private set; }
    public PlayerReadySpellState _ReadySpellState { get; private set; }

    public override void Initialize()
    {
        // lets set up the actor
        if(_LoadNewOnStart)LoadAndBuildActor();
        _PlayerActor = GetComponentInChildren<PlayerActor>();
        _PlayerActor.InitializePlayerActor(this);

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

        // ok lets get attacks up
        _AttackController = GetComponent<PlayerAttack>();

        // start the hitbox
        _Hitbox = _Actor.Find("Colliders").Find("Hitbox").GetComponent<PlayerCharacterHitbox>();
        _Hitbox.Initialize(this);

        // initialize the statemachine
        InitializeStateMachine();

        // and initialize the attack controller, since it needs the state machine
        _AttackController.Initialize(this);
    }

    private void LoadAndBuildActor()
    {
        try
        {
            CharacterSaveData loadedOutfit = GameManagerMaster.GameMaster.SaveLoadManager.LoadCharacterData();
            BuildActorBody(loadedOutfit);
        }
        catch (Exception err)
        {
            Debug.LogError($"Exception was thrown: {err}");
        }
    }

    private void BuildActorBody(CharacterSaveData saveData)
    {
        CharCustomizationDatabase parts = GameManagerMaster.GameMaster.CharacterCustomizationDatabase;

        if (saveData.isMale)
        {
            _PlayerActor = Instantiate(GameManagerMaster.GameMaster.CharacterCustomizationDatabase.mActorBase, transform).GetComponent<PlayerActor>();
            
            SkinnedMeshRenderer head = _PlayerActor.transform.Find("Model.Head").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer hair = _PlayerActor.transform.Find("Model.Hair").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer horns = _PlayerActor.transform.Find("Model.Horns").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer top = _PlayerActor.transform.Find("Model.Top").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer hands = _PlayerActor.transform.Find("Model.Hands").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer bottom = _PlayerActor.transform.Find("Model.Bottom").GetComponent<SkinnedMeshRenderer>();

            head.sharedMesh = parts.mHeadDatabase[saveData.HeadIndex].mesh;
            head.material = parts.mHeadDatabase[saveData.HeadIndex].material;

            hair.sharedMesh = parts.mHairDatabase[saveData.HairIndex].mesh;
            hair.material = parts.mHairDatabase[saveData.HairIndex].material;

            horns.sharedMesh = parts.mHornsDatabase[saveData.HornIndex].mesh;
            horns.material = parts.mHornsDatabase[saveData.HornIndex].material;

            top.sharedMesh = parts.mTorsoDatabase[saveData.TopIndex].mesh;
            top.material = parts.mTorsoDatabase[saveData.TopIndex].material;

            hands.sharedMesh = parts.mHandsDatabase[saveData.HandsIndex].mesh;
            hands.material = parts.mHandsDatabase[saveData.HandsIndex].material;

            bottom.sharedMesh = parts.mBottomsDatabase[saveData.BottomIndex].mesh;
            bottom.material = parts.mBottomsDatabase[saveData.BottomIndex].material;
        }
        else
        {
            _PlayerActor = Instantiate(GameManagerMaster.GameMaster.CharacterCustomizationDatabase.fActorBase, transform).GetComponent<PlayerActor>();

            SkinnedMeshRenderer head = _PlayerActor.transform.Find("Model.Head").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer hair = _PlayerActor.transform.Find("Model.Hair").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer horns = _PlayerActor.transform.Find("Model.Horns").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer top = _PlayerActor.transform.Find("Model.Top").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer hands = _PlayerActor.transform.Find("Model.Hands").GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer bottom = _PlayerActor.transform.Find("Model.Bottom").GetComponent<SkinnedMeshRenderer>();

            head.sharedMesh = parts.fHeadDatabase[saveData.HeadIndex].mesh;
            head.material = parts.fHeadDatabase[saveData.HeadIndex].material;

            hair.sharedMesh = parts.fHairDatabase[saveData.HairIndex].mesh;
            hair.material = parts.fHairDatabase[saveData.HairIndex].material;

            horns.sharedMesh = parts.fHornsDatabase[saveData.HornIndex].mesh;
            horns.material = parts.fHornsDatabase[saveData.HornIndex].material;

            top.sharedMesh = parts.fTorsoDatabase[saveData.TopIndex].mesh;
            top.material = parts.fTorsoDatabase[saveData.TopIndex].material;

            hands.sharedMesh = parts.fHandsDatabase[saveData.HandsIndex].mesh;
            hands.material = parts.fHandsDatabase[saveData.HandsIndex].material;

            bottom.sharedMesh = parts.fBottomsDatabase[saveData.BottomIndex].mesh;
            bottom.material = parts.fBottomsDatabase[saveData.BottomIndex].material;
        }
        _PlayerActor.name = "Actor";
    }

    private void InitializeStateMachine()
    {
        // _StateMachine = new PlayerStateMachine();
        _StateMachine = GetComponent<PlayerStateMachine>();

        _IdleState = ScriptableObject.CreateInstance<PlayerIdleState>();
        _IdleState.InitializeState(this, "idle", _StateMachine);

        _MoveState = ScriptableObject.CreateInstance<PlayerMoveState>();
        _MoveState.InitializeState(this, "move", _StateMachine);

        _FallingState = ScriptableObject.CreateInstance<PlayerFallingState>();
        _FallingState.InitializeState(this, "air", _StateMachine);

        _JumpState = ScriptableObject.CreateInstance<PlayerJumpState>();
        _JumpState.InitializeState(this, "jump", _StateMachine);

        _LandingState = ScriptableObject.CreateInstance<PlayerLandingState>();
        _LandingState.InitializeState(this, "land", _StateMachine);

        _GroundedCardState = ScriptableObject.CreateInstance<PlayerGroundedCardState>();
        _GroundedCardState.InitializeState(this, "card", _StateMachine);

        _InAirCardState = ScriptableObject.CreateInstance<PlayerInAirCardState>();
        _InAirCardState.InitializeState(this, "airCard", _StateMachine);

        _ReadySpellState = ScriptableObject.CreateInstance<PlayerReadySpellState>();
        _ReadySpellState.InitializeState(this, "range", _StateMachine);

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

    public void StateAnimationFinished()=>_StateMachine._CurrentState.AnimationFinished();
    public void StateTrigger() => _StateMachine._CurrentState.TriggerSideEffect();


}
