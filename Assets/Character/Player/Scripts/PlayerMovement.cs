using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    private PlayerCharacter _Player;
    private PlayerActor _Actor;
    private CharacterController _Controller;
    private CheckForGround _CheckForGround;

    [Header("Movement")]
    [SerializeField] private float _Gravity = 9.810f;
    [SerializeField] private float _VerticalVelocity;
    [SerializeField] private float _BaseVerticalVelocity = -1.00f;
    [SerializeField] private float _MaxFallVelocity = -25.00f; // terminal velocity
    [SerializeField] private Vector3 _DesiredMoveDirection;
    [SerializeField] private Vector3 _MovementDirection = new Vector3();
    [field: SerializeField] public float movementSpeed { get; private set; }
    [field: SerializeField] public float targetSpeed { get; private set; }
    private float lerpSpeedOnMovement = 0.085f;

    [Header("Rotation")]
    private float rotationVelocity;
    private float rotationSmoothingTime = 0.35f;
    private float targetRotation;
    private float lerpSpeedOnSlowDown = 0.5f;
    private Camera cam;

    [Header("In Air Schmoovement")]
    [field: SerializeField] public bool canDoubleJump { get; private set; }
    [field: SerializeField] public bool canAirDash { get; private set; }
    public void Initialize(PlayerCharacter controllingPlayer)
    {
        _Player = controllingPlayer;
        _Actor = controllingPlayer._PlayerActor;
        _Controller = _Player._Actor.GetComponent<CharacterController>();
        _CheckForGround = _Actor.GetComponent<CheckForGround>();
        cam = Camera.main;

        canAirDash = true;
        canDoubleJump = true;
    }

    private void Update()
    {
    }

    public void ApplyGravity(float gravityModifier = 1.0f)
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

    public void ZeroOutVertVelocity()
    {
        ZeroOutVelocity();
        _Controller.Move(Vector3.zero);
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
        if (_CheckForGround.IsGrounded() && movementSpeed > 5.5f)
        {
            rotationSmoothingTime = 0.15f; 
        }
        else if(!_CheckForGround.IsGrounded())
        {
            rotationSmoothingTime = 0.45f;
        }
        float rotationDirection = Mathf.SmoothDampAngle(_Actor.transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothingTime);
        // actually rotating the transform
        // transform.rotation = Quaternion.Euler(0.0f, rotationDirection, 0.0f);
        _Actor.transform.rotation = Quaternion.Euler(0.0f, rotationDirection, 0.0f);
    }
    public void RotateInstantly(Vector2 inputDirection)
    {
        print($"rotating to this input direction {inputDirection}");
        targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y)
    * Mathf.Rad2Deg
    + cam.transform.eulerAngles.y; // we take the rotation of the camera into consideration
        // rotation direction determines which direction we move to reach our intended rotation
        if (_CheckForGround.IsGrounded() && movementSpeed > 5.5f)
        {
            rotationSmoothingTime = 0.15f;
        }
        else if (!_CheckForGround.IsGrounded())
        {
            rotationSmoothingTime = 0.45f;
        }
        float rotationDirection = Mathf.SmoothDampAngle(_Actor.transform.eulerAngles.y, targetRotation, ref rotationVelocity, 0);
        // actually rotating the transform
        // transform.rotation = Quaternion.Euler(0.0f, rotationDirection, 0.0f);
        _Actor.transform.rotation = Quaternion.Euler(0.0f, rotationDirection, 0.0f);
    }

    public void RotateTowardsTarget(Vector3 targetPos)
    {
        print($"rotating towards target position {targetPos} || playerMovement.rotateTowardsTarget");
        // Vector3 rotation = Vector3.RotateTowards(_Actor.transform.position, targetPos - _Actor.transform.position, Time.deltaTime, 0.0f);
        // Vector3 direction = targetPos - _Player._PlayerSpells._spellOrigin.transform.position;

        // _Actor.transform.localRotation = Quaternion.LookRotation(rotation); // this causes the x to get rotated.
        // _Actor.transform.eulerAngles = new Vector3(0, rotation.y, 0);
        // _Actor.transform.localRotation = Quaternion.Euler(rotation);
        // print($"target to look at {direction}");
        
        // _Actor.transform.LookAt(targetPos);
        // _Player._PlayerSpells._spellOrigin.LookAt(targetPos);
        
        // _Actor.transform.eulerAngles = new Vector3(0.0f, _Actor.transform.rotation.y, 0.0f);
        
        StartCoroutine(RotateTowards(targetPos, 12));
    }

    private IEnumerator RotateTowards(Vector3 targetPos, float deg)
    {
        Vector3 dirToLookAt = (targetPos - _Actor.transform.position).normalized;
        float targetAngle = Mathf.Atan2(dirToLookAt.x, dirToLookAt.z) * Mathf.Rad2Deg;

        while(Mathf.Abs(Mathf.DeltaAngle(_Actor.transform.eulerAngles.y, targetAngle)) > Mathf.Epsilon)
        {
            float angle = Mathf.MoveTowardsAngle(_Actor.transform.eulerAngles.y, targetAngle, deg);
            _Actor.transform.eulerAngles = Vector3.up * angle;
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
        if (_Player._CheckGrounded.IsGrounded())
            targetSpeed = _Player._Controls._RushInput ? _Player._CharacterSheet._RunSpeed : _Player._CharacterSheet._WalkSpeed;
        else
            targetSpeed = _Player._CharacterSheet._RunSpeed;
        ApplyDesiredMoveToMovement();
        _Controller.Move(_MovementDirection * Time.deltaTime);
    }

    public void Jump()
    {
        if (_Player._CheckGrounded.IsGrounded())
        {
            _VerticalVelocity = Mathf.Sqrt(_Player._CharacterSheet._JumpPower * 2);
        }
        else
        {
            print("air jumping");
            _VerticalVelocity = Mathf.Sqrt(_Player._CharacterSheet._JumpPower * 3.245f);
        }
    }
    public void SetDoubleJump(bool value) => canDoubleJump = value;
    public void SetAirDash(bool value) => canAirDash = value;

    // end
}
