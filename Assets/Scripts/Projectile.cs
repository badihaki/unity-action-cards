using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Character _controllingCharacter;
    //[SerializeField] private int _damage;
    //[SerializeField] private float _force;
    [field: SerializeField]
    private Damage _dmgObj;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifetime = 5.135f;
    [SerializeField] private GameObject _impactVFX;
    [SerializeField] private responsesToDamage intendedResponse;


	private Rigidbody _rigidbody;

    [SerializeField] private bool _ready = false;

    public void InitializeProjectile(Character _controller, Damage dmgObj, float _speed, float _life, GameObject impactPrefab)
    {
        _rigidbody = GetComponent<Rigidbody>();

        _controllingCharacter = _controller;
        //_damage = _dmg;
        _dmgObj = dmgObj;
        this._speed = _speed;
        _lifetime = _life;
        _impactVFX = impactPrefab;
        
        StartCoroutine("StartLifetimeTimer");
        _ready = true;
    }

    private void FixedUpdate()
    {
        if (_ready)
        {
            _rigidbody.linearVelocity = transform.forward * _speed;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //print(collider.name);
        if(_ready)
        {
            if (collider.gameObject.layer == 7) // looking for environment layer
            {
                if (_impactVFX) OnImpact(true);
                else OnImpact(false);
                return;
            }

			collider.TryGetComponent(out Projectile hitProjectile); // looking for another projectile
			if (hitProjectile != null)
			{
				print("hit projectile with a projectile!!");
				if (hitProjectile._controllingCharacter != _controllingCharacter)
				{
					if (_impactVFX) OnImpact(true);
					else
						OnImpact(false);
				}
				return;
			}
			/*
            Character hitCharacter = collider.GetComponentInParent<Character>();
            if (hitCharacter != null && hitCharacter != _controllingCharacter)
            {
                // check for group activity with the hit character
                if (_controllingCharacter.isGroupedUp)
                {
                    if (hitCharacter == _controllingCharacter.GetGroup()._GroupLeader)
                        return;
                    foreach (CharacterGroupLeader.GroupMemberStruct grMem in _controllingCharacter.GetGroup()._GroupLeader._GroupMembers)
                    {
                        if (hitCharacter == grMem.Character)
                            return;
                    }
                }
                // end group check

                if (collider.name == "Hurtbox")
                {
                    IDamageable damageableEntity = collider.GetComponentInParent<IDamageable>();
                    //Damage damageOb = new Damage(_damage, _force, intendedResponse, transform, _controllingCharacter);

					damageableEntity?.Damage(_dmgObj);
            
                     print("projectile " + name + " collided with " + hitCharacter.name);
                }
                if (_impactVFX) OnImpact(true);
                else
                    OnImpact(false);
                return;
            }
             */

			// get damageable obj
			collider.TryGetComponent(out IDamageable damageableObj);
            if (damageableObj != null && damageableObj.GetControllingEntity() != _controllingCharacter.transform)
            {
                // get a character obj
                damageableObj.GetControllingEntity().TryGetComponent(out Character hitCharacter);
				if (hitCharacter != null)
                {
					// we hit a character
					// check for group activity with the hit character
					if (_controllingCharacter.isGroupedUp)
					{
						if (hitCharacter == _controllingCharacter.GetGroup()._GroupLeader)
							return;
						foreach (CharacterGroupLeader.GroupMemberStruct grMem in _controllingCharacter.GetGroup()._GroupLeader._GroupMembers)
						{
							if (hitCharacter == grMem.Character)
								return;
						}
					}
                    // end group check

                    damageableObj.Damage(_dmgObj);
					if (_impactVFX) OnImpact(true);
					else OnImpact(false);
					return;
				}
			}

			// elemental stuff
            if(_dmgObj.dmgOptions.fireDmg > 0)
            {
			    collider.TryGetComponent(out IFlammable flammableObj);
                if (flammableObj != null && flammableObj.GetControllingEntity() != _controllingCharacter.transform)
                {
			        flammableObj.GetControllingEntity().TryGetComponent(out Character hitFlammableCharacter);
				    // check for group activity with the hit character
				    if (_controllingCharacter.isGroupedUp)
				    {
					    if (hitFlammableCharacter == _controllingCharacter.GetGroup()._GroupLeader)
						    return;
					    foreach (CharacterGroupLeader.GroupMemberStruct grMem in _controllingCharacter.GetGroup()._GroupLeader._GroupMembers)
					    {
						    if (hitFlammableCharacter == grMem.Character)
							    return;
					    }
				    }
				    // end group check
				    if (_impactVFX) OnImpact(true);
				    else OnImpact(false);
				    flammableObj.TakeFireDamage(_dmgObj.dmgOptions.fireDmg);
			    }
            }
		}
    }

    IEnumerator StartLifetimeTimer()
    {
        yield return new WaitForSeconds(_lifetime);

        //Destroy(gameObject);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    protected virtual void OnImpact(bool withVfx)
    {
        if (withVfx)
        {
            //Instantiate(_impactVFX, transform.position, Quaternion.identity);
            GameObject effect = ObjectPoolManager.GetObjectFromPool(_impactVFX, transform.position, Quaternion.identity, ObjectPoolManager.PoolFolder.Particle);
		}
		//Destroy(gameObject);
		ObjectPoolManager.ReturnObjectToPool(gameObject);
	}

    // end
}
