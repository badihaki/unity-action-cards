using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Character controllingCharacter;
    [SerializeField] private int damage;
    [SerializeField] private float speed;

    private Rigidbody _rigidbody;

    private bool ready = false;

    public void InitializeProjectile(Character _controller, int _dmg, float _speed)
    {
        _rigidbody = GetComponent<Rigidbody>();

        controllingCharacter = _controller;
        damage = _dmg;
        speed = _speed;

        ready = true;
    }

    private void FixedUpdate()
    {
        if (ready)
        {
            _rigidbody.velocity = Vector3.forward * speed;
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        //
    }

    // end
}
