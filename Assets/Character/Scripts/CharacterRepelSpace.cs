using UnityEngine;

public class CharacterRepelSpace : MonoBehaviour
{
	[field:SerializeField]
	private Character character;
	[field:SerializeField]
	private Actor actor;
	//private Ray detectRayMid;
	private RaycastHit hit;
	[SerializeField]
	private float detectionRayDistance = -1.0f;
	[SerializeField]
	private Collider body;
	[SerializeField]
	private LayerMask layerMask;
	[field:SerializeField]
	private Transform entityOnHead;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character = GetComponentInParent<Character>();
		actor = character.GetComponentInChildren<Actor>();
		//detectRayMid = new Ray(transform.position, transform.up);
		detectionRayDistance = -0.430f;
		layerMask = LayerMask.GetMask("Character");
		body = GetComponent<Collider>();
		Debug.DrawRay(transform.position, transform.TransformDirection(transform.up) * detectionRayDistance, Color.green, 10.0f);
	}

	//private void FixedUpdate()
	//{
	//	detectRayMid.origin = transform.position;
	//	detectRayMid.direction = transform.up;
	//	Vector3 up = transform.TransformDirection(Vector3.up) * rayLength;
	//	//if (Physics.Raycast(detectRay, out RaycastHit hitInfo, rayLength, 3))
	//	if (Physics.Raycast(detectRayMid, out hit, rayLength))
	//	{
	//		Actor entityToBeRepelled = hit.transform.GetComponentInParent<Actor>();
	//		if(entityToBeRepelled != null)
	//		{
	//			float xOffset;
	//			float zOffset;

	//			if (entityToBeRepelled.transform.position.x > transform.position.x)
	//				xOffset = 0.075f;
	//			else
	//				xOffset = -0.075f;

	//			if (entityToBeRepelled.transform.position.z > transform.position.z)
	//				zOffset = -0.075f;
	//			else
	//				zOffset = 0.075f;

	//			Vector3 offset = new Vector3(xOffset, 0.0f, zOffset);
	//			entityToBeRepelled.transform.Translate(offset);

	//			print("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
	//			print($"ray hit info for {character.name}:");
	//			print($"repelling {entityToBeRepelled.transform.parent.name}'s actor {entityToBeRepelled.name}");
	//			print("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
	//		}
	//	}
	//	Debug.DrawRay(transform.position, transform.transform.up * rayLength, Color.red);
	//	Debug.DrawRay(transform.position, transform.TransformDirection(transform.up) * rayLength, Color.magenta);
	//	//Debug.DrawLine(transform.position, transform.transform.up * rayLength, Color.blue, 0.01f);
	//	//Debug.DrawLine(transform.position, transform.TransformDirection(transform.up) * rayLength, Color.blue, 0.01f);
	//}

	//private void OnTriggerEnter(Collider other)
	//{
	//	print($"{other.name} with parent {other.transform.parent.name} is being detected on the head of {character.name}");
	//	Character colIsCharacter = other.GetComponentInParent<Character>();
	//	GameObject debubObj = new GameObject($"{gameObject.transform.parent.transform.parent.name} Head Collision Location");
	//	debubObj.AddComponent<DebugLocationHelper>();
	//	debubObj.transform.position = gameObject.transform.position;
	//	Destroy(debubObj, 10.5f);

	//	//if (colIsCharacter != null && colIsCharacter != character)
	//	//{
	//	//	print("trig");
	//	//}
	//	if (colIsCharacter != null)
	//	{
	//		//print("trig");
	//		//print(colIsCharacter.name);
	//		if(colIsCharacter != character)
	//		{
	//			//print("trig OTHER!!");
	//			print(colIsCharacter.name);
	//			float xOffset;
	//			float zOffset;

	//			if (colIsCharacter.transform.position.x > transform.position.x)
	//				xOffset = 0.075f;
	//			else
	//				xOffset = -0.075f;

	//			if (colIsCharacter.transform.position.z > transform.position.z)
	//				zOffset = -0.075f;
	//			else
	//				zOffset = 0.075f;

	//			Vector3 offset = new Vector3(xOffset, 0.0f, zOffset);
	//			colIsCharacter.transform.Translate(offset);
	//		}
	//	}
	//}

	private void FixedUpdate()
	{
		entityOnHead = DetectActor();
	}

	public Transform DetectActor(float extraDetectionRayLength = 0.0f)
	{
		Ray midRay = new Ray(transform.position, -transform.up);
		Ray leftRay = new Ray(new Vector3(transform.position.x - (body.bounds.extents.x-0.5f), transform.position.y, transform.position.z), -transform.up);
		Ray rightRay = new Ray(new Vector3(transform.position.x + (body.bounds.extents.x+0.5f), transform.position.y, transform.position.z), -transform.up);
		Ray frontRay = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z + (body.bounds.extents.z+0.5f)), -transform.up);
		Ray backRay = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z - (body.bounds.extents.z-0.5f)), -transform.up);
		if (Physics.Raycast(midRay, out hit, detectionRayDistance + extraDetectionRayLength, layerMask))
		{
			Debug.DrawLine(midRay.origin, new Vector3(midRay.origin.x, midRay.origin.y - (detectionRayDistance + extraDetectionRayLength), midRay.origin.z), Color.red, 0.05f);
			Debug.Log(hit.transform);
			return hit.transform;
		}
		if (Physics.Raycast(leftRay, out hit, detectionRayDistance + extraDetectionRayLength, layerMask))
		{
			Debug.DrawLine(leftRay.origin, new Vector3(leftRay.origin.x, leftRay.origin.y - (detectionRayDistance + extraDetectionRayLength), leftRay.origin.z), Color.red, 0.05f);
			Debug.Log(hit.transform);
			return hit.transform;
		}
		if (Physics.Raycast(rightRay, out hit, detectionRayDistance + extraDetectionRayLength, layerMask))
		{
			Debug.DrawLine(rightRay.origin, new Vector3(rightRay.origin.x, rightRay.origin.y - (detectionRayDistance + extraDetectionRayLength), rightRay.origin.z), Color.red, 0.05f);
			Debug.Log(hit.transform);
			return hit.transform;
		}
		if (Physics.Raycast(frontRay, out hit, detectionRayDistance + extraDetectionRayLength, layerMask))
		{
			Debug.DrawLine(frontRay.origin, new Vector3(frontRay.origin.x, frontRay.origin.y - (detectionRayDistance + extraDetectionRayLength), frontRay.origin.z), Color.red, 0.05f);
			Debug.Log(hit.transform);
			return hit.transform;
		}
		if (Physics.Raycast(backRay, out hit, detectionRayDistance + extraDetectionRayLength, layerMask))
		{
			Debug.DrawLine(backRay.origin, new Vector3(backRay.origin.x, backRay.origin.y - (detectionRayDistance + extraDetectionRayLength), backRay.origin.z), Color.red, 0.05f);
			Debug.Log(hit.transform);
			return hit.transform;
		}
		else
		{
			Debug.DrawLine(midRay.origin, new Vector3(midRay.origin.x, midRay.origin.y - (detectionRayDistance + extraDetectionRayLength), midRay.origin.z), Color.gray, 0.05f);
			Debug.DrawLine(leftRay.origin, new Vector3(leftRay.origin.x, leftRay.origin.y - (detectionRayDistance + extraDetectionRayLength), leftRay.origin.z), Color.gray, 0.05f);
			Debug.DrawLine(rightRay.origin, new Vector3(rightRay.origin.x, rightRay.origin.y - (detectionRayDistance + extraDetectionRayLength), rightRay.origin.z), Color.gray, 0.05f);
			Debug.DrawLine(frontRay.origin, new Vector3(frontRay.origin.x, frontRay.origin.y - (detectionRayDistance + extraDetectionRayLength), frontRay.origin.z), Color.gray, 0.05f);
			Debug.DrawLine(backRay.origin, new Vector3(backRay.origin.x, backRay.origin.y - (detectionRayDistance + extraDetectionRayLength), backRay.origin.z), Color.gray, 0.05f);
			return null;
		}
	}

	// end
}
