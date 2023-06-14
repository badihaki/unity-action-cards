using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [field: SerializeField] public PlayerControlsInput ControlsInput { get; private set; }
    [field: SerializeField] public PlayerCamera CameraController { get; private set; }
    [field: SerializeField] public PlayerMovement LocomotionController { get; private set; }
    [field: SerializeField] public PlayerCards PlayerCards { get; private set; }


    public override void Initialize()
    {
        base.Initialize();
        
        // get the inputs
        ControlsInput = GetComponent<PlayerControlsInput>();

        // start the camera
        CameraController = GetComponent<PlayerCamera>();
        CameraController.Initialize(this);
        LocomotionController = GetComponent<PlayerMovement>();
        LocomotionController.Initialize(this);
        PlayerCards = GetComponent<PlayerCards>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
