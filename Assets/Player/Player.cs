using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public CharacterSheet CharacterSheet { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public PlayerControlsInput ControlsInput { get; private set; }
    [field: SerializeField] public PlayerCamera CameraController { get; private set; }
    [field: SerializeField] public PlayerMovement LocomotionController { get; private set; }
    [field: SerializeField] public Transform Character { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        // Create the character in the game world
        Character = transform.Find("Character");

        // start health
        Health = GetComponent<Health>();
        Health.InitiateHealth(CharacterSheet.StartingHealth);

        // get the inputs
        ControlsInput = GetComponent<PlayerControlsInput>();

        // start the camera
        CameraController = GetComponent<PlayerCamera>();
        CameraController.Initialize(this);
        LocomotionController = GetComponent<PlayerMovement>();
        LocomotionController.Initialize(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
