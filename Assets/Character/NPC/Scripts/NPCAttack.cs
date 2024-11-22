using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    private NonPlayerCharacter _NPC;

    [field: SerializeField]
    public Transform _ActiveTarget { get; private set; }
    [field: SerializeField] public float _DesiredAttackDistance { get; private set; }
    [field: SerializeField] public float _MaxAttackDistance { get; private set; }
    [SerializeField] private bool _AttackTicket;
    
    public void InitiateAttack(NonPlayerCharacter character)
    {
        _NPC = character;
        _AttackTicket = true;
        SetDesiredAttackDistance(3.5f);
        if (_NPC._Hurtbox) _NPC._Hurtbox.DetermineWhoWhurtMe += SetNewTarget;
    }

    public void SetDesiredAttackDistance(float distance = 1.5f)
    {
        _DesiredAttackDistance = distance;
        _MaxAttackDistance = distance + 3.25f;
        _NPC._NavigationController.SetTargetDesiredDistance(_DesiredAttackDistance);
    }

    public void SetNewTarget(Transform aggressor)
    {
        _ActiveTarget = aggressor;
    }

    public float GetDistanceFromTarget()
    {
        float dist = Vector3.Distance(_NPC._NPCActor.transform.position, _ActiveTarget.position);
        return dist;
    }
}
