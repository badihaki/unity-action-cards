using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    private NonPlayerCharacter _NPC;
    [field: SerializeField] public bool _IsAggressive { get; private set; }
    [SerializeField] private Transform _Aggressor;

    public Transform _MainTarget { get; private set; }
    public List<Transform> _SideTargets { get; private set; }
    [field: SerializeField] public float _DesiredAttackDistance { get; private set; }
    [SerializeField] private bool _AttackTicket;
    
    public void InitiateAttack(NonPlayerCharacter character)
    {
        _NPC = character;
        _AttackTicket = true;
        SetDesiredAttackDistance(3.5f);
        if (_NPC._Hurtbox) _NPC._Hurtbox.DetermineWhoWhurtMe += SetNewAggressor;
    }

    public void SetDesiredAttackDistance(float distance = 1.5f)
    {
        _DesiredAttackDistance = distance;
        _NPC._NavigationController.SetTargetDesiredDistance(_DesiredAttackDistance);
    }

    private void SetNewAggressor(Transform aggressor)
    {
        _Aggressor = aggressor;
        int roll = GameManagerMaster.GameMaster.Dice.RollD10();
        if (roll < 5) MakeAggressive(aggressor);
    }

    public void MakeAggressive(Transform aggressor)
    {
        _IsAggressive = true;
        if (!_Aggressor) _MainTarget = aggressor;
        else _MainTarget = _Aggressor;
        if (!_AttackTicket) _AttackTicket = true;
        _NPC._NavigationController.SetTarget(_MainTarget);
    }
}
