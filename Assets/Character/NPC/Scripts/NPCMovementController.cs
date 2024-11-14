using UnityEngine;

public class NPCMovementController : MonoBehaviour
{
    private NonPlayerCharacter _Character;
    [field: SerializeField, Header("Components")]
	public CharacterController _CharacterController { get; private set; }
	
	[field: SerializeField]
	private NPCNavigator _Navigator;
	
	[field: Header("Forces"), SerializeField]
    public Vector3 _ExternalForces { get; private set; }
	[SerializeField] private float _Gravity = 9.810f;
	[SerializeField] private float _VerticalVelocity;
	[SerializeField] private float _BaseVerticalVelocity = -1.00f;
	[SerializeField] private float _MaxFallVelocity = -25.00f; // terminal velocity

	public void InitializeNPCMovement(NonPlayerCharacter character)
    {
        _Character = character;
        _CharacterController = _Character._Actor.GetComponent<CharacterController>();
		_Navigator = _Character._NavigationController;
        _ExternalForces = Vector3.zero;
    }

	public void ApplyGravity(float gravityModifier = 1.0f)
	{
		gravityModifier = Mathf.Clamp(gravityModifier, 0.1f, 3.0f);
		if (_Character._CheckGrounded.IsGrounded())
		{
			_VerticalVelocity = _BaseVerticalVelocity;
		}
		else
		{
			_VerticalVelocity -= (_Gravity * (Time.deltaTime * 2.25f)) * gravityModifier;
			if (_VerticalVelocity < _MaxFallVelocity) _VerticalVelocity = _MaxFallVelocity;
			Vector3 forceModifier = _ExternalForces;
			forceModifier.y = _VerticalVelocity;
			_ExternalForces = forceModifier;
		}
	}

	public void ZeroOutMovement() => _CharacterController.Move(Vector3.zero);

	public void SetExternalForces(Transform forceSource, float knockforce, float launchForce)
	{
		if (forceSource != _Character.transform)
		{
			Vector3 direction = (transform.position - forceSource.position).normalized;
			// Vector3 force = new Vector3(0, launchForce, knockforce);
			//direction *= -1;
			Vector3 force = new Vector3(direction.x, direction.y * launchForce, direction.z * knockforce);
			//_CharacterController.Move(direction * Mathf.Sqrt(knockforce)); // was I using the wrong value all along??
			//_CharacterController.Move(force);

			//print($"{name} is KNOCKBACKED in direction >> {direction} with force {force} resulting in {direction * Mathf.Sqrt(knockforce)} <<");
			_ExternalForces = force;
		}
		else _ExternalForces = Vector3.zero;
	}

	public void ApplyExternalForces()
	{
		_CharacterController.Move(_ExternalForces * Time.deltaTime);
	}

	public void MoveToCurrentNavNode()
	{
		Vector3 direction = (_Navigator._CurrentNavNode.transform.position - _Character._Actor.transform.position).normalized;
		ApplyGravity(1);
		direction.y = _VerticalVelocity;
		_CharacterController.Move((direction * _Character._CharacterSheet._WalkSpeed) * Time.deltaTime);
	}

	public void RotateTowardsTarget(Transform target)
	{
		float rotationSmoothTime = 5.50f;
		Vector3 targetRotation = target.position - _Character._NPCActor.transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(targetRotation);
		lookRotation.x = 0;
		lookRotation.z = 0;
		_Character._NPCActor.transform.rotation = Quaternion.Slerp(_Character._NPCActor.transform.rotation, lookRotation, Time.deltaTime * rotationSmoothTime);
	}

	public void MoveToTarget()
	{
		Vector3 direction = (_Navigator._Target.transform.position - _Character._Actor.transform.position).normalized;
		ApplyGravity(1);
		direction.y = _VerticalVelocity;
		_CharacterController.Move((direction * _Character._CharacterSheet._WalkSpeed) * Time.deltaTime);
	}
}
