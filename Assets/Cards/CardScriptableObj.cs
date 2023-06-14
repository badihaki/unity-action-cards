using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScriptableObj : ScriptableObject
{
    [Header("Card Base Info")]
    [Tooltip("The name of the card")]
    public string _CardName;
    public enum _CardType
    {
        Minion,
        Weapon,
        Spell
    }
    public _CardType _TypeOfCard;
    [Tooltip("The type of card (can be minion, weapon or spell)")]
    public int _CardCost;
    public Sprite _CardImage;

    public virtual void PlayCard(Character controllingCharacter)
    {
        //
    }
}
