using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player Player;
    private Rigidbody Rigidbody;
    private Collider body;
    [SerializeField]private float GroundRayDistance = 0.500f;
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
    public bool Grounded;
    [SerializeField] private float Gravity = -15.0f;
    [SerializeField] private float VerticalVelocity;
    [SerializeField] private float BaseVerticalVelocity = -2.00f;
    [SerializeField] private float maxVertVelocity = -150.00f;
    private float TerminalVelocity = 53.00f;
    [SerializeField]private Vector3 moveDirection;
    private float rotationVelocity;
    private float rotationSmoothingTime = 0.15f;
    private float targetRotation;

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
        Grounded = IsGrounded();
    }
    private void FixedUpdate()
    {
        /*
         * DELETE This shit below:
        */
        MoveTowardsCam(Player.ControlsInput.moveInput);
        if (Player.ControlsInput.jump)
        {
            Jump();
        }
        ApplyGravity();
    }

    public void ApplyGravity()
    {
        // stop vertical velocity from dropping infinitely when grounded
        if (IsGrounded())
        {
            if(VerticalVelocity < 0.00f)
            {
                VerticalVelocity = BaseVerticalVelocity;
            }
        }
        else
        {
            // Apply gravity over time if under terminal (max) velocity
            if (VerticalVelocity < TerminalVelocity)
            {
                VerticalVelocity += Gravity * Time.deltaTime;

                if (VerticalVelocity < maxVertVelocity) VerticalVelocity = maxVertVelocity;
            }
        }
        // Rigidbody.velocity = new Vector3(0.0f, VerticalVelocity, 0.0f);
        Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, Rigidbody.velocity.y + VerticalVelocity, Rigidbody.velocity.z);
    }

    public void MoveTowardsCam(Vector2 direction)
    {
        RotateCharacter(direction);
        moveDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        if (direction == Vector2.zero) moveDirection = Vector2.zero;
        float movementSpeed = Player.ControlsInput.run ? Player.CharacterSheet.RunSpeed : Player.CharacterSheet.WalkSpeed;
        // Vector3 desiredMoveDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        // moveDirection = new Vector3(desiredMoveDirection.x, desiredMoveDirection.y + VerticalVelocity, desiredMoveDirection.z);

        // Rigidbody.velocity = moveDirection * Player.CharacterSheet.WalkSpeed * Time.deltaTime;
        Rigidbody.velocity = new Vector3(moveDirection.x * movementSpeed, moveDirection.y, moveDirection.z * movementSpeed) * Time.deltaTime;
    }

    private void RotateCharacter(Vector2 inputDirection)
    {
        // target rotation is the intended vector we want to rotate to
        targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y)
            * Mathf.Rad2Deg
            + Camera.main.transform.eulerAngles.y; // we take the rotation of the camera into consideration
        // rotation direction determines which direction we move to reach our intended rotation
        float rotationDirection = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothingTime);
        // actually rotating the transform
        transform.rotation = Quaternion.Euler(0.0f, rotationDirection, 0.0f);
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            VerticalVelocity = Mathf.Sqrt(Player.CharacterSheet.JumpPower * BaseVerticalVelocity * Gravity);
        }
    }

    // end
}
