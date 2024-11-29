using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Character : MonoBehaviour
{
    [field: SerializeField, Header(">> Character <<")] public CharacterSheet _CharacterSheet { get; protected set; }
    public Health _Health { get; private set; }
    public Aether _Aether { get; private set; }
    public CharacterAttackController _AttackController { get; protected set; }
    public CheckForGround _CheckGrounded { get; private set; }
    [field: SerializeField] public Actor _Actor { get; protected set; }
    [field: SerializeField, Header("Character Components")] public Animator _AnimationController { get; private set; }
    [field: SerializeField] public CharacterHurtbox _Hurtbox { get; private set; }
    [field: SerializeField] public CharacterSoundManager _SoundManager { get; protected set; }
    public Camera _CameraRef { get; protected set; }


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        // Create the character in the game world
        _Actor = transform.Find("Actor").GetComponent<Actor>();

		// start the hitbox
		_Actor.transform.Find("Hitbox").GetComponent<CharacterHitbox>()?.Initialize(this);

        // Attack
        _AttackController = GetComponent<CharacterAttackController>();

		// start health
		_Health = GetComponent<Health>();
        if (_Health == null) _Health = transform.AddComponent<Health>();
        _Health.InitiateHealth(_CharacterSheet._StartingHealth);

        // start aether points (magic points)
        _Aether = GetComponent<Aether>();
        _Aether.InitiateAetherPointPool(_CharacterSheet._StartingAetherPool);

        // start checking for ground
        _CheckGrounded = _Actor.GetComponent<CheckForGround>();
        if (_CheckGrounded == null) _CheckGrounded = _Actor.AddComponent<CheckForGround>();
        _CheckGrounded.Initialize();

        // Start the hurtbox
        _Hurtbox = GetComponentInChildren<CharacterHurtbox>();
        if (_Hurtbox == null) _Hurtbox = transform.Find("Hurtbox").GetComponent<CharacterHurtbox>();
        _Hurtbox.InitializeHurtBox(this);

        _AnimationController = _Actor.GetComponent<Animator>();

        _CameraRef = Camera.main;

        _SoundManager = GetComponent<CharacterSoundManager>();
        _SoundManager.InitializeSoundManager(this);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void RespondToHit(string hitType)
    {
    }

    public virtual void DestroyEntity()
    {
        print("goodbye, " + name);
        Destroy(gameObject);
    }

	public void CalculateHitResponse(bool isKnocked, bool isLaunched, float damage = 1.0f)
	{
		float poiseLost = UnityEngine.Random.Range(0.2f, damage * MathF.PI / 2);
		//if (poiseLost > _Health._PoiseThreshold() * 1.15f) // is this too complicated
		//    poiseLost = Mathf.Clamp(poiseLost, 0.0f, _Health._PoiseThreshold() / 2);
		float updatedPoise = _Health.ChangePoise(poiseLost);
		
        if (isLaunched)
		{
			RespondToHit("launch");
			return;
		}
		if (isKnocked)
		{
			RespondToHit("knockBack");
			return;
		}
		if (updatedPoise > _Health._PoiseThreshold())
        {
			RespondToHit("hit");
            return;
        }
	}
}
