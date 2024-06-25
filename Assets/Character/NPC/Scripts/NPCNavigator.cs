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
        Vector3 newPos = new Vector3(_PatrolRange, 0, _PatrolRange);
        Ray ray = new Ray(newPos, Vector3.forward);

        if(Physics.CheckSphere(newPos,1.0f))
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
