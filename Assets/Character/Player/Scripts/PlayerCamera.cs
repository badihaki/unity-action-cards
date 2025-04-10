using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Cinemachine;
// using Cinemachine.Editor;
using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor;

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
    [SerializeField] private float topLookClamp = 38.60f;
    [SerializeField] private float topLookClampWhileAiming = 59.125f;
	[Tooltip("How far can the camera look down")]
    [SerializeField] private float bottomLookClamp = -43.3f;
	[SerializeField] private float lookSensitivity = 3.0f;

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
        InitializeCamController();
        InitializeAimCamController();
        LockCursorKBM();
        currentCameraController = _PlayerCamController;
        cursorLocked = true;
        SetTargetCamera();
        ResetCinemachineTargetTransform();
    }

    public void SetTargetCamera() => _Camera = Camera.main;

	private void InitializeCamController()
    {
        GameObject newGameObj = new GameObject();
        newGameObj.name = "PlayerCamController";
        newGameObj.transform.parent = transform;
        _PlayerCamController = newGameObj.AddComponent<CinemachineCamera>();
        cinemachineCamTarget.transform.rotation = _PlayerCamController.transform.rotation; // does this even work??
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
        newGameObj.transform.parent = transform;
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
        pos.y = Player._Actor.transform.position.y + 1.5f;
        cinemachineCamTarget.transform.position = pos;
    }

    public void ControlCameraRotation(Vector2 aimInput, bool isAiming = false)
    {
        const float lookThreshold = 0.01f; // how far do we attempt to look around before the cam should start moving
        if(aimInput.sqrMagnitude >= lookThreshold)
        {
            aimInput *= 1.43f; // add a prelim modifier
            cinemachineTargetYaw += aimInput.x;
            cinemachineTargetPitch += aimInput.y;
        }

        float desiredTopClamp = isAiming ? topLookClampWhileAiming : topLookClamp;
        float desiredBotClamp = bottomLookClamp;
#if UNITY_EDITOR
		if (Application.isEditor && PlayModeWindow.GetPlayModeFocused() == true)
        {
            lookSensitivity = Mathf.Clamp(lookSensitivity, isAiming ? 1.750f : 2.25f, isAiming ? 2.5f : 3.0f);
            desiredTopClamp = desiredTopClamp / 2;
            desiredBotClamp = desiredBotClamp / 2;
        }
#endif

        // clamp movement
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, desiredBotClamp, desiredTopClamp);



        // rotate cam target
        cinemachineCamTarget.rotation = Quaternion.Euler(-cinemachineTargetPitch * lookSensitivity, cinemachineTargetYaw * lookSensitivity, 0.0f);
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
