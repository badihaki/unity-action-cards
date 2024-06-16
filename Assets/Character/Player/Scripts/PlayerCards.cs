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
    private float radius = 300.00f;
    private bool isShowingHand;
    
    public void Initialize(PlayerCharacter pl)
    {
        player = pl;
        // handOfCards = transform.Find("UI-Hand").gameObject;
        handOfCards = Instantiate(handOfCards, transform);
        handOfCards.name = "Card-Hand-UI";
        handOfCards.SetActive(false);
        isShowingHand = false;
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
            Button[] childCards = handOfCards.GetComponentsInChildren<Button>();

            for (int card = 0; card < childCards.Length; card++)
            {
                Destroy(childCards[card].gameObject);
            }

            handOfCards.SetActive(false);
            isShowingHand = false;
        }
    }

    public void TestUseCard()
    {
        _Hand[0].PlayCard(player);
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
    }

}
