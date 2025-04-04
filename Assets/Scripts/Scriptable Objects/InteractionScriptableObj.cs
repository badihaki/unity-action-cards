using System;
using UnityEngine;

//[CreateAssetMenu(menuName = "Create new Interaction(base)/[interaction]",fileName ="interaction_")]
public class InteractionScriptableObj : ScriptableObject
{
	public string interactionName;
	public virtual void Interact(Character initiator)
	{
		Debug.Log($"Interacting || {interactionName}(interactioon)");
	}
}
