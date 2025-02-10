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


	// State Machine
	public NPCStateMachine _StateMachine { get; private set; }

    [field: SerializeField] private string hitAnimationString;
    
    public virtual void BuildAndInitialize(NPCSheetScriptableObj characterSheet)
    {
		transform.SetParent(GameManagerMaster.GameMaster.GeneralConstantVariables.CharactersFolder(), true);
		_CharacterSheet = characterSheet;
        _NPCharacterSheet = _CharacterSheet as NPCSheetScriptableObj;
        name = _CharacterSheet._CharacterName;
        GameObject cloneActor = Instantiate(characterSheet.Actor, transform);
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

	protected override void RespondToHit(string hitType)
	{
		switch (hitType)
		{
			case "hit":
				print($"{name} >>>> respond hit >>> hit state");
                _StateMachine.ChangeState(_StateMachine._StateLibrary._HitState);
				break;
			case "staggered":
				print($"{name} >>>> respond knockBack//stagger>>> stagger state");
                _StateMachine.ChangeState(_StateMachine._StateLibrary._StaggerState);
				break;
			case "launched":
				print($"{name} >>>> respond launch >>> launch state");
                _StateMachine.ChangeState(_StateMachine._StateLibrary._LaunchState); 
				break;
			case "airHit":
				print($"{name} >>>> respond air hit >>> air Hit state");
				_StateMachine.ChangeState(_StateMachine._StateLibrary._AirHitState);
				break;
			case "knockback":
				print($"{name} >>>> respond far far knock back//knockback >>> kncok back state");
				_StateMachine.ChangeState(_StateMachine._StateLibrary._KnockbackState);
				break;
            default:
                print("probably not an attack");
                break;
		}
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
