using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerCards : MonoBehaviour
{
    private PlayerCharacter player;

    [Tooltip("The Deck is where all the player's usable are kept.")]
    [field: SerializeField] public List<CardScriptableObj> _Deck { get; private set; }
    
    [Tooltip("When a card is used, it goes to the Abyss")]
    [field: SerializeField] public List<CardScriptableObj> _Abyss { get; private set; }
    
    [Tooltip("The player's hand is where the active cards are stored")]
    [field: SerializeField] public List<CardScriptableObj> _Hand { get; private set; }
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject handOfCards;
    [SerializeField] private float radius = 300.00f;
    public bool showingHand = false;
    
    public void Initialize(PlayerCharacter pl)
    {
        player = pl;
        handOfCards = transform.Find("UI-Hand").gameObject;
        handOfCards.SetActive(false);
        showingHand = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player._Controls._Cards && !showingHand)
        {
            ShowHand();
            showingHand = true;
        }
        else if (!player._Controls._Cards)
        {
            if(showingHand) PutAwayHand();
            showingHand = false;
        }
    }

    public void ShowHand()
    {
        handOfCards.SetActive(true);
        List<RectTransform> cardGameObjs = new List<RectTransform>();
        print("Amount of cards in hand: " + _Hand.Count);
        for (int cardIndex = 0; cardIndex < _Hand.Count; cardIndex++)
        {
            GameObject cardGameObj = CreateCardUIObject(_Hand[cardIndex]);
            cardGameObjs.Add(cardGameObj.GetComponent<RectTransform>());
            // print("Card Index: " + cardIndex + " || Card Name: " + cardGameObj.name);
        }
        print("Amount of card game objs before rearranging: " + cardGameObjs.Count);
        RearrangeCards(cardGameObjs);
    }

    public void PutAwayHand()
    {
        Button[] childCards = handOfCards.GetComponentsInChildren<Button>();

        for (int card = 0; card < childCards.Length; card++)
        {
            Destroy(childCards[card].gameObject);
        }

        handOfCards.SetActive(false);
    }

    public void TestUseCard()
    {
        _Hand[0].PlayCard(player);
    }

    private GameObject CreateCardUIObject(CardScriptableObj cardSO)
    {
        GameObject newCard = Instantiate(cardPrefab, handOfCards.transform);
        newCard.name = cardSO.name;
        newCard.transform.Find("Illo").GetComponent<RawImage>().texture = cardSO._CardImage.texture;
        newCard.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = cardSO._CardName;
        newCard.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = cardSO._CardCost.ToString();

        return newCard;
    }
    private void RearrangeCards(List<RectTransform> cardGameObjs)
    {
        // float radiansOfSeparation = Mathf.PI / cardGameObjs.Count;
        float radiansOfSeparation = (Mathf.PI * 2) / cardGameObjs.Count;
        
        for(int currentIndex = 0; currentIndex < cardGameObjs.Count; currentIndex++)
        {
            float transformX = Mathf.Sin(radiansOfSeparation * currentIndex * radius);
            float transformY = Mathf.Cos(radiansOfSeparation * currentIndex * radius);

            print("Current Index: " + currentIndex + " Card: " + cardGameObjs[currentIndex]);
            print("X: " + transformX + " Y: " + transformY);
            cardGameObjs[currentIndex].anchoredPosition = new Vector2(transformX, transformY);
        }
    }

}