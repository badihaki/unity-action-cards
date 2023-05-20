using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player Player;
    private Rigidbody Rigidbody;
    private Collider body;
    private float GroundRayDistance = 0.250f;
    public bool IsGrounded()
    {
        Ray midRay = new Ray(transform.position, -transform.up);
        Ray leftRay = new Ray(new Vector3(transform.position.x - body.bounds.extents.x, transform.position.y, transform.position.z), -transform.up);
        Ray rightRay = new Ray(new Vector3(transform.position.x + body.bounds.extents.x, transform.position.y, transform.position.z), -transform.up);
        Ray frontRay = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z + body.bounds.extents.z), -transform.up);
        Ray backRay = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z - body.bounds.extents.z), -transform.up);
        if (Physics.Raycast(midRay, out RaycastHit midHitInfo, GroundRayDistance))
        {
            Debug.DrawLine(midRay.origin, new Vector3(midRay.origin.x, midRay.origin.y - GroundRayDistance, midRay.origin.z), Color.red, 0.05f);
            Debug.Log(midHitInfo);
            return true;
        }
        if (Physics.Raycast(leftRay, out RaycastHit leftHitInfo, GroundRayDistance))
        {
            Debug.DrawLine(leftRay.origin, new Vector3(leftRay.origin.x, leftRay.origin.y - GroundRayDistance, leftRay.origin.z), Color.red, 0.05f);
            return true;
        }
        if (Physics.Raycast(rightRay, out RaycastHit rightHitInfo, GroundRayDistance))
        {
            Debug.DrawLine(rightRay.origin, new Vector3(rightRay.origin.x, rightRay.origin.y - GroundRayDistance, rightRay.origin.z), Color.red, 0.05f);
            return true;
        }
        if (Physics.Raycast(frontRay, out RaycastHit frontHitInfo, GroundRayDistance))
        {
            Debug.DrawLine(frontRay.origin, new Vector3(frontRay.origin.x, frontRay.origin.y - GroundRayDistance, frontRay.origin.z), Color.red, 0.05f);
            return true;
        }
        if (Physics.Raycast(backRay, out RaycastHit backHitInfo, GroundRayDistance))
        {
            Debug.DrawLine(backRay.origin, new Vector3(backRay.origin.x, backRay.origin.y - GroundRayDistance, backRay.origin.z), Color.red, 0.05f);
            return true;
        }
        else
        {
            Debug.DrawLine(midRay.origin, new Vector3(midRay.origin.x, midRay.origin.y - GroundRayDistance, midRay.origin.z), Color.gray, 0.05f);
            Debug.DrawLine(leftRay.origin, new Vector3(leftRay.origin.x, leftRay.origin.y - GroundRayDistance, leftRay.origin.z), Color.gray, 0.05f);
            Debug.DrawLine(rightRay.origin, new Vector3(rightRay.origin.x, rightRay.origin.y - GroundRayDistance, rightRay.origin.z), Color.gray, 0.05f);
            Debug.DrawLine(frontRay.origin, new Vector3(frontRay.origin.x, frontRay.origin.y - GroundRayDistance, frontRay.origin.z), Color.gray, 0.05f);
            Debug.DrawLine(backRay.origin, new Vector3(backRay.origin.x, backRay.origin.y - GroundRayDistance, backRay.origin.z), Color.gray, 0.05f);
            return false;
        }
    }
    [SerializeField] private float Gravity = -15.0f;
    [SerializeField] private float VerticalVelocity;
    private float TerminalVelocity = 53.00f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(Player controller)
    {
        Player = controller;
        Rigidbody = GetComponent<Rigidbody>();
        body = transform.Find("Colliders").Find("Body").GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsCam(Player.ControlsInput.moveInput);
    }

    public void ApplyGravity()
    {
        // Apply gravity over time if under terminal (max) velocity
        if(VerticalVelocity < TerminalVelocity)
        {
            VerticalVelocity += Gravity * Time.deltaTime;
        }

        // stop vertical velocity from dropping infinitely when grounded
        if(VerticalVelocity < 0.00f)
        {
            VerticalVelocity = -2.00f;
        }
    }

    public void MoveTowardsCam(Vector2 direction)
    {
        ApplyGravity();

        Vector3 moveDirection = new Vector3(direction.x, VerticalVelocity, direction.y);

        Rigidbody.velocity = moveDirection * Time.deltaTime;
    }
    
// end
}
