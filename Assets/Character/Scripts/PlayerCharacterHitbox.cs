using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterHitbox : MonoBehaviour
{
    [field: SerializeField] public PlayerCharacter _Character { get; private set; }
    
    public void Initialize(PlayerCharacter character)
    {
        _Character = character;
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if(_Character != null)
        {
            Character hitCharacter = trigger.GetComponentInParent<Character>();
            // gonna have to add props as a secondary thing
            if (hitCharacter && hitCharacter != _Character)
                if (hitCharacter != _Character)
                {
                    IDamageable damageableEntity = trigger.GetComponentInParent<IDamageable>();
                    damageableEntity?.Damage(_Character._AttackController._Damage, this.transform, _Character._AttackController._KnockbackForce, _Character._AttackController._LaunchForce);

                    print(_Character.name+"'s hitbox collided with " + hitCharacter.name);
                    gameObject.SetActive(false);
                }
            // else if prop != null then run IDamageable.Damage()
        }
    }

    //
}
