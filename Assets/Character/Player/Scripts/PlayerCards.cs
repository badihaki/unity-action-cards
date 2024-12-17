using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class PlayerCards : MonoBehaviour
{
    private PlayerCharacter player;

    [Tooltip("The Deck is where all the player's usable are kept.")]
    [field: SerializeField] public List<CardScriptableObj> _Deck { get; private set; }
    
    [Tooltip("When a card is used, it goes to the Abyss")]
    [field: SerializeField] public List<CardScriptableObj> _Abyss { get; private set; }
    
    [Tooltip("The player's hand is where the active cards are stored")]
    [field: SerializeField] public List<CardScriptableObj> _Hand { get; private set; }
    [field: SerializeField, Header("In-game")] private GameObject cardPrefab;
    [SerializeField] private GameObject handOfCards;
    private float radius = 300.00f;
    [SerializeField] private bool isShowingHand;

    [field: SerializeField, Header("Drawing Cards")]
    private float timeTillDrawNewCard = 5.52f;
    [field: SerializeField]
    private float drawCardTimer = 0.0f;
    [field: SerializeField]
    private bool drawTimerActivated = false;
    public delegate void ChangeDeckCount(int value);
    public event ChangeDeckCount onDeckCountChanged;
    
    [Header("Deck Recharging")]
    private WaitForSeconds deckRechargeTime = new WaitForSeconds(8.5f);
    [field: SerializeField]
    private bool deckIsRecharging = false;
    public delegate void ChangeDeckIsActive(bool value);
    public event ChangeDeckIsActive onDeckIsActiveChanged;

	public void Initialize(PlayerCharacter pl)
    {
        player = pl;
        // handOfCards = transform.Find("UI-Hand").gameObject;
        handOfCards = Instantiate(handOfCards, transform);
        handOfCards.name = "Card-Hand-UI";
        handOfCards.SetActive(false);
        isShowingHand = false;
        if (_Hand.Count == 0)
            DrawFullHand();
    }


    public void ShowHand()
    {
        if(!isShowingHand)
        {
            player._CameraController.UnlockCursorKBM();
            handOfCards.SetActive(true);
            List<RectTransform> cardGameObjs = new List<RectTransform>();

            for (int cardIndex = 0; cardIndex < _Hand.Count; cardIndex++)
            {
                GameObject cardGameObj = CreateCardUIObject(_Hand[cardIndex], cardIndex);
                cardGameObjs.Add(cardGameObj.GetComponent<RectTransform>());
                // print("Card Index: " + cardIndex + " || Card Name: " + cardGameObj.name);
            }

            RearrangeCards(cardGameObjs);
            isShowingHand = true;
        }
    }

    public void PutAwayHand()
    {
        if (isShowingHand)
        {
            player._CameraController.LockCursorKBM();
            Button[] childCards = handOfCards.GetComponentsInChildren<Button>();

            for (int card = 0; card < childCards.Length; card++)
            {
                Destroy(childCards[card].gameObject);
            }

            handOfCards.SetActive(false);
            isShowingHand = false;
        }
    }

    private GameObject CreateCardUIObject(CardScriptableObj cardSO, int index)
    {
        GameObject newCard = Instantiate(cardPrefab, handOfCards.transform);
        newCard.name = cardSO.name;
        newCard.GetComponent<CardUI>().Initialize(cardSO, player, index);
        // newCard.transform.Find("Illo").GetComponent<RawImage>().texture = cardSO._CardImage.texture;
        // newCard.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = cardSO._CardName;
        // newCard.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = cardSO._CardCost.ToString();

        return newCard;
    }
    private void RearrangeCards(List<RectTransform> cardGameObjs)
    {
        // float radiansOfSeparation = Mathf.PI / cardGameObjs.Count;
        float radiansOfSeparation = (Mathf.PI * 2) / cardGameObjs.Count;
        
        for(int currentIndex = 0; currentIndex < cardGameObjs.Count; currentIndex++)
        {
            float transformX = Mathf.Sin(radiansOfSeparation * currentIndex) * radius;
            float transformY = Mathf.Cos(radiansOfSeparation * currentIndex) * radius;

            cardGameObjs[currentIndex].anchoredPosition = new Vector2(transformX, transformY);
            // cardGameObjs[currentIndex].anchoredPosition3D = new Vector3(transformX, transformY, 0.00f);
        }
    }

    public void PlayCard(int cardIndex)
    {
        _Hand[cardIndex].PlayCard(player);
        _Abyss.Add(_Hand[cardIndex]);
        _Hand.RemoveAt(cardIndex);
        player._Controls.CardSelected();
        if (!drawTimerActivated)
        {
            drawTimerActivated = true;
            if (_Deck.Count > 0 && _Hand.Count > 0)
            { 
                StartCoroutine(StartDrawCardTmer());
            }
        }
        CheckDeckCount();
    }

    private void DrawCard()
    {
        if(_Deck.Count > 0 && _Hand.Count < 4)
        {
            int drawCardIndex = Random.Range(0, _Deck.Count);
            if (CardWasAdded(drawCardIndex))
            {
                _Deck.RemoveAt(drawCardIndex);
                if(onDeckCountChanged != null)
                    onDeckCountChanged(_Deck.Count);
            }
        }
        else
		{
			print($"deck count is {_Deck.Count} and hand count is {_Hand.Count}");
			Debug.LogError("can't draw another card. either not enogh cards in deck or too many in hand.");
			CheckDeckCount();
		}
	}

    private void CheckDeckCount()
    {
        Debug.LogWarning("Checking deck");
        if (_Deck.Count <= 0)
        {
            print(">>>>>>>>>>>>> Should not be drawing more cards");
            drawTimerActivated = false;
            drawCardTimer = 0.0f;
            if (onDeckIsActiveChanged != null)
                onDeckIsActiveChanged(false);
            StopCoroutine(StartDrawCardTmer());
            StartCoroutine(StartDeckRechargeTimer());
        }
        else
            print($"Deck checked out ok with {_Deck.Count} cards left");
	}

	private void DrawFullHand()
    {
        for (int i = 0; i < 4; i++)
        {
            DrawCard();
            if (_Hand.Count >= 4) break;
        }
    }

	private bool CardWasAdded(int drawCardIndex)
	{
        //print($">>>> looking for index[{drawCardIndex}] from deck with {_Deck.Count - 1} possible indexes (count-1) <<<<<<<<<<");
        if (_Hand.Count < 4)
        {
		    _Hand.Add(_Deck[drawCardIndex]);
            return true;
        }
        Debug.LogError(">>>>>>>>>> cant add another CARD !!!!!!!!!!!!!!!!!!");
        return false;
	}

    private IEnumerator RechargeDeck()
    {
        while (deckIsRecharging)
        {
            int rechargeIndex = Random.Range(0, _Abyss.Count - 1);
            _Deck.Add(_Abyss[rechargeIndex]);
            _Abyss.RemoveAt(rechargeIndex);
            if (_Abyss.Count <= 0)
            {
                deckIsRecharging = false;
				onDeckIsActiveChanged(!deckIsRecharging);
			}
            else
                yield return null;
        }
        //DrawFullHand();
        if(_Hand.Count < 4)
        {
            if (_Hand.Count == 0)
                DrawCard();
            print(">>>>>>>>>> Need more cards");
            StartCoroutine(StartDrawCardTmer());
        }
		print(">>>>>>>>>>>> finish recharge");
	}

	private IEnumerator StartDrawCardTmer()
    {
        while (drawTimerActivated)
        {
            drawCardTimer += Time.deltaTime;
            if(drawCardTimer >= timeTillDrawNewCard)
            {
				DrawCard();
                drawCardTimer = 0;
                if(_Hand.Count == 4)
                {
                    drawTimerActivated = false;
                }
            }
            yield return null;
        }
    }

    private IEnumerator StartDeckRechargeTimer()
    {
        print("strart <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< recharge");
		print(">>>>>>>>>>>> start recharge");
		deckIsRecharging = true;
		yield return deckRechargeTime;
        StartCoroutine(RechargeDeck());
    }

    // end
}
