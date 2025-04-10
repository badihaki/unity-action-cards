using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardManagementCanvas : InteractionCanvasBase
{
	[field: SerializeField, Header("Card Management Stuff")]
	private CardDeckManagementBtn cardDeckManagementBtnTemplate;
	[field: SerializeField]
	public GameObject managementGroup { get; private set; }
	public GameObject cardInventoryArea { get; private set; }
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
	public GameObject addBtn;
	public GameObject removeBtn;
	private TextMeshProUGUI totalCopies;
	private TextMeshProUGUI copiesInDeck;
	[field: SerializeField, Header("Deck Area")]
	private GameObject deckPanel;
	private Transform deckContent;
	[field:SerializeField]
	private List<CardDeckManagementBtn> deckCardBtns;

	public override void Initialize(Interaction interaction, string interactionName)
	{
		base.Initialize(interaction, interactionName);

		managementGroup = transform.Find("ManagementGroup").gameObject;
		cardInventoryArea = managementGroup.transform.Find("CardInventoryGroup").Find("Scroll View").Find("Viewport").Find("Content").gameObject;
		cardDetailsPanel = managementGroup.transform.Find("CardDetailPanel").gameObject;
		cardDetailsTitle = cardDetailsPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>();
		cardDetailsCost = cardDetailsPanel.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
		cardDetailsImage = cardDetailsPanel.transform.Find("Image").GetComponent<Image>();
		addBtn = cardDetailsPanel.transform.Find("Add").gameObject;
		removeBtn = cardDetailsPanel.transform.Find("Remove").gameObject;
		totalCopies = cardDetailsPanel.transform.Find("TotalCopies").GetComponent<TextMeshProUGUI>();
		copiesInDeck = cardDetailsPanel.transform.Find("CopiesInDeck").GetComponent<TextMeshProUGUI>();
		deckPanel = transform.Find("DeckPanel").gameObject;
		deckContent = deckPanel.transform.Find("Viewport").Find("Content");
		deckPanel.SetActive(false);
		SetShowCardDetailsPanel(false, activeCardId);
		managementGroup.SetActive(false);
	}

	public override void OnInteractBtnClilcked()
	{
		base.OnInteractBtnClilcked();

		GameManagerMaster.Player._PlayerCards.ReturnAllCardsToDeck();
		basePanel.SetActive(false);
		managementGroup.SetActive(true);
		BuildCardInventoryList();
		BuildDeckPanel();
	}

	private void BuildCardInventoryList()
	{
		for (int i = 0; i < GameManagerMaster.GameMaster.CardsManager.cardsFound.Count; i++)
		{
			CardSave cardSave = GameManagerMaster.GameMaster.CardsManager.cardsFound[i];
			CardDeckManagementBtn btn = Instantiate(cardDeckManagementBtnTemplate, cardInventoryArea.transform);
			btn.Initialize(cardSave.cardScriptableObj, this, i);
			btn.GetComponent<Button>().onClick.AddListener(() => ClickedCardIcon(btn.btnId));
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
		deckPanel.SetActive(false);
		deckCardBtns.ForEach(cardBtn => cardBtn.gameObject.SetActive(false));
		DestroyContentArea();
		GameManagerMaster.Player._PlayerCards.DrawFullHand();
		base.CancelInteraction();
	}
	public void ClickedCardIcon(int btnId)
	{
		print($"clicking card with id {btnId}");
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
			UpdateActiveCardInDeckAmt();
		}
	}

	private void RemoveCardDetails()
	{
		SetShowCardDetailsPanel(false, activeCardId);
	}
	private void SetBasicCardDetails(int btnId)
	{
		activeCardId = btnId;
		CardScriptableObj card = GameManagerMaster.GameMaster.CardsManager.cardsFound[activeCardId].cardScriptableObj;
		cardDetailsTitle.text = card._CardName;
		cardDetailsCost.text = card._CardCost.ToString();
		cardDetailsImage.sprite = card._CardImage;
		SetAddRemoveBtn();
	}

	public void SetAddRemoveBtn()
	{
		CardSave card = GameManagerMaster.GameMaster.CardsManager.cardsFound[activeCardId];
		if (card.copiesInDeck >= 2 || card.copiesInDeck >= card.copiesOwned)
		{
			addBtn.SetActive(false);
			removeBtn.SetActive(true);
		}
		else if (card.copiesInDeck < 2)
		{
			addBtn.SetActive(true);
			if (card.copiesInDeck == 0)
				removeBtn.SetActive(false);
			else
				removeBtn.SetActive(true);
		}
		
	}

	public void OnClickAddCardBtn()
	{
		print($"add card {activeCardId} to deck");
		if (GameManagerMaster.GameMaster.CardsManager.TryAddCardtoDeck(activeCardId))
		{
			GameManagerMaster.Player._PlayerCards.AddCardToDeck(GameManagerMaster.GameMaster.CardsManager.cardsFound[activeCardId].cardScriptableObj);
			UpdateDeckPanel();
			UpdateActiveCardInDeckAmt();
			SetAddRemoveBtn();
		}
	}

	public void OnClickRemoveBtn()
	{
		print($"remove {activeCardId} to deck");
		if (GameManagerMaster.GameMaster.CardsManager.TryRemoveCardFromDeck(activeCardId))
		{
			GameManagerMaster.Player._PlayerCards.RemoveCardFromDeck(GameManagerMaster.GameMaster.CardsManager.cardsFound[activeCardId].cardScriptableObj);
			UpdateDeckPanel();
			UpdateActiveCardInDeckAmt();
			SetAddRemoveBtn();
		}
	}

	private void UpdateActiveCardInDeckAmt()
	{
		CardSave card = GameManagerMaster.GameMaster.CardsManager.cardsFound[activeCardId];
		copiesInDeck.text = card.copiesInDeck.ToString();
		totalCopies.text = card.copiesOwned.ToString();
	}

	#region Deck Panel
	public void BuildDeckPanel()
	{
		deckPanel.SetActive(true);
		print("we are building the deck panel here");
		//for (int i = 0; i < GameManagerMaster.GameMaster.CardsManager.cardsFound.Count; i++)
		print($"Deck panel should have {GameManagerMaster.Player._PlayerCards._Deck.Count} objects in it");
        for (int i = 0; i < GameManagerMaster.Player._PlayerCards._Deck.Count; i++)
			{
			CardDeckManagementBtn btn = Instantiate(cardDeckManagementBtnTemplate, deckContent.transform);
			int cardIndex = GameManagerMaster.GameMaster.CardsManager.cardsFound.FindIndex(card => card.cardScriptableObj == GameManagerMaster.Player._PlayerCards._Deck[i]);
			btn.Initialize(GameManagerMaster.Player._PlayerCards._Deck[i], this, cardIndex);
			btn.GetComponent<Button>().onClick.AddListener(() => ClickedCardIcon(btn.btnId));
			deckCardBtns.Add(btn);
		}
	}

	private void UpdateDeckPanel()
	{
        deckCardBtns.ForEach(cardBtn=> Destroy(cardBtn.gameObject));
		deckCardBtns.Clear();
		BuildDeckPanel();
    }
	#endregion

	// end
}
