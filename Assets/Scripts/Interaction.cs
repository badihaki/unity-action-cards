using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour, IInteractable
{
	[SerializeField]
	private Transform controllingEntity;
	public Transform GetControllingEntity()
	{
		return controllingEntity;
	}
	[SerializeField]
	private InteractionScriptableObj interaction;
	[SerializeField]
	private InteractionCanvasBase interactionCanvas;



	private void Start()
	{
		interactionCanvas = GetComponentInChildren<InteractionCanvasBase>();
		interactionCanvas.Initialize(this);
		interactionCanvas.gameObject.SetActive(false);
	}

	public void Interact(Character interactingCharacter)
	{
		interaction.Interact(interactingCharacter);
		interactionCanvas.gameObject.SetActive(true);
	}

	/// <summary>
	/// Basic way to stop an interaction
	/// </summary>
	public void StopInteraction() => interactionCanvas.gameObject.SetActive(false);
	/// <summary>
	/// Stop an interaction with a  specific character
	/// </summary>
	/// <param name="interactingCharacter">Pass in a character to have it run logic additional logic on that character</param>
	public void StopInteraction(Character interactingCharacter)
	{
		interactionCanvas.gameObject.SetActive(false);
	}

	// end
}
