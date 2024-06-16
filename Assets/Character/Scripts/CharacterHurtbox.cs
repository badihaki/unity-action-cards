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
            character._Health.TakeDamage(damage);
            character.transform.LookAt(damageSource);

            Quaternion rotation = character.transform.rotation;
            rotation.x = 0;
            rotation.z = 0;

            character.transform.rotation = rotation;

            knockInterface?.ApplyKnockback(damageSource, knockForce, launchForce);
        }
    }

    public Transform GetControllingEntity()
    {
        return character.transform;
    }


    // end
}
