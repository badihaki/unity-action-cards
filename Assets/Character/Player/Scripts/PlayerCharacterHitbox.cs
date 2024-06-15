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
            IDamageable damageableEntity = trigger.GetComponent<IDamageable>();
            damageableEntity?.Damage(_Character._AttackController._Damage, _Character.transform, _Character._AttackController._KnockbackForce, _Character._AttackController._LaunchForce);
            Vector3 position = new Vector3(trigger.transform.parent.position.x, trigger.transform.parent.position.y + 0.5f, trigger.transform.parent.position.z);
            GameObject hitFX = Instantiate(_Character._AttackController._CurrentWeapon._WeaponHitSpark, position, trigger.transform.parent.rotation);
        }
    }

    //
}
