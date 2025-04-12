using System;
using UnityEngine;

public interface IInteractable
{
	void Interact(Character interactingCharacter);
	void StopInteraction(Character interactingCharacter);
	Transform GetControllingEntity();
}
