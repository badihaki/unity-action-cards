using UnityEngine;

public class InteractionCanvasBase : MonoBehaviour
{
	Interaction _Interaction;
	
	public virtual void Initialize(Interaction interaction)
	{
		_Interaction = interaction;
	}

	public void CancelInteraction()
	{
		GameManagerMaster.Player._InteractionController.StopPlayerInteraction();
		_Interaction.StopInteraction();
	}

	// end
}
