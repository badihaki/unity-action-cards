using System;
using UnityEngine;

public interface IInteractable
{
	void Interact(Character interactingCharacter);
	Transform GetControllingEntity();

}
