using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;

public class CamFocus : MonoBehaviour
{
    [field: Header("Possible Camera Positions")]
    [field: SerializeField] public Transform regularPos { get; private set; }
    [field: SerializeField] public Transform zoomedPos { get; private set; }

    [field:Header("Cam Movement")]
    [field:SerializeField]public bool canMove {  get; private set; } = false;
    [field: SerializeField] public Transform cam { get; private set; }
    [field: SerializeField] public Transform gameLight { get; private set; }
    [field: SerializeField] public Vector2 aimVector { get; private set; }
    [SerializeField] private float angle = 120.00f;

    [field: Header("Zoom toggle")]
    [field: SerializeField] public bool isZoomed { get; private set; } = false;
    [field: SerializeField] public Transform originalTransformInfo { get; private set; }



    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Camera").transform;
        gameLight = GameObject.Find("Light").transform;
        originalTransformInfo = gameLight;
    }

    public void TurnCamera(InputAction.CallbackContext input)
    {
        aimVector = input.ReadValue<Vector2>();
        if(canMove)
        {
            Vector3 rotationVector = new Vector3(0, aimVector.x, 0);
            cam.RotateAround(transform.position, rotationVector, angle * Time.deltaTime);
            gameLight.RotateAround(transform.position, rotationVector, angle * Time.deltaTime);
        }
    }

    public void ToggleZoom(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            SetZoom();
        }
    }

    public void SetZoom()
    {
        isZoomed = !isZoomed;
        if (isZoomed)
        {
            cam.position = zoomedPos.position;
            cam.rotation = zoomedPos.rotation;
            print("Zoom");
        }
        else
        {
            cam.position = regularPos.position;
            cam.rotation = regularPos.rotation;
            print("ZoomOAWT");
        }
        gameLight.eulerAngles = new Vector3(50.0f, -30.0f, 0); // how you set rotation!!
        print("finish zoom");
    }

    public void HoldToMove(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            canMove = true;
        }
        else if (input.canceled)
        {
            canMove = false;
        }
    }
}
