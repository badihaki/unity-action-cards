using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Character _controllingCharacter;
    [SerializeField] private int _damage;
    [SerializeField] private float _force;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifetime = 5.135f;
    [SerializeField] private GameObject _impactVFX;
    [SerializeField] private responsesToDamage intendedResponse;


	private Rigidbody _rigidbody;

    [SerializeField] private bool _ready = false;

    public void InitializeProjectile(Character _controller, int _dmg, float _speed, float _life, GameObject impactPrefab)
    {
        _rigidbody = GetComponent<Rigidbody>();

        _controllingCharacter = _controller;
        _damage = _dmg;
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
            if(collider.gameObject.layer == 9)
            {
                if (_impactVFX) OnImpact(true);
                else OnImpact(false);
            }
            Character hitCharacter = collider.GetComponentInParent<Character>();
            if(hitCharacter != _controllingCharacter)
            {
                 //print(hitCharacter.name);
                if (collider.name == "Hurtbox")
                {
                    IDamageable damageableEntity = collider.GetComponentInParent<IDamageable>();
                    Damage damageOb = new Damage(_damage, _force, intendedResponse, transform, _controllingCharacter);

					damageableEntity?.Damage(damageOb);
            
                     print("projectile " + name + " collided with " + hitCharacter.name);
                }
                if (_impactVFX) OnImpact(true);
                else OnImpact(false);
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
