using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    private NonPlayerCharacter _NPC;

    public Transform _MainTarget { get; private set; }
    public List<Transform> _SideTargets { get; private set; }
    [SerializeField] private bool _AttackTicket;
    
    public void InitiateAttack(NonPlayerCharacter character)
    {
        _NPC = character;
        _AttackTicket = true;
    }
}
