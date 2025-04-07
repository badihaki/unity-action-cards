using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create new Interaction(base)/talk", fileName = "interaction_talk_")]
public class InteractionTalk : InteractionScriptableObj
{
	public List<DialogueScriptableObj> dialoguesList;

	public override void Interact(Character initiator)
	{
		base.Interact(initiator);
	}
}
