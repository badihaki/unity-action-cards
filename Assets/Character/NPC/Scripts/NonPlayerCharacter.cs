using UnityEngine;

public class NonPlayerCharacter : Character, IDestroyable
{
    [field: SerializeField, Header("~> Nonplayer Character <~")]
    public NPCSheetScriptableObj _NPCharacterSheet { get; private set; }
	[field: SerializeField] public NPCActor _NPCActor { get; private set; }
    [field: SerializeField] public NPCMovementController _MoveController { get; private set; }
    [field: SerializeField] public NPCNavigator _NavigationController { get; private set; }
    [field: SerializeField] public NPCMoveSet _MoveSet { get; private set; }
	[field: SerializeField] public CharacterUIController _UI { get; protected set; }
    [field: SerializeField] public CharacterEyesight _EyeSight { get; protected set; }
	[field: SerializeField] public NPCTypesManager _TypesManager { get; protected set; }


	// State Machine
	public NPCStateMachine _StateMachine { get; private set; }

    [field: SerializeField] private string hitAnimationString;
    
    public virtual void BuildAndInitialize(NPCSheetScriptableObj characterSheet)
    {
		transform.SetParent(GameManagerMaster.GameMaster.GeneralConstantVariables.GetCharactersFolder(), true);
		_CharacterSheet = characterSheet;
        _NPCharacterSheet = _CharacterSheet as NPCSheetScriptableObj;
        _TypesManager = GetComponent<NPCTypesManager>();
        _TypesManager.Initialize(_NPCharacterSheet);
        //name = _CharacterSheet._CharacterName;
        GameObject cloneActor = ObjectPoolManager.GetObjectFromPool(characterSheet.Actor, transform.position, transform.rotation, transform);
        cloneActor.name = "Actor";
        Initialize();
    }

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

        // eyes for sight beyond sight
        _EyeSight = GetComponentInChildren<CharacterEyesight>();

        // state machine
        _StateMachine = GetComponent<NPCStateMachine>();
        
        // Initialize state machine
        _StateMachine.InitializeStateMachine(this);
        
        // init moveset
        _MoveSet.Initialize(this);
    }

    public override void RespondToHit(responsesToDamage intendedDamageResponse) => _StateMachine.GoToHurtState(intendedDamageResponse);
	public override void PushBackCharacter(Vector3 pushFromPoint, float pushBackForce, bool isLaunched = false) => _MoveController.GetPushedBack(pushFromPoint, pushBackForce, isLaunched);
	public override void ResetCharacterPushback() => _MoveController.ResetPushback();

	public void EndHurtAnimation()
    {
        _AnimationController.SetBool(hitAnimationString, false);
        hitAnimationString = "";
    }

	public void DestroyEntity()
	{
        
        _Actor.Die();
        ObjectPoolManager.ReturnObjectToPool(gameObject);
	}

	public override void AddToExternalForce(Vector3 force)
	{
		base.AddToExternalForce(force);
        _MoveController.AddToExternalForces(force);
	}

	#region State Triggers
	public void StateSideEffect() => _StateMachine._CurrentState.SideEffectTrigger();
    public void StateVisFX() => _StateMachine._CurrentState.VFXTrigger();
    public void StateSoundFX() => _StateMachine._CurrentState.SFXTrigger();
    public void StateAnimEnd() => _StateMachine._CurrentState.AnimationEndTrigger();
    #endregion
}
