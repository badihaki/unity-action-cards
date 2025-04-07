using UnityEngine;

[CreateAssetMenu(menuName = "Create new Interaction(base)/teleport",fileName ="interaction_teleport")]
public class InteractionTeleport : InteractionScriptableObj
{
    [Header("For teleportation")]
    public int sceneIndex;

	public override void Interact(Character initiator)
	{
		base.Interact(initiator);
		GameManagerMaster.GameMaster.SceneManager.ChangeScene(sceneIndex);
	}
}
