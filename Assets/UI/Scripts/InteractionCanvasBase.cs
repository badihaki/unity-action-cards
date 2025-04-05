using TMPro;
using UnityEngine;

public class InteractionCanvasBase : MonoBehaviour
{
	Interaction _Interaction;
	private string _InteractionName;
	
	public virtual void Initialize(Interaction interaction, string interactionName)
	{
		_Interaction = interaction;
		_InteractionName = interactionName;
		transform.Find("btn_Interact").GetComponentInChildren<TextMeshProUGUI>().text = _InteractionName;
	}

	public void RunInteraction() => _Interaction.RunInteraction();

	public void CancelInteraction()
	{
		GameManagerMaster.Player._InteractionController.StopPlayerInteraction();
		_Interaction.StopInteraction();
	}

	// end
}
