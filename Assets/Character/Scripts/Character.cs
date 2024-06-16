using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Character : MonoBehaviour
{
    [field: SerializeField] public CharacterSheet _CharacterSheet { get; protected set; }
    public Health _Health { get; private set; }
    public Aether _AetherPoints { get; private set; }
    public CheckForGround _CheckGrounded { get; private set; }
    [field: SerializeField] public Transform _Actor { get; protected set; }
    [field: SerializeField] public Animator _AnimationController { get; private set; }
    [field: SerializeField] public CharacterHurtbox _Hurtbox { get; private set; }
    [field: SerializeField] public CharacterUIController _UI { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        if (_Health != null) _Health.OnHit += TriggerhitAnimation;
    }

    private void OnDisable()
    {
        if (_Health != null) _Health.OnHit -= TriggerhitAnimation;
    }

    public virtual void Initialize()
    {
        // Create the character in the game world
        _Actor = transform.Find("Actor");

        // start health
        _Health = GetComponent<Health>();
        if (_Health == null) _Health = transform.AddComponent<Health>();
        _Health.InitiateHealth(_CharacterSheet._StartingHealth);
        _Health.OnHit += TriggerhitAnimation;

        // start aether points (magic points)
        _AetherPoints = GetComponent<Aether>();
        _AetherPoints.InitiateAetherPointPool(_CharacterSheet._StartingAetherPool);

        // start checking for ground
        _CheckGrounded = GetComponent<CheckForGround>();
        if (_CheckGrounded == null) _CheckGrounded = transform.AddComponent<CheckForGround>();
        _CheckGrounded.Initialize();

        // Start the hurtbox
        _Hurtbox = GetComponentInChildren<CharacterHurtbox>();
        if (_Hurtbox == null) _Hurtbox = transform.Find("Colliders").Find("Hurtbox").AddComponent<CharacterHurtbox>();
        _Hurtbox.InitializeHurtBox(this);

        // start UI
        _UI = GetComponent<CharacterUIController>();

        _AnimationController = _Actor.GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int damage, Transform damageSource)
    {
        throw new System.NotImplementedException();
    }

    protected virtual void TriggerhitAnimation(string hitType)
    {
        if (_AnimationController)
        {
            print(hitType);
            _AnimationController.SetTrigger(hitType);
        }
    }
}
