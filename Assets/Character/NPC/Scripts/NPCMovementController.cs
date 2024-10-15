using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementController : MonoBehaviour, IKnockbackable
{
    private NonPlayerCharacter _Character;
    [SerializeField]private CharacterController _PhysicsController;

    public void InitializeNPCMovement(NonPlayerCharacter character)
    {
        _Character = character;
        _PhysicsController = _Character._Actor.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ZeroOutMovement() => _PhysicsController.Move(Vector3.zero);

    public void ApplyKnockback(Transform forceSource, float knockforce, float launchForce)
    {
        Vector3 direction = (transform.position - forceSource.position).normalized;
        // Vector3 force = new Vector3(0, launchForce, knockforce);
        Vector3 force = new Vector3(direction.x, direction.y * launchForce, direction.z * knockforce);
        _PhysicsController.Move(direction * knockforce);
    }
}
