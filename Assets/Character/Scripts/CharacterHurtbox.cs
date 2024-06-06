using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHurtbox : MonoBehaviour, IDamageable
{
    [SerializeField] private Character character;
    private IKnockbackable knockInterface;

    public void InitializeHurtBox(Character _character)
    {
        character = _character;
        knockInterface = GetComponentInParent<IKnockbackable>();
    }

    public void Damage(int damage, Transform damageSource, float knockForce, float launchForce)
    {

        if(damageSource != character.transform)
        {
            // print("damaging");
            character._Health.TakeDamage(damage);
            character.transform.LookAt(damageSource);

            knockInterface?.ApplyKnockback(damageSource, knockForce, launchForce);
        }

        // send damageSource to character movement controller
    }


    // end
}
