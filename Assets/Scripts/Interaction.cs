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
	[field: SerializeField]
	public InteractionScriptableObj interactionScrObj { get; protected set; }
	[SerializeField]
	private InteractionCanvasBase interactionCanvas;
	[SerializeField]
	private Character interactingCharacter;



	private void Start()
	{
		interactionCanvas = GetComponentInChildren<InteractionCanvasBase>();
		interactionCanvas.Initialize(this, interactionScrObj.interactionName);
		interactionCanvas.gameObject.SetActive(false);
	}

	public void Interact(Character interactionInitiator)
	{
		interactingCharacter = interactionInitiator;
		interactionCanvas.gameObject.SetActive(true);
	}

	public virtual void RunInteraction()
	{
		interactionScrObj.Interact(interactingCharacter);
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
