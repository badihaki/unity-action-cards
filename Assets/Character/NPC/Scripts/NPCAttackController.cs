using System.Collections;
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

    public float gottenDistance;
    public bool highUp;
    
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
        
        if (_NPC._Hurtbox) _NPC._Hurtbox.DetermineWhoWhurtMe += SetNewTargetEnemy;
    }

    public void SetNewTargetEnemy(Transform aggressor)
    {
        _ActiveTarget = aggressor;
    }

    public float GetDistanceFromTarget()
    {
        float dist;
        //float myY = _NPC._NPCActor.transform.position.y > 0 ? _NPC._NPCActor.transform.position.y : _NPC._NPCActor.transform.position.y * -1;
        //float targetY = _ActiveTarget.position.y > 0 ? _ActiveTarget.position.y : _ActiveTarget.position.y * -1;

        float yDistance = _NPC._NPCActor.transform.position.y - _ActiveTarget.position.y;
        if (yDistance < 1 && yDistance > -1)
        {
            Vector3 myPos = _NPC._NPCActor.transform.position;
            myPos.y = 0;
            Vector3 targetPos = _ActiveTarget.transform.position;
            targetPos.y = 0;
            dist = Vector3.Distance(myPos, targetPos);
            highUp = false;
		}
        else
        {
            dist = Vector3.Distance(_NPC._NPCActor.transform.position, _ActiveTarget.position);
            highUp = true;
        }
        gottenDistance = dist;
        return dist;
    }

	//public override void SetAttackParameters(bool knockback, bool launch, int damageModifier = 0)
	public override void SetAttackParameters(responsesToDamage intendedDmgResponse = responsesToDamage.hit, int damageModifier = 0, float force = 1)
	{
		base.SetAttackParameters(intendedDmgResponse, damageModifier, force);
		_Damage = _NPC._MoveSet.GetCurrentAttack().damage + damageModifier;
        // calculate force here
        //base.SetAttackParameters(knockback, launch);
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
