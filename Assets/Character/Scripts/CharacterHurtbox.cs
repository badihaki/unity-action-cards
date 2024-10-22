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
        print($"{character.name} has knock interface {knockInterface}");
    }

    public void Damage(int damage, Transform damageSource, float knockForce, float launchForce)
    {

        if(damageSource != character.transform)
        {
            character._Health.TakeDamage(damage);
            character._Actor.transform.LookAt(damageSource);

            Quaternion rotation = character._Actor.transform.rotation;
            rotation.x = 0;
            rotation.z = 0;

            character._Actor.transform.rotation = rotation;

            knockInterface?.ApplyKnockback(damageSource, knockForce, launchForce);
            DetermineWhoWhurtMe(damageSource);
        }
    }

    public Transform GetControllingEntity()
    {
        return character.transform;
    }


    // end
}
