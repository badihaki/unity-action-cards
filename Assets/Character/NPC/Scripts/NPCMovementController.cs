using UnityEngine;
using UnityEngine.AI;

public class NPCMovementController : MonoBehaviour
{
    private NonPlayerCharacter _NPC;
    [field: SerializeField, Header("Components")]
	public CharacterController _CharacterController { get; private set; }
	
	[field: Header("Forces"), SerializeField]
    public Vector3 _ExternalForces { get; private set; }
	[SerializeField] private float _Gravity = 9.810f;
	[SerializeField] private float _VerticalVelocity;
	[SerializeField] private float _BaseVerticalVelocity = -0.1f;
	[SerializeField] private float _MaxFallVelocity = -25.00f; // terminal velocity

	[field: SerializeField, Header("Nav Agent Stuff"),]
	private NavMeshAgent _NavAgent;
	[SerializeField]
	private Vector3 _IntendedVelocity;

	[field: SerializeField, Header("Knockback")]
	public Vector3 knockBackSource = Vector3.zero;
	[field: SerializeField]
	public Vector3 knockBackDir = Vector3.zero;
	[field: SerializeField]
	public float knockBackForce = 0;

	public void InitializeNPCMovement(NonPlayerCharacter character)
	{
		_NPC = character;
		_CharacterController = _NPC._Actor.GetComponent<CharacterController>();
		_NavAgent = _NPC._Actor.GetComponent<NavMeshAgent>();
		_ExternalForces = Vector3.zero;
		SetAgentUpdates(false);
	}

	public void SetAgentUpdates(bool value)
	{
		_NavAgent.updatePosition = value;
		_NavAgent.updateRotation = value;
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

	public void ZeroOutMovement()
	{
		_CharacterController.Move(Vector3.zero);
		_NavAgent.isStopped = true;
		SyncAgentVelToCharControllerVel();
	}

	public void SetExternalForces(Transform forceSource, float knockforce, float launchForce)
	{
		if (forceSource != _NPC.transform)
		{
			Vector3 direction = (transform.position - forceSource.position).normalized;
			Vector3 force = new Vector3(direction.x, direction.y * launchForce, direction.z * knockforce);
			_ExternalForces = force;
		}
		else _ExternalForces = Vector3.zero;
	}

	public void AddToExternalForces(Vector3 force)
	{
		if (force != Vector3.zero)
			_ExternalForces += force;
		else
			_ExternalForces = Vector3.zero;
	}

	public void GetPushedBack(Vector3 pushFromPoint, float pushBackForce = 0, bool isLaunched = false)
	{
		if (knockBackSource != pushFromPoint && pushFromPoint != Vector3.zero)
		{
			if (GameManagerMaster.GameMaster.GMSettings.logExraPlayerData)
				print(">> Setting pushed back stuff");
			knockBackSource = pushFromPoint;
			knockBackForce = pushBackForce;
			knockBackDir = knockBackSource - _NPC._NPCActor.transform.position;
			knockBackDir = -knockBackDir.normalized;
			if (!isLaunched)
			{
				knockBackDir.y = 1;
			}
			else
			{
				if (knockBackDir.y < 0)
					knockBackDir.y *= -1;
			}
		}
		_CharacterController.Move(knockBackDir * knockBackForce);
	}

	public void ResetPushback()
	{
		knockBackSource = Vector3.zero;
		knockBackForce = 0;
		knockBackDir = Vector3.zero;
	}

	public void ApplyExternalForces()
	{
		_CharacterController.Move(_ExternalForces * Time.deltaTime);
		SyncAgentVelToCharControllerVel();
	}

	public void MoveToCurrentNavNode()
	{
		//Vector3 direction = (_Navigator._CurrentNavNode.transform.position - _NPC._Actor.transform.position).normalized;
		Vector3 direction = _NavAgent.desiredVelocity.normalized;
		ApplyGravity(1);
		direction.y = _VerticalVelocity;
		_CharacterController.Move((direction * _NPC._CharacterSheet._WalkSpeed) * Time.deltaTime);
		if (_NavAgent.isStopped)
			_NavAgent.isStopped = false; // make sure nav agent is not stopped
		SyncAgentVelToCharControllerVel();
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

	private void SyncAgentVelToCharControllerVel()
	{
		_NavAgent.velocity = _CharacterController.velocity;
		//if (GameManagerMaster.GameMaster.GMSettings.logExtraNPCData)
		//	print($"{transform.name} is syncing agent velocity {_NavAgent.velocity.ToString()} to character controller velocity {_CharacterController.velocity}");
	}
	public void SetAgentDestination(Vector3? destination, float desiredDistance = 0.0f)
	{
		//if (GameManagerMaster.GameMaster.GMSettings.logExtraNPCData)
		//	print($"Setting destination to {destination.ToString()}");
		if (destination != null)
			_NavAgent.destination = destination.Value;
		else
		{
			_NavAgent.destination = _NPC._NPCActor.transform.position;
			_NavAgent.stoppingDistance = desiredDistance;
		}
	}

	public void MoveToTarget()
	{
		//Vector3 direction = (_Navigator._Target.transform.position - _NPC._Actor.transform.position).normalized;
		Vector3 direction = _NavAgent.desiredVelocity.normalized;
		ApplyGravity(1);
		direction.y = _VerticalVelocity;
		_CharacterController.Move((direction * _NPC._CharacterSheet._WalkSpeed) * Time.deltaTime);
		if (_NavAgent.isStopped)
		{
			if (GameManagerMaster.GameMaster.GMSettings.logNPCNavData)
				print("!!!!!!!!!!!!!! Nav agent was stopped, but is being started again");
			_NavAgent.isStopped = false; // make sure nav agent is not stopped
		}
		else
		{
			if(GameManagerMaster.GameMaster.GMSettings.logNPCNavData)
				print("!!!!!!!!!!!!!! Nav agent was turned on, nothing happened");
		}
		SyncAgentVelToCharControllerVel();
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
		float backForceVelocity = _NPC._NPCActor.transform.forward.z * Vector3.back.z;
		
		if (force.z > 0)
		{
			print($"backwards force is {backForceVelocity}");
			force.z *= backForceVelocity;
		}
		else
		{
			print(Time.deltaTime);
			float newVerticalVel = _VerticalVelocity * Time.deltaTime;
			force.z = backForceVelocity * _VerticalVelocity > 0 ? newVerticalVel : newVerticalVel * -1;
		}
		_ExternalForces = force;
	}

	public void FarKnocBack(Vector3 fromPosition)
	{
		// how far high we goin?
		_VerticalVelocity = Mathf.Sqrt(GameManagerMaster.GameMaster.GeneralConstantVariables.FarKnockBackForce.y);
		Vector3 backwardsDir = -_NPC._NPCActor.transform.forward;
		if (GameManagerMaster.GameMaster.GMSettings.logExtraNPCData)
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
