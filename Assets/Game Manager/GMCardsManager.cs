using System;
using System.Collections.Generic;
using UnityEngine;

public class GMCardsManager : MonoBehaviour
{
	[SerializeField]
	private bool devMove = true;
	[field: SerializeField]
	public List<CardSave> cardsFound { get; private set; }
	[field: SerializeField]
	private CardScriptableObj[] starterCards;
	[field: SerializeField]
	private bool hasBuiltNewStarterDeck = false;

	private void Start()
	{
		if (devMove && !hasBuiltNewStarterDeck)
		{
			//playerCards.RemoveAllCards();
			cardsFound = new List<CardSave>();

			List<CardScriptableObj> cardsInNewDeck = new List<CardScriptableObj>();
            foreach (CardScriptableObj card in starterCards)
            {
				CardSave cardSave = new CardSave(card, true);
				cardSave.AddCopy();
				cardSave.AddCopy();
				cardSave.TryAddCopyToDeck();
				cardSave.TryAddCopyToDeck();
				cardsInNewDeck.Add(card);
				cardsInNewDeck.Add(card);
				cardsFound.Add(cardSave);
			}
			print($"congrats, your new deck has {cardsInNewDeck.Count} cards in it!!");
			PlayerCards playerCards = GameObject.Find("Player").GetComponent<PlayerCards>();
			playerCards.RebuildDeck(cardsInNewDeck);
			hasBuiltNewStarterDeck = true;
		}
	}

	public bool TryAddCardtoDeck(int cardId)
	{
		return cardsFound[cardId].TryAddCopyToDeck();
	}
	public bool TryRemoveCardFromDeck(int cardId)
	{
		return cardsFound[cardId].TryRemoveCopyFromDeck();
	}

	// end
}

[Serializable]
public class CardSave
{
	[field: SerializeField]
	public CardScriptableObj cardScriptableObj { get; private set; }
	[field: SerializeField]
	public bool isUnlocked { get; private set; }
	[field: SerializeField]
	public int copiesOwned { get; private set; }
	[field: SerializeField]
	public int copiesInDeck { get; private set; }

	public CardSave(CardScriptableObj cardScriptableObj, bool isInstantlyUnlocked)
	{
		this.cardScriptableObj = cardScriptableObj;
		if (isInstantlyUnlocked)
		{
			isUnlocked = true;
			copiesOwned = 1;
			copiesInDeck = 0;
		}
		else
		{
			isUnlocked = false;
			copiesOwned = 0;
			copiesInDeck = 0;
		}
	}

	#region Utility Methods
	public int AddCopy()
	{
		copiesOwned++;
		if(!isUnlocked && copiesOwned >= 5)
		{
			UnlockCard();
		}
		return copiesOwned;
	}

	#region Adding and Removing from Deck
	public bool TryAddCopyToDeck()
	{
		if (copiesInDeck < 2 && copiesInDeck + 1 <= copiesOwned)
		{
			copiesInDeck++;
			return true;
		}
		return false;
	}
	public bool TryRemoveCopyFromDeck()
	{
		Debug.Log("removing from deck");
		if (copiesInDeck > 0)
		{
			Debug.Log("Yes!!");
			copiesInDeck--;
			return true;
		}
		return false;
	}
	#endregion
	public void UnlockCard() => isUnlocked = true;
	#endregion

	// end
}
