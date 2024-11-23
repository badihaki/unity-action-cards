using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttackController : MonoBehaviour
{
    private NonPlayerCharacter _NPC;

    [SerializeField] private bool _AttackTicket;
    [field: SerializeField, Header("Attacking Target")]
    public Transform _ActiveTarget { get; private set; }
    
    [field: SerializeField, Header("Distances")]
    public float _DesiredAttackDistance { get; private set; }
    [field: SerializeField] public float _MaxAttackDistance { get; private set; }

    private WaitForSeconds shortAttackWait = new WaitForSeconds(1.35f);
    private WaitForSeconds longAttackWait = new WaitForSeconds(3.65f);
    private WaitForSeconds superLongAttackWait = new WaitForSeconds(7.15f);
    public enum AttackWaitType
    {
        Short,
        Long,
        SuperLong
    }

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

    public void UseAttackTicket()
    {
        _AttackTicket = false;
    }

    public void EndAttackAddWait(AttackWaitType waitType)
    {
        StartCoroutine(WaitToAttack(waitType));
    }

	private IEnumerator WaitToAttack(AttackWaitType waitType)
	{
        switch (waitType)
        {
            case AttackWaitType.Short:
                print("attack wait short <<<<<<<<<<<<<<<<<<");
                yield return shortAttackWait;
                break;
			case AttackWaitType.Long:
				print("attack wait long <<<<<<<<<<<<<<<<<<");
				yield return longAttackWait;
				break;
			case AttackWaitType.SuperLong:
				print("attack wait supaLong <<<<<<<<<<<<<<<<<<");
				yield return superLongAttackWait;
				break;
		}
        _AttackTicket = true;
	}
}
