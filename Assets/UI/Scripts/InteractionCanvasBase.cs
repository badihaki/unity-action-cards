using TMPro;
using UnityEngine;

public class InteractionCanvasBase : MonoBehaviour
{
	protected Interaction _Interaction;
	private string _InteractionName;
	
	public virtual void Initialize(Interaction interaction, string interactionName)
	{
		_Interaction = interaction;
		_InteractionName = interactionName;
		transform.Find("BasePanel").Find("btn_Interact").GetComponentInChildren<TextMeshProUGUI>().text = _InteractionName;
	}

	public virtual void OnInteractBtnClilcked() => _Interaction.RunInteraction();

	public virtual void CancelInteraction()
	{
		GameManagerMaster.Player._InteractionController.StopPlayerInteraction();
		_Interaction.StopInteraction();
	}

	// end
}
