using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementController : MonoBehaviour
{
    private NonPlayerCharacter _Character;
    [SerializeField]private CharacterController _CharacterController;

    [field: Header("Forces"), SerializeField]
    public Vector3 externalForces { get; private set; }

    public void InitializeNPCMovement(NonPlayerCharacter character)
    {
        _Character = character;
        _CharacterController = _Character._Actor.GetComponent<CharacterController>();
        externalForces = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ZeroOutMovement() => _CharacterController.Move(Vector3.zero);

    public void SetKnockforce(Transform forceSource, float knockforce, float launchForce)
    {
        Vector3 direction = (transform.position - forceSource.position).normalized;
        // Vector3 force = new Vector3(0, launchForce, knockforce);
        direction *= -1;
        Vector3 force = new Vector3(direction.x, direction.y * launchForce, direction.z * knockforce);
        print($"{name} is KNOCKBACKED in direction >> {direction} with force {force} resulting in {direction * Mathf.Sqrt(knockforce)} <<");
        //_CharacterController.Move(direction * Mathf.Sqrt(knockforce)); // was I using the wrong value all along??
        //_CharacterController.Move(force);
        externalForces = force;
	}

    public void ApplyExternalForces()
    {
        _CharacterController.Move(externalForces * Time.deltaTime);
    }
}
