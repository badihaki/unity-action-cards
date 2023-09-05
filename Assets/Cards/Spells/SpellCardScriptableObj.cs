using System;
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
    public float _SpellProjectileSpeed;
    public float _SpellAddonTime;

    public enum spellAnimationType
    {
        shoot,
        pull,
        meteor
    }

    public spellAnimationType _SpellAnimationBool;
    protected override void UseCardAbility(Character controllingCharacter)
    {
        if (controllingCharacter.GetComponent<PlayerCharacter>() != null)
        {
            PlayerCharacter player = controllingCharacter.GetComponent<PlayerCharacter>();
            player._PlayerSpells.AddSpellToList(this);
        }
    }
}
