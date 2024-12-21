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

	private void FixedUpdate()
	{
		//entityOnHead = DetectActor();
	}

	private void OnTriggerStay(Collider other)
	{
		//Character touchingChar = other.GetComponentInParent<Character>();
		//print(other.name);
		Actor touchingChar = other.GetComponentInParent<Actor>();
		if (touchingChar != null && touchingChar != actor)
		{
			print($"{character} touching {touchingChar} through {other.name}");
		}
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
