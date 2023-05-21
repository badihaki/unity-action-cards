using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    private Player Player;
    [field: SerializeField] private CinemachineVirtualCamera CinemachineController;
    [Tooltip("How far can the camera look up")]
    [SerializeField] private float topLookClamp = 70.0f;
    [Tooltip("How far can the camera look down")]
    [SerializeField] private float bottomLookClamp = -30.0f;
    [field: SerializeField] private Transform cinemachineCamTarget;
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(Player controller)
    {
        Player = controller;
        // GameObject newGameObj = Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity);
        cinemachineCamTarget = transform.Find("CamTarget");
        InitializeCinemachineController();
    }
    private void InitializeCinemachineController()
    {
        GameObject newGameObj = new GameObject();
        newGameObj.name = "PlayerCamController";
        CinemachineController = newGameObj.AddComponent<CinemachineVirtualCamera>();
        CinemachineController.Follow = cinemachineCamTarget;
        CinemachineController.LookAt = cinemachineCamTarget;
        Cinemachine3rdPersonFollow body = CinemachineController.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
        body.CameraDistance = 3.270f;
        body.VerticalArmLength = 1.55f;
        CinemachineComposer composer = CinemachineController.AddCinemachineComponent<CinemachineComposer>();
        composer.m_TrackedObjectOffset = new Vector3(0, 1.15f, 0);
        cinemachineTargetYaw = cinemachineCamTarget.transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation(Player.ControlsInput.aimInput);
    }

    public void CameraRotation(Vector2 aimInput)
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

    // end
}
