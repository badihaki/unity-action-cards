using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttackController : CharacterAttackController
{
    private NonPlayerCharacter _NPC;

    [SerializeField] private bool _AttackTicket;
    [field: SerializeField, Header("Attacking Target")]
    public Transform _ActiveTarget { get; private set; }
    
    [field: SerializeField, Header("Distances")]

    private WaitForSeconds shortAttackWait = new WaitForSeconds(0.673f);
    private WaitForSeconds longAttackWait = new WaitForSeconds(1.465f);
    private WaitForSeconds superLongAttackWait = new WaitForSeconds(3.15f);
    public enum AttackWaitType
    {
        Short,
        Long,
        SuperLong
    }

	public override void Initialize(Character character)
	{
        _NPC = character as NonPlayerCharacter;
        _AttackTicket = true;
        
        if (_NPC._Hurtbox) _NPC._Hurtbox.DetermineWhoWhurtMe += SetNewTarget;
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

	public override void SetAttackParameters(bool knockback, bool launch, int damageModifier = 0)
	{
        _Damage = _NPC._MoveSet.GetCurrentAttack().damage + damageModifier;
		base.SetAttackParameters(knockback, launch);
	}

	public void UseAttackTicket()
    {
        _AttackTicket = false;
    }

    public void EndAttackAddWait(float waitTime)
    {
        AttackWaitType waitType;
        if (waitTime < 2) waitType = AttackWaitType.Short;
        else if (waitTime < 3.75f) waitType = AttackWaitType.Long;
        else waitType = AttackWaitType.SuperLong;
        StartCoroutine(WaitToAttack(waitType));
    }

	private IEnumerator WaitToAttack(AttackWaitType waitType)
	{
        switch (waitType)
        {
            case AttackWaitType.Short:
                yield return shortAttackWait;
                break;
			case AttackWaitType.Long:
				yield return longAttackWait;
				break;
			case AttackWaitType.SuperLong:
				yield return superLongAttackWait;
				break;
		}
        _AttackTicket = true;
	}
}
