using System;
using System.Collections.Generic;
using UnityEngine;

public class GMCardsManager : MonoBehaviour
{
	[SerializeField]
	private bool devMove = true;
	[field: SerializeField]
	public List<CardStruct> cardsFound { get; private set; }
	[field: SerializeField]
	private CardScriptableObj[] starterCards;

	private void Start()
	{
		if (devMove)
		{
			cardsFound = new List<CardStruct>();
			List<CardScriptableObj> cardsInNewDeck = new List<CardScriptableObj>();
            foreach (CardScriptableObj card in starterCards)
            {
				CardStruct cardStruct = new CardStruct(card, true);
				cardStruct.AddCopy();
				cardsInNewDeck.Add(card);
				cardStruct.AddCopy();
				cardsInNewDeck.Add(card);
				cardStruct.TryAddCopyInDeck();
				cardStruct.TryAddCopyInDeck();
				cardsFound.Add(cardStruct);
			}
			//GameManagerMaster.Player._PlayerCards.RebuildDeck(cardsInNewDeck);
			GameObject.Find("Player").GetComponent<PlayerCards>().RebuildDeck(cardsInNewDeck);
        }
	}

	// end
}

[Serializable]
public struct CardStruct
{
	public CardScriptableObj CardScriptableObj;
	public bool isUnlocked;
	public int copiesOwned;
	public int copiesInDeck;

	public CardStruct(CardScriptableObj cardScriptableObj, bool isInstantlyUnlocked)
	{
		CardScriptableObj = cardScriptableObj;
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
	public bool TryAddCopyInDeck()
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
		if (copiesInDeck > 0)
		{
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
