using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterHitbox : MonoBehaviour
{
    [field: SerializeField] public PlayerCharacter _Character { get; private set; }
    
    public void Initialize(PlayerCharacter character)
    {
        _Character = character;

        gameObject.SetActive(false);
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
                    damageableEntity?.Damage(_Character._PlayerAttack._Damage, this.transform, _Character._PlayerAttack._KnockbackForce, _Character._PlayerAttack._LaunchForce);

                    print("projectile " + name + " collided with " + hitCharacter.name);
                    Destroy(gameObject);
                }
            // else if prop != null then run IDamageable.Damage()
        }
    }

    //
}
