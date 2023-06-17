using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Create Card/New Spell")]
public class SpellCardScriptableObj : CardScriptableObj
{
    [Header("Spell Specific Below")]
    public GameObject _SpellProjectile;
    public int _SpellCharges;

    public override void PlayCard(Character controllingCharacter)
    {
        base.PlayCard(controllingCharacter);

        Debug.Log("Playing spell card: " + _CardName);
    }
}
