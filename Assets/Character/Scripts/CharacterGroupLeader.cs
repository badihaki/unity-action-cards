using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterGroupLeader : CharacterGroupMember
{
    [field: Header("Group Leader Specifics"), SerializeField]
    protected Character _LeaderCharacter;

    // group member struct
    [Serializable]
    protected struct GroupMember
    {
        public string Name;
        public NonPlayerCharacter Character;
        public Actor Actor;
        public CharacterGroupMember Group;
        public float Distance;

        public GroupMember(string name, NonPlayerCharacter character, Actor actor, CharacterGroupMember grouping, float dist)
        {
            Name = name;
            Character = character;
            Actor = actor;
            Group = grouping;
            Distance = dist;
        }
    }
	[field: Header("Group Member Properties"), SerializeField]
    protected List<GroupMember> _GroupMembers { get; private set; }
    
    [field: SerializeField, Tooltip("The leash is how far entities can move from the leader")]
    public float _GroupLeashDistance { get; private set; } = 3.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _LeaderCharacter = GetComponent<Character>();
        _GroupMembers = new List<GroupMember>();
        Initialize(this);
    }

	private void Update()
	{
        if (_GroupMembers.Count > 0)
			ManageGroup();
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
        print($"character actor: {character._Actor != null}, new group member: {newGroupMember != null}");
        float distance = Vector3.Distance(_Character._Actor.transform.position, character._Actor.transform.position);
		GroupMember newMemberStruct = new(character.name, character as NonPlayerCharacter, character._Actor, newGroupMember, distance);
        _GroupMembers.Add(newMemberStruct);
        newGroupMember.Initialize(this);
    }

    private void ManageGroup()
    {
        ManageGroupDistance();
    }

	private void ManageGroupDistance()
	{
		foreach (var character in _GroupMembers)
		{
			// chekc the distance
            // if too far, call go to leader on the group member
		}
	}

    // end
}
