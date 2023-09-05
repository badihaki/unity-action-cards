using System;
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

    public void PlayCard(Character controllingCharacter)
    {
        if (AetherCheck(controllingCharacter._AetherPoints._CurrentAetherPoints, controllingCharacter._AetherPoints))
        {
            UseCardAbility(controllingCharacter);

            Debug.Log("Playing card: " + _CardName);
        }
    }

    protected virtual void UseCardAbility(Character controllingCharacter)
    {
        throw new NotImplementedException();
    }

    protected bool AetherCheck(float aetherPoints, Aether _AetherPool)
    {
        if (aetherPoints >= _CardCost)
        {
            _AetherPool.UseAetherPoints(_CardCost);
            return true;
        }
        else return false;
    }

    // end
}
