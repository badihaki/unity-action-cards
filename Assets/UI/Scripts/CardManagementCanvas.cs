using UnityEngine;

public class CardManagementCanvas : InteractionCanvasBase
{
	[field: SerializeField, Header("Card Management Stuff")]
	private CardDeckManagementBtn cardDeckManagementBtnTemplate;
	[field: SerializeField]
	public GameObject managementGroup { get; private set; }
	[field: SerializeField]
	public GameObject cardContentArea { get; private set; }

	public override void Initialize(Interaction interaction, string interactionName)
	{
		base.Initialize(interaction, interactionName);

		managementGroup = transform.Find("ManagementGroup").gameObject;
		cardContentArea = managementGroup.transform.Find("CardInventoryGroup").Find("Scroll View").Find("Viewport").Find("Content").gameObject;
		managementGroup.SetActive(false);
	}

	public override void OnInteractBtnClilcked()
	{
		base.OnInteractBtnClilcked();

		basePanel.SetActive(false);
		managementGroup.SetActive(true);
	}

	public override void CancelInteraction()
	{
		basePanel.SetActive(true);
		managementGroup.SetActive(false);

		base.CancelInteraction();
	}

	// end
}
