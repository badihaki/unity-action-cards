using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    private NonPlayerCharacter _NPC;
    [field: SerializeField] public bool _IsAggressive { get; private set; }

    public Transform _MainTarget { get; private set; }
    public List<Transform> _SideTargets { get; private set; }
    [SerializeField] private bool _AttackTicket;
    
    public void InitiateAttack(NonPlayerCharacter character)
    {
        _NPC = character;
        _AttackTicket = true;
    }

    public void MakeAggressive(Transform aggressor)
    {
        _IsAggressive = true;
        _MainTarget = aggressor;
    }
}
