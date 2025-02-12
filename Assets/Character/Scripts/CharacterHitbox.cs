using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHitbox : MonoBehaviour
{
    [field: SerializeField] public Character _Character { get; private set; }
    
    public void Initialize(Character character)
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
                //print(damageableEntity.ToString());
                Damage dmgObj = new Damage(_Character._AttackController._Damage, _Character._AttackController._Force, _Character._AttackController._IntendedResponseToDamageBeingDealt, _Character._Actor.transform);
                
                damageableEntity?.Damage(dmgObj);
                Vector3 hitFxPosition = new Vector3(damageableEntity.GetDamagedEntity().position.x, damageableEntity.GetDamagedEntity().position.y + 0.5f, damageableEntity.GetDamagedEntity().position.z);
                _Character._AttackController.PlayHitSpark(hitFxPosition);
			}
        }
    }

    //
}
