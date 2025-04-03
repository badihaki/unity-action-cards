using UnityEngine;

[CreateAssetMenu(menuName = "Create new Interaction(base)/teleport",fileName ="interaction_teleport")]
public class interaction_Teleport : InteractionScriptableObj
{
    [Header("For teleportation")]
    public int sceneIndex;

	public override void Interact(Character initiator)
	{
		base.Interact(initiator);
	}
}
