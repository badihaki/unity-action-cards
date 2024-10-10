using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerCharacter _Player;
    private CharacterController _Controller;
    private CheckForGround _CheckForGround;
    
    [SerializeField] private float _Gravity = 9.810f;
    [SerializeField] private float _VerticalVelocity;
    [SerializeField] private float _BaseVerticalVelocity = -1.00f;
    [SerializeField] private float _MaxFallVelocity = -25.00f; // terminal velocity
    [SerializeField] private Vector3 _DesiredMoveDirection;
    [SerializeField] private Vector3 _MovementDirection = new Vector3();


    private float rotationVelocity;
    private float rotationSmoothingTime = 0.35f;
    [SerializeField] float attackRotationSmoothTime = 0.825f;
    private float targetRotation;
    private float attackRotationTimer;
    [field: SerializeField] public float movementSpeed { get; private set; }
    [field: SerializeField] private float targetSpeed;
    private float lerpSpeedOnMovement = 0.085f;
    private float lerpSpeedOnSlowDown = 0.5f;
    private Camera cam;


    public void Initialize(PlayerCharacter controllingPlayer)
    {
        _Player = controllingPlayer;
        _Controller = _Player._Actor.GetComponent<CharacterController>();
        _CheckForGround = _Player._PlayerActor.GetComponent<CheckForGround>();
        cam = Camera.main;
    }

    private void Update()
    {
        
    }

    public void ApplyGravity(float gravityModifier)
    {
        gravityModifier = Mathf.Clamp(gravityModifier, 0.1f, 3.0f);
        if (_CheckForGround.IsGrounded())
        {
            _VerticalVelocity = _BaseVerticalVelocity;
        }
        else
        {
            _VerticalVelocity -= (_Gravity * (Time.deltaTime * 2.25f)) * gravityModifier;
            if ( _VerticalVelocity < _MaxFallVelocity) _VerticalVelocity = _MaxFallVelocity;
        }
    }

    public void ZeroOutVelocity()
    {
        movementSpeed = 0.0f;
        targetSpeed = 0.0f;
        _MovementDirection = Vector3.zero;
        _DesiredMoveDirection = Vector3.zero;
        _Player._AnimationController.SetFloat("speed", 0.0f);
    }

    public void SlowDown()
    {
        // print("slowing down");
        if (movementSpeed < 0.1f) ZeroOutVelocity();
        targetSpeed = 0;
        SetMovementSpeed();

        _Player._AnimationController.SetFloat("speed", Mathf.InverseLerp(0, targetSpeed, movementSpeed));
    }

    public void DetectMove(Vector2 moveInput)
    {
        if (moveInput == Vector2.zero) _DesiredMoveDirection = Vector3.zero;
        else
        {
            // RotateCharacter(moveInput);
            _DesiredMoveDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        }
        SetMovementSpeed();
        _Player._AnimationController.SetFloat("speed", Mathf.InverseLerp(_Player._CharacterSheet._WalkSpeed, _Player._CharacterSheet._RunSpeed, movementSpeed));

    }

    private void SetMovementSpeed()
    {
        movementSpeed = Mathf.Lerp(movementSpeed, targetSpeed, lerpSpeedOnMovement);
    }

    public void MoveWhileAiming()
    {
        /*
        if (direction == Vector2.zero) _MoveDirection = Vector2.zero;
        else
        {
            _MoveDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        }

        targetSpeed = _Player._CharacterSheet._WalkSpeed * 0.45f;
        movementSpeed = Mathf.Lerp(movementSpeed, targetSpeed, lerpSpeedOnMovement);


        _Player._AnimationController.SetFloat("speed", Mathf.InverseLerp(_Player._CharacterSheet._WalkSpeed, _Player._CharacterSheet._RunSpeed, movementSpeed));
        ApplyMovementToVelocity();
        ApplyGravity();
        */
    }

    public void RotateCharacter(Vector2 inputDirection)
    {
        // target rotation is the intended vector we want to rotate to
        targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y)
            * Mathf.Rad2Deg
            + cam.transform.eulerAngles.y; // we take the rotation of the camera into consideration
        // rotation direction determines which direction we move to reach our intended rotation
        float rotationDirection = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothingTime);
        // actually rotating the transform
        transform.rotation = Quaternion.Euler(0.0f, rotationDirection, 0.0f);
    }

    public void SetAttackRotationTime() => attackRotationTimer = attackRotationSmoothTime;
    public IEnumerator RotateCharacterWhileAttacking(Vector2 inputDirection)
    {
        while(attackRotationTimer > 0.0f)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y)
                * Mathf.Rad2Deg
                + cam.transform.eulerAngles.y;
            // rotation direction determines which direction we move to reach our intended rotation
            float newRotationVel = rotationVelocity / 2;
            float rotationDirection = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref newRotationVel, attackRotationSmoothTime);
            // actually rotating the transform
            // transform.rotation = Quaternion.Euler(0.0f, rotationDirection, 0.0f);
            _Player._PlayerActor.transform.rotation = Quaternion.Euler(0.0f, rotationDirection, 0.0f);
            attackRotationTimer -= Time.deltaTime;
            yield return null;
        }
    }

    public void ApplyDesiredMoveToMovement()
    {
        _MovementDirection = _DesiredMoveDirection * movementSpeed; // we need to make sure movement is equal to the speed we're trying to achieve
        _MovementDirection.y = _VerticalVelocity;
    }

    public void MoveWithVerticalVelocity()
    {
        targetSpeed = _Player._Controls._RushInput ? _Player._CharacterSheet._RunSpeed : _Player._CharacterSheet._WalkSpeed;
        ApplyDesiredMoveToMovement();
        _Controller.Move(_MovementDirection * Time.deltaTime);
        
    }

    public void Jump()
    {
        if (_Player._CheckGrounded.IsGrounded())
        {
            _VerticalVelocity = Mathf.Sqrt(_Player._CharacterSheet._JumpPower * 2);
        }
    }

    // end
}
