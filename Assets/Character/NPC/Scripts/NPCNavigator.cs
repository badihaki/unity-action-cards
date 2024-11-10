using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCNavigator : MonoBehaviour
{
    private NonPlayerCharacter _NPC;
    [field: SerializeField, Header("Target info")] public Transform _Target { get; private set; }
    [field: SerializeField] public Vector3 _TargetLocation { get; private set; }
    [field: SerializeField] public float _PatrolRange { get; private set; }
    [field: SerializeField] public float _MaxDistance { get; private set; }

    public void InitializeNavigator(NonPlayerCharacter npc)
    {
        _NPC = npc;
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
            //CreateDebugObject(newPos);
            return true;
        }
        else
        {
            return false;
        }
    }


    public void SetTarget(Transform newTarget) => _Target = newTarget;
    public void SetTargetDesiredDistance(float distance, float m_distance = 2.0f)
    {
        SetMaxAttackDistance(distance + 1.0f);
    }
    public void SetMaxAttackDistance(float m_distance = 2.0f) => _MaxDistance = m_distance;
}
