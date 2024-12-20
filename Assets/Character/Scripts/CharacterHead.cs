using UnityEngine;

public class CharacterHead : MonoBehaviour
{
	[field:SerializeField]
	private Character character;
	[field:SerializeField]
	private Actor actor;
	private Ray detectRayMid;
	private RaycastHit hit;
	[SerializeField]
	private float rayLength = 0.782f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character = GetComponentInParent<Character>();
		actor = character.GetComponentInChildren<Actor>();
		detectRayMid = new Ray(transform.position, transform.up);
		rayLength = 0.715f;
		Debug.DrawRay(transform.position, transform.TransformDirection(transform.up) * rayLength, Color.green, 10.0f);
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

	private void OnTriggerEnter(Collider other)
	{
		print($"{other.name} with parent {other.transform.parent.name} is being detected on the head of {character.name}");
		Character colIsCharacter = other.GetComponentInParent<Character>();
		GameObject debubObj = new GameObject($"{gameObject.transform.parent.transform.parent.name} Head Collision Location");
		debubObj.AddComponent<DebugLocationHelper>();
		debubObj.transform.position = gameObject.transform.position;
		Destroy(debubObj, 10.5f);

		//if (colIsCharacter != null && colIsCharacter != character)
		//{
		//	print("trig");
		//}
		if (colIsCharacter != null)
		{
			//print("trig");
			//print(colIsCharacter.name);
			if(colIsCharacter != character)
			{
				//print("trig OTHER!!");
				print(colIsCharacter.name);
				float xOffset;
				float zOffset;

				if (colIsCharacter.transform.position.x > transform.position.x)
					xOffset = 0.075f;
				else
					xOffset = -0.075f;

				if (colIsCharacter.transform.position.z > transform.position.z)
					zOffset = -0.075f;
				else
					zOffset = 0.075f;

				Vector3 offset = new Vector3(xOffset, 0.0f, zOffset);
				colIsCharacter.transform.Translate(offset);
			}
		}
	}

	// end
}
