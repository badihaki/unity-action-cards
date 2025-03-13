using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHitbox : MonoBehaviour
{
    [field: SerializeField] public Character _Character { get; private set; }
    public delegate void HitSomething(Transform hitObj);
    public event HitSomething OnHit;
    
    public void Initialize(Character character)
    {
        _Character = character;
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if(_Character != null)
        {
			trigger.TryGetComponent(out IDamageable damageableEntity);
			if (damageableEntity != null && _Character.transform != damageableEntity.GetControllingEntity())
            {
                // checking group
                if (_Character.isGroupedUp)
                {
                    if (_Character.GetGroup()._GroupLeader.transform == damageableEntity.GetControllingEntity()) // trying to damage group leader
                    {
						//print($"{_Character.name} hit their group leader!!");
					    return;
					}
                    foreach (var member in _Character.GetGroup()._GroupLeader._GroupMembers)
                    {
                        if (member.Character.transform == damageableEntity.GetControllingEntity()) // trying to damage a friendly
                        {
                            //print($"{_Character.name} hit a friend!! Friend is {damageableEntity.GetControllingEntity().name}");
                            return;
                        }
                    }
                }
                // stop checking group

                Damage dmgObj = new Damage(_Character._AttackController._Damage, _Character._AttackController._Force, _Character._AttackController._IntendedResponseToDamageBeingDealt, _Character._Actor.transform, _Character);
				damageableEntity?.Damage(dmgObj);
                if(OnHit != null)
                    OnHit(damageableEntity.GetControllingEntity());
                Vector3 hitFxPosition = new Vector3(damageableEntity.GetDamagedEntity().position.x, damageableEntity.GetDamagedEntity().position.y + 0.5f, damageableEntity.GetDamagedEntity().position.z);
                _Character._AttackController.PlayHitSpark(hitFxPosition);
			}
        }
    }

    //
}
