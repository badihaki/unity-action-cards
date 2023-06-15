using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerCards : MonoBehaviour
{
    private PlayerCharacter player;

    [Tooltip("The Deck is where all the player's usable are kept.")]
    [field: SerializeField] public CardScriptableObj[] _Deck { get; private set; }
    
    [Tooltip("When a card is used, it goes to the Abyss")]
    [field: SerializeField] public CardScriptableObj[] _Abyss { get; private set; }
    
    [Tooltip("The player's hand is where the active cards are stored")]
    [field: SerializeField] public CardScriptableObj[] _Hand { get; private set; }
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject handOfCards;
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

        for(int card = 0; card < _Hand.Length; card++)
        {
            // childCards.Add(CreateCardUIObject(_Hand[card]).transform);
            CreateCardUIObject(_Hand[card]);
        }
    }
    [SerializeField] Button[] childCards;
    public void PutAwayHand()
    {
        childCards = handOfCards.GetComponentsInChildren<Button>();
        Debug.Log(childCards);

        for (int card = 0; card < childCards.Length; card++)
        {
            print(childCards[card].gameObject.name);
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
        newCard.transform.Find("Illo").GetComponent<RawImage>().texture = cardSO._CardImage.texture;
        newCard.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = cardSO._CardName;
        newCard.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = cardSO._CardCost.ToString();

        return newCard;
    }
}
