using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHurtbox : MonoBehaviour, IDamageable
{
    [field: SerializeField, SerializeReference] private Character character;
    private IKnockbackable knockInterface;
    [Serializable]
    struct LastDamageObj
    {
        public int damageAmount;
        public float poiseDamageAmount;

        public responsesToDamage intendedResponse;
		public float damageForce;
		public Transform damageSource;
        public LastDamageObj(int dmg, float poiseDmg, responsesToDamage dmgResp, float force, Transform dmgSource)
        {
            damageAmount = dmg;
            poiseDamageAmount = poiseDmg;
            damageSource = dmgSource;
            intendedResponse = dmgResp;
            damageForce = force;
            damageSource = dmgSource;
        }
	}
    [field: SerializeField]
    LastDamageObj lastDamage;

    public delegate void DetectWhoHurtMe(Character target);
    public event DetectWhoHurtMe OnHurtByCharacter;

    public void InitializeHurtBox(Character _character)
    {
        character = _character;
        knockInterface = GetComponentInParent<IKnockbackable>();
        //print($"{character.name} has knock interface {knockInterface}");
    }

    public void Damage(Damage dmgObj)
	{
		//print("damagin from hurtbox");
        character._Actor.TakeDamage(dmgObj);
		StoreLastDamageSource(dmgObj);
        print(dmgObj != null);
        if (OnHurtByCharacter != null)
            OnHurtByCharacter(dmgObj.damageCreatorCharacter);
    }

    private void StoreLastDamageSource(Damage dmgObj) => lastDamage = new LastDamageObj(dmgObj.damageAmount, dmgObj.poiseDamageAmount, dmgObj.intendedResponse, dmgObj.damageForce, dmgObj.damageSource);

	public Transform GetControllingEntity()
    {
        return character.transform;
    }

	public Transform GetDamagedEntity()
	{
        return character._Actor.transform;
	}

	// end
}
