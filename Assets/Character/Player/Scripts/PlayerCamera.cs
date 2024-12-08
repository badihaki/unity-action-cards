using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Cinemachine;
// using Cinemachine.Editor;
using System;
using Unity.Cinemachine;
using Unity.VisualScripting;

public class PlayerCamera : MonoBehaviour
{
    private PlayerCharacter Player;
    [field: SerializeField] public Camera _Camera { get; private set; }
    [Header("Cinemachine Cameras")]
    [field: SerializeField] public CinemachineCamera _PlayerCamController { get; private set; }
    [field: SerializeField] public CinemachineCamera _PlayerSpellCamController { get; private set; }
    [SerializeField] private CinemachineCamera currentCameraController;

    [Header("Camera Settings")]
    [Tooltip("How far can the camera look up")]
    [SerializeField] private float topLookClamp = 10.0f;
    [Tooltip("How far can the camera look down")]
    [SerializeField] private float bottomLookClamp = -15.0f;
    [SerializeField] private float lookSensitivity = 1.0f;

    [field: SerializeField] public Transform cinemachineCamTarget { get; private set; }
    
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    [SerializeField] private bool cursorLocked;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialize(PlayerCharacter controller)
    {
        Player = controller;
        cinemachineCamTarget = Player.transform.Find("CamTarget");
        InitializeCinemachineController();
        InitializeAimCamController();
        LockCursorKBM();
        currentCameraController = _PlayerCamController;
        cursorLocked = false;
        _Camera = Camera.main;
    }
    private void InitializeCinemachineController()
    {
        GameObject newGameObj = new GameObject();
        newGameObj.name = "PlayerCamController";
        _PlayerCamController = newGameObj.AddComponent<CinemachineCamera>();
        _PlayerCamController.Follow = cinemachineCamTarget;
        _PlayerCamController.Priority = 10;
        _PlayerCamController.Lens.FieldOfView = 90;

        CinemachineThirdPersonFollow body = _PlayerCamController.AddComponent<CinemachineThirdPersonFollow>();
        body.CameraDistance = 2.65f;
        body.VerticalArmLength = 0.6f;
        body.ShoulderOffset = new Vector3(-0.450f, -0.125f, 0.0f);
        body.CameraSide = 0.84f;
    }
    private void InitializeAimCamController()
    {
        GameObject newGameObj = new GameObject();
        newGameObj.name = "PlayerAimCamController";
        _PlayerSpellCamController = newGameObj.AddComponent<CinemachineCamera>();
        _PlayerSpellCamController.Follow = cinemachineCamTarget;
        _PlayerSpellCamController.Priority = 0;

        _PlayerSpellCamController.Lens.FieldOfView = 25.5f;

        CinemachineThirdPersonFollow body = _PlayerSpellCamController.AddComponent<CinemachineThirdPersonFollow>();
        body.CameraDistance = 1.75f;
        body.VerticalArmLength = 1.6f;
        body.ShoulderOffset = new Vector3(0.750f, -1.25f, 0.0f);
        body.CameraSide = 0.84f;
    }

    public void LockCursorKBM()
    {
        if (!cursorLocked)
        {
            cursorLocked = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void UnlockCursorKBM()
    {
        if (cursorLocked)
        {
            cursorLocked = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void MakeCameraFollowPlayerActor()
    {
        Vector3 pos = Player._Actor.transform.position;
        pos.y = 1.5f;
        cinemachineCamTarget.transform.position = pos;
    }

    public void ControlCameraRotation(Vector2 aimInput)
    {
        const float lookThreshold = 0.01f; // how far do we attempt to look around before the cam should start moving
        if(aimInput.sqrMagnitude >= lookThreshold)
        {
            cinemachineTargetYaw += aimInput.x;
            cinemachineTargetPitch += aimInput.y;
        }

        // clamp movement
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomLookClamp, topLookClamp);

        LimitSensitivity();

        // rotate cam target
        cinemachineCamTarget.rotation = Quaternion.Euler(-cinemachineTargetPitch * lookSensitivity, cinemachineTargetYaw * lookSensitivity, 0.0f);
    }

    private void LimitSensitivity()
    {
        if (lookSensitivity < 3.0f) lookSensitivity = 3.0f;
        else if (lookSensitivity > 8.00f) lookSensitivity = 8.00f;
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void SwitchCam(CinemachineCamera vCam)
    {
        currentCameraController.Priority = 0;
        currentCameraController = vCam;
        currentCameraController.Priority = 10;
    }

    public void ResetCinemachineTargetTransform() => cinemachineCamTarget.rotation = Quaternion.Euler(Vector3.zero);

    // end
}
