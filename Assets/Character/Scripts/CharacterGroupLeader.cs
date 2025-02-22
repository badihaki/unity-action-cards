using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterGroupLeader : MonoBehaviour
{
    [field: Header("The controlling character"), SerializeField]
    private Character _Character;
	[field: Header("The group members"), SerializeField]
    public List<CharacterGroupMember> _GroupMembers { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Character = GetComponent<Character>();
    }

    public void AddCharacterToGroup(Character character)
    {
        // add a group member class to the character
        character.TryGetComponent<CharacterGroupMember>(out CharacterGroupMember newGroupMember);
        if (newGroupMember == null)
        {
            newGroupMember = character.AddComponent<CharacterGroupMember>();
        }
        // set the character's group leader to this character
        newGroupMember.Initialize(this);
    }

    // end
}
