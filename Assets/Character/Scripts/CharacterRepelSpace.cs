using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRepelSpace : MonoBehaviour
{
	[field:SerializeField]
	private Character character;
	[field:SerializeField]
	private List<Character> charactersOnHead;
	private WaitForSeconds repelWait = new WaitForSeconds(0.178f);


	public void Initialize(Character newChar)
	{
		character = newChar;

		character._Actor.onDeath += CleanUp;
	}

	public void CleanUp()
	{
		charactersOnHead.Clear();
	}

	private void FixedUpdate()
	{
		if (character != null && charactersOnHead.Count > 0)
		{
			charactersOnHead.ForEach(character =>
			{
				Vector3 force = Vector3.zero;
				if (transform.position.z > character._Actor.transform.position.z)
					force.z = -0.1f;
				else
					force.z = 0.1f;
				if (transform.position.x > character._Actor.transform.position.x)
					force.x = -0.1f;
				else
					force.x = 0.1f;

				character.AddToExternalForce(force);
			});
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Hurtbox")
		{
			Character touchingChar = other.GetComponentInParent<Character>();
			//Actor touchingChar = other.GetComponentInParent<Actor>();
			if (touchingChar != null && touchingChar != character)
			{
				//print($"{character} touching {touchingChar} through {other.name}");
				if (!charactersOnHead.Contains(touchingChar))
					charactersOnHead.Add(touchingChar);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.name == "Hurtbox")
		{
			Character touchingChar = other.GetComponentInParent<Character>();
			//Actor touchingChar = other.GetComponentInParent<Actor>();
			if (charactersOnHead.Contains(touchingChar))
			{
				charactersOnHead.Remove(touchingChar);
				StartCoroutine(StopRepelFromCharacter(touchingChar));
			}
		}
	}

	private IEnumerator StopRepelFromCharacter(Character character)
	{
		yield return repelWait;
		character.AddToExternalForce(Vector3.zero);
	}

	// end
}
