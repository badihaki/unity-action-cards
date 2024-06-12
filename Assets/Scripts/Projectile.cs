using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Character _controllingCharacter;
    [SerializeField] private int _damage;
    [SerializeField] private Vector2 damageForces;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifetime = 5.135f;

    private Rigidbody _rigidbody;

    [SerializeField] private bool _ready = false;

    public void InitializeProjectile(Character _controller, int _dmg, float _speed, float _life)
    {
        _rigidbody = GetComponent<Rigidbody>();

        _controllingCharacter = _controller;
        _damage = _dmg;
        this._speed = _speed;
        _lifetime = _life;
        
        StartCoroutine("StartLifetimeTimer");
        _ready = true;
    }

    private void FixedUpdate()
    {
        if (_ready)
        {
            _rigidbody.velocity = transform.forward * _speed;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(_ready)
        {
            Character hitCharacter = collider.GetComponentInParent<Character>();
            if(hitCharacter != _controllingCharacter)
            {
                if (collider.name == "Hurtbox")
                {
                    IDamageable damageableEntity = collider.GetComponentInParent<IDamageable>();
                    damageableEntity?.Damage(_damage, _controllingCharacter.transform, damageForces.x, damageForces.y);
            
                    print("projectile " + name + " collided with " + hitCharacter.name);
                    OnImpact();
                }
                else OnImpact();
            }
        }
    }

    IEnumerator StartLifetimeTimer()
    {
        yield return new WaitForSeconds(_lifetime);

        Destroy(gameObject);
    }

    private void OnImpact()
    {
        Destroy(gameObject);
    }

    // end
}
