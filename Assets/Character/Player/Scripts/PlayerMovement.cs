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

    public void Initialize(PlayerCharacter controller)
    {
        _Player = controller;
        _Rigidbody = GetComponent<Rigidbody>();
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

    public void ZeroOutVelocity() => _Rigidbody.velocity = Vector3.zero;

    public void MoveTowardsCam(Vector2 direction)
    {
        if (direction == Vector2.zero) _MoveDirection = Vector2.zero;
        else
        {
            RotateCharacter(direction);
            _MoveDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        }
        
        // Vector3 desiredMoveDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        // moveDirection = new Vector3(desiredMoveDirection.x, desiredMoveDirection.y + VerticalVelocity, desiredMoveDirection.z);
        // Rigidbody.velocity = moveDirection * Player.CharacterSheet.WalkSpeed * Time.deltaTime;

        float movementSpeed = _Player._Controls._RunInput ? _Player._CharacterSheet._RunSpeed : _Player._CharacterSheet._WalkSpeed;
        // Rigidbody.velocity = new Vector3(moveDirection.x * movementSpeed, moveDirection.y, moveDirection.z * movementSpeed) * Time.deltaTime;
        _Rigidbody.velocity = new Vector3(_MoveDirection.x * movementSpeed, _MoveDirection.y, _MoveDirection.z * movementSpeed);

        ApplyGravity();
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
        if (_Player._CheckGrounded.IsGrounded())
        {
            // _VerticalVelocity = Mathf.Sqrt(_Player._CharacterSheet._JumpPower * _BaseVerticalVelocity * _Gravity);
            _VerticalVelocity = Mathf.Sqrt((_Player._CharacterSheet._JumpPower * _BaseVerticalVelocity) * _Gravity);
        }
    }

    // end
}
