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
    [field: SerializeField] private GameObject _TargetDebugObject;
    [field: SerializeField] public float _MaxDistance { get; private set; }

    public void InitializeNavigator(NonPlayerCharacter npc)
    {
        _NPC = npc;
        _Agent = _NPC._NPCActor.GetComponent<NavMeshAgent>();
        _Agent.speed = _NPC._CharacterSheet._WalkSpeed;
    }

    public bool TryFindNewPatrol()
    {
        float xPos = Random.Range(-_PatrolRange, _PatrolRange);
        float zPos = Random.Range(-_PatrolRange, _PatrolRange);
        Vector3 newPos = new Vector3(_NPC._NPCActor.transform.position.x + xPos, 0, _NPC._NPCActor.transform.position.z + zPos);
        // print("new position to find for " + name + "'s patrol: " + newPos);
        if (Physics.CheckSphere(newPos, 1.0f))
        {
            _TargetLocation = newPos;
            CreateDebugObject(newPos);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StartMoveToDestination()
    {
        if (_TargetDebugObject) DestroyDebugObject();
        if (_Target)
        {
            _Agent.SetDestination(_Target.position);
            // print($"moving to target at {_Agent.destination}");
            CreateDebugObject(_Target.position);
        }
        else
        {
            _Agent.SetDestination(_TargetLocation);
            CreateDebugObject(_TargetLocation);
        }
        
    }

    public void StopNavigation()
    {
        if (!_Agent.isStopped)
        {
            // print("~~~Stop nav");
            _Agent.isStopped = true;
            _Agent.ResetPath();
            _NPC._MoveController.ZeroOutMovement();
        }
    }

    public bool IsNavStopped() => _Agent.isStopped;

    private void CreateDebugObject(Vector3 position)
    {
        _TargetDebugObject = new GameObject();
        _TargetDebugObject.transform.position = position;
        _TargetDebugObject.name = name + "TargetLoc";
    }
    public void DestroyDebugObject()
    {
        Destroy(_TargetDebugObject);
        _TargetDebugObject = null;
    }

    public void SetTarget(Transform newTarget) => _Target = newTarget;
    public void SetTargetDesiredDistance(float distance, float m_distance = 2.0f)
    {
        _Agent.stoppingDistance = distance;
        SetMaxAttackDistance(distance + 1.0f);
    }
    public void SetMaxAttackDistance(float m_distance = 2.0f) => _MaxDistance = m_distance;
}
