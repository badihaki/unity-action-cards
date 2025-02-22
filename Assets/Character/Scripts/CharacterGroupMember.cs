using UnityEngine;

public class CharacterGroupMember : MonoBehaviour
{
    [field: SerializeField]
    private NonPlayerCharacter npc;
    [field: SerializeField]
	public CharacterGroupLeader _GroupLeader { get; private set; }

    public void Initialize(CharacterGroupLeader Leader)
    {
        npc = GetComponent<NonPlayerCharacter>();
        _GroupLeader = Leader;
    }

    // end of the line
}
