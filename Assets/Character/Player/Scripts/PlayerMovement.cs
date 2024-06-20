using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerCharacter _Player;
    private Rigidbody _Rigidbody;
    
    [SerializeField] private float _Gravity = -15.0f;
    [SerializeField] private float _VerticalVelocity;
    [SerializeField] private float _BaseVerticalVelocity = -2.00f;
    [SerializeField] private float _MaxVertVelocity = -150.00f;
    private float terminalVelocity = 53.00f;
    [SerializeField]private Vector3 _MoveDirection;
    private float rotationVelocity;
    private float rotationSmoothingTime = 0.15f;
    private float targetRotation;
    [field: SerializeField] public float movementSpeed { get; private set; }
    private float targetSpeed;
    private float lerpSpeedOnMovement = 0.085f;
    private float lerpSpeedOnSlowDown = 0.5f;


    public void Initialize(PlayerCharacter controller)
    {
        _Player = controller;
        _Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        /*print("Speed normalized = " + _Rigidbody.velocity.normalized.x);
        print("Speed??? = " + targetSpeed/movementSpeed);*/
    }

    public void ApplyGravity()
    {
        // stop vertical velocity from dropping infinitely when grounded
        if (_Player._CheckGrounded.IsGrounded())
        {
            if(_VerticalVelocity < 0.00f)
            {
                _VerticalVelocity = _BaseVerticalVelocity;
            }
        }
        else
        {
            // Apply gravity over time if under terminal (max) velocity
            if (_VerticalVelocity < terminalVelocity)
            {
                _VerticalVelocity += _Gravity * Time.deltaTime;

                if (_VerticalVelocity < _MaxVertVelocity) _VerticalVelocity = _MaxVertVelocity;
            }
        }
        _Rigidbody.velocity = new Vector3(_Rigidbody.velocity.x, _Rigidbody.velocity.y + _VerticalVelocity, _Rigidbody.velocity.z);
    }

    public void ZeroOutVelocity()
    {
        movementSpeed = 0.0f;
        targetSpeed = 0.0f;
        _Rigidbody.velocity = Vector3.zero;
        _Player._AnimationController.SetFloat("speed", 0.0f);
    }

    public void SlowDown()
    {
        if (movementSpeed < 0.1f) ZeroOutVelocity();
        targetSpeed = 0;
        movementSpeed = Mathf.Lerp(movementSpeed, targetSpeed, lerpSpeedOnSlowDown);

        ApplyMovementToVelocity();
        _Player._AnimationController.SetFloat("speed", Mathf.InverseLerp(0, targetSpeed, movementSpeed));
        // print("slowing down");
    }

    public void MoveTowardsCamWithGravity(Vector2 direction, bool isAiming)
    {
        if (direction == Vector2.zero) _MoveDirection = Vector2.zero;
        else
        {
            if (!isAiming) RotateCharacter(direction);
            _MoveDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        }

        if (!isAiming) targetSpeed = _Player._Controls._RunInput ? _Player._CharacterSheet._RunSpeed : _Player._CharacterSheet._WalkSpeed;
        else targetSpeed = _Player._CharacterSheet._WalkSpeed * 0.45f;
        movementSpeed = Mathf.Lerp(movementSpeed, targetSpeed, lerpSpeedOnMovement);


        _Player._AnimationController.SetFloat("speed", Mathf.InverseLerp(_Player._CharacterSheet._WalkSpeed, _Player._CharacterSheet._RunSpeed, movementSpeed));
        ApplyMovementToVelocity();
        ApplyGravity();
    }

    private void ApplyMovementToVelocity()
    {
        _Rigidbody.velocity = new Vector3(_MoveDirection.x * movementSpeed, _MoveDirection.y, _MoveDirection.z * movementSpeed);
    }

    public void RotateCharacter(Vector2 inputDirection)
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
        if (_Player._CheckGrounded.IsGrounded())
        {
            // _VerticalVelocity = Mathf.Sqrt((_Player._CharacterSheet._JumpPower * _BaseVerticalVelocity) * _Gravity);
            _VerticalVelocity = Mathf.Sqrt(_Player._CharacterSheet._JumpPower);
        }
    }

    // end
}
