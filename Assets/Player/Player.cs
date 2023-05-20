using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerControlsInput ControlsInput { get; private set; }
    [field: SerializeField] public PlayerCamera CameraController { get; private set; }
    [field: SerializeField] public PlayerMovement LocomotionController { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        ControlsInput = GetComponent<PlayerControlsInput>();
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
