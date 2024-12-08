using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Create Card/New Spell")]
public class SpellCardScriptableObj : CardScriptableObj
{
    [Header("Spell Specific Below")]
    public GameObject _SpellProjectile;
    public GameObject _SpellImpactVFX;

    [Header("Spell stats")]
    public int _SpellDamage;
    public int _SpellCharges;
    public float _SpellProjectileSpeed;
    public int _SpellAetherCost;

    [Header("Spell lifetime variables")]
    public float _SpellAddonTime;
    public float _SpellLifetime = 5.135f;

    [Header("Sound clip variables")]
    [Tooltip("The sound that plays when the spell is fired")]
    public AudioClip _StartSound;
    [Tooltip("The sound that plays while the projectile is in play")]
    public AudioClip _TravelSound;
    [Tooltip("The sound that plays when the spell is fired")]
    public AudioClip _ImpactSound;

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
