using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHitbox : MonoBehaviour
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
            IDamageable damageableEntity = trigger.GetComponent<IDamageable>();
            if (damageableEntity != null && _Character.transform != damageableEntity.GetControllingEntity())
            {
                //print($"hitbox is hitting {trigger.transform.parent.name}");
                print($"{_Character.name} hit {damageableEntity.GetControllingEntity().name}");
                damageableEntity?.Damage(_Character._AttackController._Damage, _Character.transform, _Character._AttackController._KnockbackForce, _Character._AttackController._LaunchForce);
                Vector3 position = new Vector3(trigger.transform.parent.position.x, trigger.transform.parent.position.y + 0.5f, trigger.transform.parent.position.z);
                GameObject hitFX = Instantiate(_Character._WeaponController._CurrentWeapon._WeaponHitSpark, position, trigger.transform.parent.rotation);
            }
        }
    }

    //
}
