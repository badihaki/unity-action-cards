using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHurtbox : MonoBehaviour, IDamageable
{
    [SerializeField] private Character character;
    private IKnockbackable knockInterface;

    public delegate void DetectWhoHurtMe(Transform target);
    public event DetectWhoHurtMe DetermineWhoWhurtMe;

    public void InitializeHurtBox(Character _character)
    {
        character = _character;
        knockInterface = GetComponentInParent<IKnockbackable>();
        //print($"{character.name} has knock interface {knockInterface}");
    }

    public void Damage(int damage, Transform damageSource, bool knockedBack, bool launched, Character damageSourceController = null)
	{
		print("damagin from hurtbox");
        //if(damageSource != character.transform)
        //{
        //    character._Health.TakeDamage(damage);
        //    character._Actor.transform.LookAt(damageSource);

        //    Quaternion rotation = character._Actor.transform.rotation;
        //    rotation.x = 0;
        //    rotation.z = 0;

        //    character._Actor.transform.rotation = rotation;

        //    character.CalculateHitResponse(knockForce, launchForce, damage);
        //    knockInterface?.ApplyKnockback(damageSource, knockForce, launchForce);
        //    DetermineWhoWhurtMe(damageSource);
        //}
        character._Actor.Damage(damage, damageSource, knockedBack, launched, damageSourceController);
    }

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
