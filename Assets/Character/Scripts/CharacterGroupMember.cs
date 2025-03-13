using UnityEngine;

public class CharacterGroupMember : MonoBehaviour
{
    [field: SerializeField, Header("Group Member Specifics")]
    protected Character _Character;
    [field: SerializeField]
	public CharacterGroupLeader _GroupLeader { get; private set; }

    public void Initialize(CharacterGroupLeader Leader)
    {
        _Character = GetComponent<Character>();
        _GroupLeader = Leader;
        if(_GroupLeader != this)
        {
            _GroupLeader._Character._Actor._Hitbox.OnHit += TryGetTargetOfLeader;
            _GroupLeader._Character._Actor._Hurtbox.OnHurtByCharacter += TryGetCharacterAttackingLeader;
        }
	}

	private void OnDisable()
	{
		if (_GroupLeader != this)
        {
			_GroupLeader._Character._Actor._Hitbox.OnHit -= TryGetTargetOfLeader;
		    _GroupLeader._Character._Actor._Hurtbox.OnHurtByCharacter -= TryGetCharacterAttackingLeader;
        }
	}

	public bool CheckIfPartOfMyGroup(CharacterGroupMember characterGroupMember)
    {
        if (characterGroupMember._GroupLeader == _GroupLeader)
            return true;
		return false;
    }

    private void TryGetTargetOfLeader(Transform target)
    {
        target.TryGetComponent(out Character targetChar);
        if (targetChar == null) return;
        AttackTargetCharacter(targetChar);
    }
	private void TryGetCharacterAttackingLeader(Character target)
    {
        NPCAttackController attackController = _Character._AttackController as NPCAttackController;
        if (attackController._ActiveTarget == null)
        {
            AttackTargetCharacter(target);
            return;
        }
        if (GameManagerMaster.GameMaster.Dice.RollD10() >= 6)
            AttackTargetCharacter(target);
    }

	public void AttackTargetCharacter(Character targetChar)
    {
        NonPlayerCharacter npc = _Character as NonPlayerCharacter;
        npc._NPCActor._AggressionManager.AddAggression(100, targetChar);
        npc._NavigationController.SetTarget(targetChar._Actor.transform);
    }

    public void GoToPosition(Transform targetPosition)
    {
		NonPlayerCharacter npc = _Character as NonPlayerCharacter;
        npc._NavigationController.SetTarget(targetPosition);
	}

	// end of the line
}
