using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    CardScriptableObj card;
    Animator cardAnimator;
    PlayerCharacter player;
    int indexNumberReference;
    public void Initialize(CardScriptableObj cardSO, PlayerCharacter thePlayer, int indexNumber)
    {
        card = cardSO;
        player = thePlayer;
        indexNumberReference = indexNumber;

        transform.Find("Illo").GetComponent<RawImage>().texture = card._CardImage.texture;
        transform.Find("Name").GetComponent<TextMeshProUGUI>().text = card._CardName;
        transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = card._CardCost.ToString();

        cardAnimator = GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(player._Aether._CurrentAether >= card._CardCost)
        {
            player._PlayerCards.PlayCard(indexNumberReference);
            Destroy(gameObject);
        }
        else
        {
            print($"cant play {card._CardName} because cost ({card._CardCost.ToString()}) is greater than current aether ({player._Aether._CurrentAether.ToString()})");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardAnimator.SetBool("selected", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardAnimator.SetBool("selected", false);
    }
}
