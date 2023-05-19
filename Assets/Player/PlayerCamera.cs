using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    private Player Player;
    [field: SerializeField] private CinemachineVirtualCamera CinemachineController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(Player controller)
    {
        Player = controller;
        // GameObject newGameObj = Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity);
        InitializeController();
    }
    private void InitializeController()
    {
        GameObject newGameObj = new GameObject();
        newGameObj.name = "PlayerCamController";
        CinemachineController = newGameObj.AddComponent<CinemachineVirtualCamera>();
        CinemachineController.Follow = Player.transform;
        CinemachineController.LookAt = Player.transform;
        Cinemachine3rdPersonFollow body = CinemachineController.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
        body.CameraDistance = 3.270f;
        body.VerticalArmLength = 1.55f;
        CinemachineComposer composer = CinemachineController.AddCinemachineComponent<CinemachineComposer>();
        composer.m_TrackedObjectOffset = new Vector3(0, 1.15f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
