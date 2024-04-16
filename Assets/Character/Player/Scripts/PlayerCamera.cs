using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Editor;
using System;

public class PlayerCamera : MonoBehaviour
{
    private PlayerCharacter Player;

    [field: SerializeField] public CinemachineVirtualCamera _PlayerCamController { get; private set; }
    [field: SerializeField] public CinemachineVirtualCamera _PlayerAimCamController { get; private set; }
    [SerializeField] private CinemachineVirtualCamera currentCameraController;
    
    [Tooltip("How far can the camera look up")]
    [SerializeField] private float topLookClamp = 70.0f;
    [Tooltip("How far can the camera look down")]
    [SerializeField] private float bottomLookClamp = -30.0f;

    [field: SerializeField] public Transform cinemachineCamTarget { get; private set; }
    
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(PlayerCharacter controller)
    {
        Player = controller;
        // GameObject newGameObj = Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity);
        cinemachineCamTarget = transform.Find("CamTarget");
        InitializeCinemachineController();
        InitializeAimCamController();
        LockCursorKBM();
        currentCameraController = _PlayerAimCamController;
    }
    private void InitializeCinemachineController()
    {
        GameObject newGameObj = new GameObject();
        newGameObj.name = "PlayerCamController";
        _PlayerCamController = newGameObj.AddComponent<CinemachineVirtualCamera>();
        _PlayerCamController.Follow = cinemachineCamTarget;
        _PlayerCamController.LookAt = cinemachineCamTarget;
        _PlayerCamController.Priority = 10;
        _PlayerCamController.m_Lens.FieldOfView = 90;
        // lens fov - 90

        Cinemachine3rdPersonFollow body = _PlayerCamController.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
        body.CameraDistance = 1.850f; // 1.85
        body.VerticalArmLength = 1.55f;
        body.ShoulderOffset = new Vector3(0, -0.500f, 0);
        // rig shoulder offset 0, -0.500, 0
        
        CinemachineComposer composer = _PlayerCamController.AddCinemachineComponent<CinemachineComposer>();
        composer.m_TrackedObjectOffset = new Vector3(0, 0.850f, 0); // 0, 0.850, 0
        cinemachineTargetYaw = cinemachineCamTarget.transform.rotation.eulerAngles.y;
    }
    private void InitializeAimCamController()
    {
        GameObject newGameObj = new GameObject();
        newGameObj.name = "PlayerAimCamController";
        _PlayerAimCamController = newGameObj.AddComponent<CinemachineVirtualCamera>();
        _PlayerAimCamController.Follow = cinemachineCamTarget;
        _PlayerAimCamController.LookAt = cinemachineCamTarget;
        _PlayerAimCamController.Priority = 0;

        _PlayerAimCamController.m_Lens.FieldOfView = 25.5f;

        Cinemachine3rdPersonFollow body = _PlayerAimCamController.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
        body.CameraDistance = 1.75f;
        body.VerticalArmLength = 1.6f;
        body.ShoulderOffset = new Vector3(0.0f, 0.75f, 0.0f);
        
        CinemachineComposer composer = _PlayerAimCamController.AddCinemachineComponent<CinemachineComposer>();
        composer.m_TrackedObjectOffset = new Vector3(0, 0.775f, 0);
        cinemachineTargetYaw = cinemachineCamTarget.transform.rotation.eulerAngles.y;
        composer.m_ScreenY = 0.45f;
    }

    public void LockCursorKBM() => Cursor.lockState = CursorLockMode.Locked;
    public void UnlockCursorKBM() => Cursor.lockState = CursorLockMode.None;

    public void ControlCameraRotation(Vector2 aimInput)
    {
        const float lookThreshold = 0.01f;
        if(aimInput.sqrMagnitude >= lookThreshold)
        {
            cinemachineTargetYaw += aimInput.x;
            cinemachineTargetPitch += aimInput.y;
        }
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomLookClamp, topLookClamp);

        cinemachineCamTarget.rotation = Quaternion.Euler(-cinemachineTargetPitch, cinemachineTargetYaw, 0.0f);
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void SwitchCam(CinemachineVirtualCamera vCam)
    {
        currentCameraController.Priority = 0;
        currentCameraController = vCam;
        currentCameraController.Priority = 10;
    }

    public void ResetCinemachineTargetTransform() => cinemachineCamTarget.rotation = Quaternion.Euler(Vector3.zero);

    // end
}
