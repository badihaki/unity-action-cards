using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardManagementCanvas : InteractionCanvasBase
{
	[field: SerializeField, Header("Card Management Stuff")]
	private CardDeckManagementBtn cardDeckManagementBtnTemplate;
	[field: SerializeField]
	public GameObject managementGroup { get; private set; }
	public GameObject cardContentArea { get; private set; }
	[SerializeField]
	private List<CardDeckManagementBtn> cardButtons;
	//
	private GameObject cardDetailsPanel;
	[SerializeField, Header("Card Details Panel")]
	private int activeCardId;
	[SerializeField]
	private bool showingCardDetails = false;
	private TextMeshProUGUI cardDetailsTitle;
	private TextMeshProUGUI cardDetailsCost;
	private Image cardDetailsImage;

	public override void Initialize(Interaction interaction, string interactionName)
	{
		base.Initialize(interaction, interactionName);

		managementGroup = transform.Find("ManagementGroup").gameObject;
		cardContentArea = managementGroup.transform.Find("CardInventoryGroup").Find("Scroll View").Find("Viewport").Find("Content").gameObject;
		cardDetailsPanel = managementGroup.transform.Find("CardDetailPanel").gameObject;
		cardDetailsTitle = cardDetailsPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>();
		cardDetailsCost = cardDetailsPanel.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
		cardDetailsImage = cardDetailsPanel.transform.Find("Image").GetComponent<Image>();

		SetShowCardDetailsPanel(false, activeCardId);
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
		if (!showingCardDetails)
		{
			SetShowCardDetailsPanel(true, btnId);
		}
		else
		{
			if (activeCardId != btnId)
				SetBasicCardDetails(btnId);
			else
				RemoveCardDetails();
		}
	}

	private void SetShowCardDetailsPanel(bool value, int btnId)
	{
		showingCardDetails = value;
		cardDetailsPanel.SetActive(value);
		if (showingCardDetails)
		{
			SetBasicCardDetails(btnId);
		}
	}

	private void RemoveCardDetails()
	{
		SetShowCardDetailsPanel(false, activeCardId);
	}
	private void SetBasicCardDetails(int btnId)
	{
		activeCardId = btnId;
		CardScriptableObj card = GameManagerMaster.GameMaster.CardsManager.cardsFound[activeCardId].CardScriptableObj;
		cardDetailsTitle.text = card._CardName;
		cardDetailsCost.text = card._CardCost.ToString();
		cardDetailsImage.sprite = card._CardImage;
	}

	// end
}
