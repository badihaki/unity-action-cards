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

    // end of the line
}
