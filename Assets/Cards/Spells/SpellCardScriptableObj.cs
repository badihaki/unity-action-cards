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
    public float _SpellPushbackForce;
    public float _SpellLaunchForce;
	public int _SpellCharges;
    public float _SpellProjectileSpeed;
    public int _SpellAetherCost;
    public responsesToDamage _ResponseToDamage;

    [Header("Spell Element Modifiers")]
    public int _FireDmg;
    public int _WaterDmg;
    public int _PlasmaDmg;
    public float _GravityModifier;
    public Vector2 _GravityInfluence;

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
            player._UIController.AddSpellToUI(this);
        }
    }
}
