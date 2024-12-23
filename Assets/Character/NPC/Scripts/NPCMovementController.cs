using UnityEngine;

public class NPCMovementController : MonoBehaviour
{
    private NonPlayerCharacter _NPC;
    [field: SerializeField, Header("Components")]
	public CharacterController _CharacterController { get; private set; }
	
	[field: SerializeField]
	private NPCNavigator _Navigator;
	
	[field: Header("Forces"), SerializeField]
    public Vector3 _ExternalForces { get; private set; }
	[SerializeField] private float _Gravity = 9.810f;
	[SerializeField] private float _VerticalVelocity;
	[SerializeField] private float _BaseVerticalVelocity = -0.1f;
	[SerializeField] private float _MaxFallVelocity = -25.00f; // terminal velocity

	public void InitializeNPCMovement(NonPlayerCharacter character)
    {
        _NPC = character;
        _CharacterController = _NPC._Actor.GetComponent<CharacterController>();
		_Navigator = _NPC._NavigationController;
        _ExternalForces = Vector3.zero;
    }

	public void ApplyGravity(float gravityModifier = 1.0f)
	{
		gravityModifier = Mathf.Clamp(gravityModifier, 0.1f, 3.0f); // 3 is the highest it should and and that's a LOT of gravity
		if (_NPC._CheckGrounded.IsGrounded())
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
		if (forceSource != _NPC.transform)
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

	public void AddToExternalForces(Vector3 force) => _ExternalForces += force;

	public void ApplyExternalForces()
	{
		_CharacterController.Move(_ExternalForces * Time.deltaTime);
	}

	public void MoveToCurrentNavNode()
	{
		Vector3 direction = (_Navigator._CurrentNavNode.transform.position - _NPC._Actor.transform.position).normalized;
		ApplyGravity(1);
		direction.y = _VerticalVelocity;
		_CharacterController.Move((direction * _NPC._CharacterSheet._WalkSpeed) * Time.deltaTime);
	}

	public void RotateTowardsTarget(Transform target)
	{
		float rotationSmoothTime = 5.50f;
		Vector3 targetRotation = target.position - _NPC._NPCActor.transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(targetRotation);
		lookRotation.x = 0;
		lookRotation.z = 0;
		_NPC._NPCActor.transform.rotation = Quaternion.Slerp(_NPC._NPCActor.transform.rotation, lookRotation, Time.deltaTime * rotationSmoothTime);
	}

	public void MoveToTarget()
	{
		Vector3 direction = (_Navigator._Target.transform.position - _NPC._Actor.transform.position).normalized;
		ApplyGravity(1);
		direction.y = _VerticalVelocity;
		_CharacterController.Move((direction * _NPC._CharacterSheet._WalkSpeed) * Time.deltaTime);
	}

	public void Jump()
	{
		if (_NPC._CheckGrounded.IsGrounded())
		{
			_VerticalVelocity = Mathf.Sqrt(_NPC._CharacterSheet._JumpPower * 2);
		}
		else
		{
			_VerticalVelocity = Mathf.Sqrt(_NPC._CharacterSheet._JumpPower * 3.245f);
		}
		Vector3 force = _ExternalForces;
		force.y += _VerticalVelocity;
		_ExternalForces = force;
	}

	public void Launch()
	{
		_VerticalVelocity = Mathf.Sqrt(GameManagerMaster.GameMaster.GeneralConstantVariables.CharacterLaunchForce);
		Vector3 force = _ExternalForces;
		force.y += _VerticalVelocity;
		_ExternalForces = force;
	}

	public void FarKnocBack(Vector3 fromPosition)
	{
		// how far high we goin?
		_VerticalVelocity = Mathf.Sqrt(GameManagerMaster.GameMaster.GeneralConstantVariables.FarKnockBackForce.y);
		Vector3 backwardsDir = -_NPC._NPCActor.transform.forward;
		if (GameManagerMaster.GameMaster.logExtraNPCData)
			print($">>>>>>>>>>> backwards direction is >>> {backwardsDir.ToString()}");
		Vector3 extForceCopy = _ExternalForces;
		extForceCopy.y += _VerticalVelocity;
		// how far back we goin??
		extForceCopy.z += GameManagerMaster.GameMaster.GeneralConstantVariables.FarKnockBackForce.x;
		Vector3 forcedDirection = new Vector3(backwardsDir.x, extForceCopy.y, -backwardsDir.z * extForceCopy.z);
		_ExternalForces = forcedDirection;
		// lets check out ideas using a ray
		Vector3 headingTo = fromPosition - _NPC._NPCActor.transform.position;
		float distance = headingTo.magnitude;
		Vector3 direction = headingTo / distance;
		Debug.DrawRay(transform.position, direction, Color.red);
	}
}
