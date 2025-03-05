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
    }

    public bool CheckIfPartOfMyGroup(CharacterGroupMember characterGroupMember)
    {
        if (characterGroupMember._GroupLeader == _GroupLeader)
            return true;
		return false;
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
