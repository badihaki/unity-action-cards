using UnityEngine;

[CreateAssetMenu(menuName = "Create new Interaction(base)/talk", fileName = "interaction_talk")]
public class interaction_talk : InteractionScriptableObj
{
	public override void Interact(Character initiator)
	{
		base.Interact(initiator);
	}
}
