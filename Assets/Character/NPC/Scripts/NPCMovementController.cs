using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementController : MonoBehaviour
{
    private NonPlayerCharacter _Character;
    [field: SerializeField] public CharacterController _CharacterController { get; private set; }

    [field: Header("Forces"), SerializeField]
    public Vector3 _ExternalForces { get; private set; }

    public void InitializeNPCMovement(NonPlayerCharacter character)
    {
        _Character = character;
        _CharacterController = _Character._Actor.GetComponent<CharacterController>();
        _ExternalForces = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ZeroOutMovement() => _CharacterController.Move(Vector3.zero);

    public void SetExternalForces(Transform forceSource, float knockforce, float launchForce)
    {
        if(forceSource != _Character.transform)
        {
            Vector3 direction = (transform.position - forceSource.position).normalized;
            // Vector3 force = new Vector3(0, launchForce, knockforce);
            //direction *= -1;
            Vector3 force = new Vector3(direction.x, direction.y * launchForce, direction.z * knockforce);
            //_CharacterController.Move(direction * Mathf.Sqrt(knockforce)); // was I using the wrong value all along??
            //_CharacterController.Move(force);
        
            print($"{name} is KNOCKBACKED in direction >> {direction} with force {force} resulting in {direction * Mathf.Sqrt(knockforce)} <<");
            _ExternalForces = force;
        }
        else _ExternalForces = Vector3.zero;
	}

    public void ApplyExternalForces()
    {
        _CharacterController.Move(_ExternalForces * Time.deltaTime);
    }

    public void EnableCharacterController() => _CharacterController.enabled = true;
    public void DisableCharacterController() => _CharacterController.enabled = false;
}
