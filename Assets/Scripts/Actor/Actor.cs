using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [field: SerializeField] public CharacterSheet CharacterSheet { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public ActorMovementController MovementController { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Health = GetComponent<Health>();
        Health.InitiateHealth(CharacterSheet.StartingHealth);
        MovementController = GetComponent<ActorMovementController>();
        MovementController.Initialize(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
