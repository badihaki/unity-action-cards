using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Character _controllingCharacter;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;

    [SerializeField] private bool _ready = false;

    public void InitializeProjectile(Character _controller, int _dmg, float _speed)
    {
        _rigidbody = GetComponent<Rigidbody>();

        _controllingCharacter = _controller;
        _damage = _dmg;
        this._speed = _speed;

        _ready = true;
    }

    private void FixedUpdate()
    {
        if (_ready)
        {
            _rigidbody.velocity = transform.forward * _speed;
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if(_ready)
        {
            Character hitCharacter = trigger.GetComponentInParent<Character>();
            if (hitCharacter && hitCharacter != _controllingCharacter)
            {
                IDamageable damageableEntity = trigger.GetComponentInParent<IDamageable>();
                damageableEntity?.Damage(_damage, this.transform);

                print("projectile " + name + " collided with " + hitCharacter.name);
                Destroy(gameObject);
            }
        }
    }

    // end
}
