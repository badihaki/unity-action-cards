using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Create Card/New Spell")]
public class SpellCardScriptableObj : CardScriptableObj
{
    [Header("Spell Specific Below")]
    public GameObject _SpellProjectile;
    public int _SpellDamage;
    public int _SpellCharges;
    public float _SpellSpeed;

    public override void PlayCard(Character controllingCharacter)
    {
        base.PlayCard(controllingCharacter);

        if(controllingCharacter.GetComponent<PlayerCharacter>() != null)
        {
            PlayerCharacter player = controllingCharacter.GetComponent<PlayerCharacter>();
            player._PlayerSpells.AddSpellToList(this);
        }

        Debug.Log("Playing spell card: " + _CardName);
    }
}
