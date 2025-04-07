using TMPro;
using UnityEngine;

public class InteractionCanvasBase : MonoBehaviour
{
	protected Interaction _Interaction;
	private string _InteractionName;
	protected GameObject basePanel;
	public bool isActivated { get; private set; }

	public virtual void Initialize(Interaction interaction, string interactionName)
	{
		_Interaction = interaction;
		isActivated = false;
		_InteractionName = interactionName;
		basePanel = transform.Find("BasePanel").gameObject;
		basePanel.transform.Find("btn_Interact").GetComponentInChildren<TextMeshProUGUI>().text = _InteractionName;
	}

	public virtual void OnInteractBtnClilcked()
	{
		_Interaction.RunInteraction();
		isActivated = true;
	}

	public virtual void CancelInteraction()
	{
		GameManagerMaster.Player._InteractionController.StopPlayerInteraction();
		gameObject.SetActive(false);
		_Interaction.StopInteraction();
	}

	// end
}
