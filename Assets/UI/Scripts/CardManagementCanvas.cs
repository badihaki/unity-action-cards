using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManagementCanvas : InteractionCanvasBase
{
	[field: SerializeField, Header("Card Management Stuff")]
	private CardDeckManagementBtn cardDeckManagementBtnTemplate;
	[field: SerializeField]
	public GameObject managementGroup { get; private set; }
	[field: SerializeField]
	public GameObject cardContentArea { get; private set; }
	[SerializeField]
	private List<CardDeckManagementBtn> cardButtons;

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
		BuildContentArea();
	}

	private void BuildContentArea()
	{
   //     foreach (CardStruct cardStruct in GameManagerMaster.GameMaster.CardsManager.cardsFound)
   //     {
			//CardDeckManagementBtn btn = Instantiate(cardDeckManagementBtnTemplate, cardContentArea.transform);
			//btn.Initialize(cardStruct.CardScriptableObj, this);
			//cardButtons.Add(btn);
   //     }
		for (int i = 0; i < GameManagerMaster.GameMaster.CardsManager.cardsFound.Count; i++)
		{
			CardStruct cardStruct = GameManagerMaster.GameMaster.CardsManager.cardsFound[i];
			CardDeckManagementBtn btn = Instantiate(cardDeckManagementBtnTemplate, cardContentArea.transform);
			btn.Initialize(cardStruct.CardScriptableObj, this, i);
			cardButtons.Add(btn);
		}
    }

	private void DestroyContentArea()
	{
        foreach (CardDeckManagementBtn card in cardButtons)
        {
            Destroy(card.gameObject);
        }
		cardButtons.Clear();
    }

	public override void CancelInteraction()
	{
		basePanel.SetActive(true);
		managementGroup.SetActive(false);
		DestroyContentArea();
		base.CancelInteraction();
	}
	public void ClickedCardIcon(int btnId)
	{
		print($"clicking card {GameManagerMaster.GameMaster.CardsManager.cardsFound[btnId].CardScriptableObj._CardName}");
	}

	// end
}
