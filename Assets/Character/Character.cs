using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [field: SerializeField] public CharacterSheet _CharacterSheet { get; protected set; }
    [field: SerializeField] public Health _Health { get; private set; }
    [field: SerializeField] public CheckForGround _CheckGrounded { get; private set; }
    [field: SerializeField] public Transform _Actor { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        // Create the character in the game world
        _Actor = transform.Find("Actor");

        // start health
        _Health = GetComponent<Health>();
        _Health.InitiateHealth(_CharacterSheet._StartingHealth);

        // start checking for ground
        _CheckGrounded = GetComponent<CheckForGround>();
        _CheckGrounded.Initialize();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
