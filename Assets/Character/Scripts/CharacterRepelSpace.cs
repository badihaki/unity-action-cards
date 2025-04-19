using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRepelSpace : MonoBehaviour
{
	[field:SerializeField]
	private Character character;
	[field:SerializeField]
	private List<Character> charactersOnHead;
	private WaitForSeconds repelWait = new WaitForSeconds(0.257f);


	public void Initialize(Character _character)
	{
		character = _character;
		character._Actor.onDeath += CleanUp;
	}

	private void OnEnable()
	{
		if (character != null)
			character._Actor.onDeath += CleanUp;
	}
	private void OnDisable()
	{
		if (character != null)
			character._Actor.onDeath -= CleanUp;
	}

	public void CleanUp(Character character)
	{
		charactersOnHead.Clear();
	}

	private void FixedUpdate()
	{
		if (character != null && charactersOnHead.Count > 0)
		{
			RepelCharacters();
		}
	}

	private void RepelCharacters()
	{
		charactersOnHead.ForEach(character =>
		{
			if(character._Actor.transform.position.y > character._Actor.transform.position.y + 0.5f)
			{
				character.PushBackCharacter(transform.position, 1f);
			}
		});
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
