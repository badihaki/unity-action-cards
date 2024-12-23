using System;
using System.Collections;
using UnityEngine;

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
    private float rotationSmoothingTime = 0.082f;
    private float targetRotation;
    private float lerpSpeedOnSlowDown = 0.5f;

    [Header("Aim Rotation"), SerializeField]
    private float aimRotationSpeed = 2.0f;

	private Camera cam;

    [field: Header("In Air Schmoovement"), SerializeField]
    public bool canDoubleJump { get; private set; }
    [field: SerializeField] public bool canAirDash { get; private set; }
    private WaitForSeconds airSchmooveWait = new WaitForSeconds(0.15f);
    [field: SerializeField] public Vector3 _ExternalForces { get; private set; }


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

    public void AddToExternalForces(Vector3 force)
    {
		if (force != Vector3.zero)
			_ExternalForces += force;
		else
			_ExternalForces = Vector3.zero;
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
			rotationSmoothingTime = 0.061f;
		}
		else if (!_CheckForGround.IsGrounded())
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

    public void RotateWhileAiming(Vector2 aimInput)
    {
        //Vector3 desiredRotation = new Vector3(cam.transform.rotation.x, cam.transform.rotation.y, cam.transform.rotation.z);
        Quaternion desiredRotation = _Player._CameraController.cinemachineCamTarget.rotation;
        desiredRotation.x = 0;
        desiredRotation.z = 0;
		//_Player._CameraController.cinemachineCamTarget.transform.Rotate(0, (aimRotationSpeed * aimInput.x), 0);
		//_Actor.transform.rotation = desiredRotation;

		_Actor.transform.rotation = Quaternion.Lerp(_Actor.transform.rotation, desiredRotation, aimRotationSpeed);
		//_Actor.transform.rotation = Quaternion.Lerp(_Actor.transform.rotation, _Player._CameraController.cinemachineCamTarget.rotation, aimRotationSpeed);
	}

    private void ApplyDesiredMoveToMovement()
    {
        _MovementDirection = _DesiredMoveDirection * movementSpeed; // we need to make sure movement is equal to the speed we're trying to achieve
        _MovementDirection.y = _VerticalVelocity;
    }
	private void ApplyExternalForcesToMovement()
	{
        _MovementDirection += _ExternalForces;
	}

    public void MoveWithVerticalVelocity()
    {
        if (_Player._CheckGrounded.IsGrounded())
            targetSpeed = _Player._Controls._RushInput ? _Player._CharacterSheet._RunSpeed : _Player._CharacterSheet._WalkSpeed;
        else
            targetSpeed = _Player._CharacterSheet._RunSpeed;
        ApplyDesiredMoveToMovement();
        ApplyExternalForcesToMovement();
        _Controller.Move(_MovementDirection * Time.deltaTime);
    }


	public void Jump(float modifier = 1.0f)
    {
        if (_Player._CheckGrounded.IsGrounded())
        {
            _VerticalVelocity = Mathf.Sqrt((_Player._CharacterSheet._JumpPower * 2) * modifier);
        }
        else
        {
            _VerticalVelocity = Mathf.Sqrt((_Player._CharacterSheet._JumpPower * 3.245f) * modifier);
        }
    }
    public void SetDoubleJump(bool value) => canDoubleJump = value;
    public void SetAirDash(bool value) => canAirDash = value;
    public void ResetAllAirSchmoovement() => StartCoroutine(ResetAirSchmoovement());
    private IEnumerator ResetAirSchmoovement()
    {
		canAirDash = false;
		canDoubleJump = false;
		yield return airSchmooveWait;
        canAirDash = true;
        canDoubleJump = true;
    }

	// end
}
