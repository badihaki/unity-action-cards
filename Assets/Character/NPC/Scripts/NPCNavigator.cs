using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCNavigator : MonoBehaviour
{
    private NonPlayerCharacter _NPC;
    private NavMeshAgent _Agent;
    [field: SerializeField] public Transform _Target { get; private set; }
    [field: SerializeField] public Vector3 _TargetLocation { get; private set; }
    [field: SerializeField] public float _PatrolRange { get; private set; }

    public void InitializeNavigator(NonPlayerCharacter npc)
    {
        _NPC = npc;
        _Agent = GetComponent<NavMeshAgent>();
    }

    public bool TryFindNewPatrol()
    {
        float xPos = Random.Range(-_PatrolRange, _PatrolRange);
        float zPos = Random.Range(-_PatrolRange, _PatrolRange);
        Vector3 newPos = new Vector3(transform.position.x + xPos, 0, transform.position.z + zPos);
        print("new position to find for " + name + "'s patrol: " + newPos);
        if (Physics.CheckSphere(newPos, 1.0f))
        {
            _TargetLocation = newPos;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void MoveToPatrolLocation()
    {
        _Agent.SetDestination(_TargetLocation);
    }
}
