using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHitbox : MonoBehaviour
{
    [field: SerializeField] public Character _Character { get; private set; }
    public delegate void HitSomething(Transform hitObj);
    public event HitSomething OnHit;
    [field: SerializeField, Header("Elements")]
    private bool hasFireProperties;
    [field: SerializeField]
    private float fireDmg;
	[field: SerializeField]
	private bool hasIceProperties;
	[field: SerializeField]
	private float iceDmg;
	[field: SerializeField]
    private bool hasPlasmaProperties;
	[field: SerializeField]
	private float plasmaDmg;

	public void Initialize(Character character)
    {
        _Character = character;
    }

    public enum HitBoxElementalProps
    {
        none,
        fire,
        ice,
        plasma
    }
    public void SetElementalProperties(HitBoxElementalProps elementalProp, bool value, float damageValue = 0)
    {
        switch(elementalProp)
        {

            case HitBoxElementalProps.fire:
                hasFireProperties = value;
                fireDmg = damageValue;
				break;
			case HitBoxElementalProps.ice:
                hasIceProperties = value;
                iceDmg = damageValue;
				break;
            case HitBoxElementalProps.plasma:
                hasPlasmaProperties = value;
                plasmaDmg = damageValue;
				break;
            case HitBoxElementalProps.none:
                hasFireProperties = false;
                fireDmg = 0;
				hasIceProperties = false;
                iceDmg = 0;
                hasPlasmaProperties = false;
                plasmaDmg = 0;
                break;
			default:
				hasFireProperties = false;
				hasIceProperties = false;
				hasPlasmaProperties = false;
				break;
		}
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if(_Character != null) // make sure we have an assigned character to this hitbox
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

            // fire dmg
            if (hasFireProperties)
            {
                trigger.TryGetComponent(out IFlammable flammableEntity);

                if(flammableEntity != null) // we have hit something flammable
                {
                    if(flammableEntity.GetControllingEntity() != _Character) // we arent hitting ourself
                    {
                        // check if hitting a group member
						if (_Character.isGroupedUp) // if we do, we dont hurt it
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

                        flammableEntity.TakeFireDamage(fireDmg);
					}
                }
            }
        }
    }

    //
}
