using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMovementController : MonoBehaviour
{
    private Actor _actor;
    private Rigidbody _rigidbody;

    public void Initialize(Actor newActor)
    {
        _actor = newActor;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
