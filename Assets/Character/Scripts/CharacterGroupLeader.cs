using System;
using System.Collections;
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
        public CharacterGroupMember GroupMemberController;
        public float DistanceFromLeader;

        public bool commandedToMove;
        public bool commandedToAttack;

        public GroupMember(string name, NonPlayerCharacter character, Actor actor, CharacterGroupMember grouping, float dist)
        {
            Name = name;
            Character = character;
            Actor = actor;
            GroupMemberController = grouping;
            DistanceFromLeader = dist;

            commandedToMove = false;
            commandedToAttack = false;
        }
    }
	[field: Header("Group Member Properties"), SerializeField]
    protected List<GroupMember> _GroupMembers { get; private set; }
    
    [field: SerializeField, Tooltip("The leash is how far entities can move from the leader")]
    public float _GroupLeashDistance { get; private set; } = 12.0f;

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
		LeashGroupMembers();
    }

	private void LeashGroupMembers()
	{
		// if too far, call go to leader on the group member
		List<GroupMember> groupMembers = new List<GroupMember>(_GroupMembers);
		foreach (GroupMember character in groupMembers)
		{
			GroupMember newCharacterStats = character;
			if (newCharacterStats.DistanceFromLeader > _GroupLeashDistance && !newCharacterStats.commandedToMove)
            {
                newCharacterStats.Character._NavigationController.SetTarget(_LeaderCharacter._Actor.transform);
                newCharacterStats.commandedToMove = true;

				int index = _GroupMembers.IndexOf(character);
				_GroupMembers[index] = newCharacterStats;

                StartCoroutine(MoveGroupMemberToLeader(_GroupMembers[index], index));
			}
		}
	}

	private void ManageGroupDistance()
	{
        List<GroupMember> groupMembers = new List<GroupMember>(_GroupMembers);
		foreach (GroupMember character in groupMembers)
		{
            // chekc the distance
            GroupMember newCharacterStats = character;
			float distance = Vector3.Distance(_Character._Actor.transform.position, character.Actor.transform.position);
            newCharacterStats.DistanceFromLeader = distance;
            
            int index = _GroupMembers.IndexOf(character);
            _GroupMembers[index] = newCharacterStats;
		}
	}

    private IEnumerator MoveGroupMemberToLeader(GroupMember groupMember, int memberIndexNum)
    {
		while (groupMember.DistanceFromLeader > _GroupLeashDistance)
        {
            yield return null;
        }
        GroupMember copyGroupMember = groupMember;
		copyGroupMember.Character._NavigationController.SetTarget(null);
		copyGroupMember.commandedToMove = false;
		_GroupMembers[memberIndexNum] = copyGroupMember;
	}

    // end
}
