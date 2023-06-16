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
    public void Initialize(CardScriptableObj cardSO, PlayerCharacter thePlayer)
    {
        card = cardSO;
        player = thePlayer;

        transform.Find("Illo").GetComponent<RawImage>().texture = card._CardImage.texture;
        transform.Find("Name").GetComponent<TextMeshProUGUI>().text = card._CardName;
        transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = card._CardCost.ToString();

        cardAnimator = GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        card.PlayCard(player);
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
