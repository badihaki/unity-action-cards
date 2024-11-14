using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Character _controllingCharacter;
    [SerializeField] private int _damage;
    [SerializeField] private Vector2 _damageForces;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifetime = 5.135f;
    [SerializeField] private GameObject _impactVFX;

    private Rigidbody _rigidbody;

    [SerializeField] private bool _ready = false;

    public void InitializeProjectile(Character _controller, int _dmg, float _speed, float _life, Vector2 _knockAndLaunchForces, GameObject impactPrefab)
    {
        _rigidbody = GetComponent<Rigidbody>();

        _controllingCharacter = _controller;
        _damage = _dmg;
        this._speed = _speed;
        _lifetime = _life;
        _damageForces = _knockAndLaunchForces;
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
        print(collider.name);
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
                    damageableEntity?.Damage(_damage, _controllingCharacter._Actor.transform, _damageForces.x, _damageForces.y, _controllingCharacter);
            
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

        Destroy(gameObject);
    }

    protected virtual void OnImpact(bool withVfx)
    {
        if (withVfx) Instantiate(_impactVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // end
}
