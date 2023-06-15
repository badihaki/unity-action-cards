using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [field: SerializeField] public PlayerControlsInput _Controls { get; private set; }
    [field: SerializeField] public PlayerCamera _CameraController { get; private set; }
    [field: SerializeField] public PlayerMovement _LocomotionController { get; private set; }
    [field: SerializeField] public PlayerCards _PlayerCards { get; private set; }


    public override void Initialize()
    {
        base.Initialize();
        
        // get the inputs
        _Controls = GetComponent<PlayerControlsInput>();

        // start the camera
        _CameraController = GetComponent<PlayerCamera>();
        _CameraController.Initialize(this);
        _LocomotionController = GetComponent<PlayerMovement>();
        _LocomotionController.Initialize(this);
        _PlayerCards = GetComponent<PlayerCards>();
        _PlayerCards.Initialize(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
