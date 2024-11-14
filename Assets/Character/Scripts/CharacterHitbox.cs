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
                //damageableEntity?.Damage(_Character._AttackController._Damage, _Character.transform, _Character._AttackController._KnockbackForce, _Character._AttackController._LaunchForce);
				damageableEntity?.Damage(_Character._AttackController._Damage, _Character._Actor.transform, _Character._AttackController._KnockbackForce, _Character._AttackController._LaunchForce);
                Vector3 hitFxPosition = new Vector3(damageableEntity.GetDamagedEntity().position.x, damageableEntity.GetDamagedEntity().position.y + 0.5f, damageableEntity.GetDamagedEntity().position.z);
                GameObject hitFX = Instantiate(_Character._WeaponController._CurrentWeapon._WeaponHitSpark, hitFxPosition, trigger.transform.parent.rotation);
            }
        }
    }

    //
}
