using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHurtbox : MonoBehaviour, IDamageable
{
    [SerializeField] private Character character;

    public void InitializeHurtBox(Character _character)
    {
        character = _character;
    }

    public void Damage(int damage, Transform damageSource, float knockForce, float launchForce)
    {
        character._Health.TakeDamage(damage);

        // send damageSource to character movement controller
    }


    // end
}
