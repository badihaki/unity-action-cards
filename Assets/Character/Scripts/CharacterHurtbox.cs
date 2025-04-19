using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHurtbox : MonoBehaviour, IDamageable, IFlammable
{
    [field: SerializeField, SerializeReference] private Character character;
    struct DmgObjInfo
    {
        public int damageAmount;
        public float poiseDamageAmount;

        public responsesToDamage intendedResponse;
		public float damageForce;
		public Transform damageSource;
        public DmgObjInfo(int dmg, float poiseDmg, responsesToDamage dmgResp, float force, Transform dmgSource)
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
    DmgObjInfo lastDamageObjRecieved;

    public delegate void DetectWhoHurtMe(Character target);
    public event DetectWhoHurtMe OnHurtByCharacter;

    [SerializeField]
    private CharacterFlammableInterface characterFlammableInterface;

	#region regular stuff
	public void InitializeHurtBox(Character _character)
    {
        character = _character;
        //print($"{character.name} has knock interface {knockInterface}");
    }

	//private void OnTriggerEnter(Collider other)
	//{
 //       if (other.name == "Hurtbox")
 //       {
 //           Character collidedCharacter = other.GetComponentInParent<Character>();
 //           if (collidedCharacter != null)
 //           {
 //               character.PushBackCharacter(transform.position, 1f, false);
 //           }
 //       }
	//}

	private void StoreLastDamageSource(Damage dmgObj) => lastDamageObjRecieved = new DmgObjInfo(dmgObj.damageAmount, dmgObj.poiseDamageAmount, dmgObj.intendedResponse, dmgObj.damageForce, dmgObj.damageSource);
	#endregion

	#region damage interface
	public void Damage(Damage dmgObj)
	{
		//print("damagin from hurtbox");
        character._Actor.TakeDamage(dmgObj);
		StoreLastDamageSource(dmgObj);
        if (OnHurtByCharacter != null)
            OnHurtByCharacter(dmgObj.damageCreatorCharacter);
    }

	public Transform GetControllingEntity()
    {
        return character.transform;
    }

	public Transform GetDamagedEntity()
	{
        return character._Actor.transform;
	}
	#endregion

	#region flammable interface
	public void TakeFireDamage(float damage)
	{
		throw new NotImplementedException();
	}

	public void SetOnFire()
	{
		throw new NotImplementedException();
	}
	public BoxCollider referenceCollider => characterFlammableInterface.collider;

	public float currentFireDmg => characterFlammableInterface.flammableDamage;

	public bool isOnFire => characterFlammableInterface.onFire;
	#endregion

	// end
}


[Serializable]
class CharacterFlammableInterface
{
    private Character character;

    [field: SerializeField]
    public BoxCollider collider { get; private set; }

    [field: SerializeField]
    public float flammableDamage {  get; private set; }
    [field: SerializeField]
    public bool onFire { get; private set; }
}