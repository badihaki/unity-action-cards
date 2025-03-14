using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterGroupLeader : CharacterGroupMember
{
    [field: Header("Group Leader Specifics"), SerializeField]
    protected Character _LeaderCharacter;

    // group member struct
    [Serializable]
	public struct GroupMemberStruct
    {
        public string Name;
        public NonPlayerCharacter Character;
        public Actor Actor;
        public CharacterGroupMember GroupMemberController;
        public float DistanceFromLeader;

        public bool commandedToMove;
        public bool commandedToAttack;

        public GroupMemberStruct(string name, NonPlayerCharacter character, Actor actor, CharacterGroupMember grouping, float dist)
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
    public List<GroupMemberStruct> _GroupMembers { get; private set; }
    
    [field: SerializeField, Tooltip("The leash is how far entities can move from the leader")]
    public float _GroupLeashDistance { get; private set; } = 3.85f;
    [field: SerializeField, Tooltip("The maximum length of the leash")]
	public float _GroupMaxLeashDistance { get; private set; } = 12.0f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        _LeaderCharacter = GetComponent<Character>();
        _GroupMembers = new List<GroupMemberStruct>();
        Initialize(this);
        if(_LeaderCharacter is NonPlayerCharacter)
        {
            NonPlayerCharacter npc = _LeaderCharacter as NonPlayerCharacter;
            npc.GetGroupedUp(this);
        }
    }

	private void Update()
	{
        if (_GroupMembers.Count > 0)
			ManageGroup();
	}

	public void AddCharacterToGroup(NonPlayerCharacter character)
    {
		// add a group member class to the character
		character.TryGetComponent(out CharacterGroupMember newGroupMember);
        if (newGroupMember == null)
        {
            newGroupMember = character.AddComponent<CharacterGroupMember>();
        }
        // set the character's group leader to this character
        float distance = Vector3.Distance(_Character._Actor.transform.position, character._Actor.transform.position);
		GroupMemberStruct newMemberStruct = new(character.name, character, character._Actor, newGroupMember, distance);
        _GroupMembers.Add(newMemberStruct);
        newGroupMember.Initialize(this);
        character.GetGroupedUp(newGroupMember);
        character._Actor.onDeath += RemoveCharacterFromGroup;
    }

	private void RemoveCharacterFromGroup(Character character)
	{
		character.TryGetComponent(out CharacterGroupMember newGroupMember);
        GroupMemberStruct removingMember = _GroupMembers.Find(member=>member.Character == character);
        _GroupMembers.Remove(removingMember);
        // _GroupMembers.IndexOf()
	}

	private void ManageGroup()
    {
        ManageGroupDistance();
		LeashGroupMembers();
    }

	private void LeashGroupMembers()
	{
		// if too far, call go to leader on the group member
		List<GroupMemberStruct> groupMembers = new List<GroupMemberStruct>(_GroupMembers);
		foreach (GroupMemberStruct character in groupMembers)
		{
			GroupMemberStruct newCharacterStats = character;
			if (newCharacterStats.DistanceFromLeader > _GroupMaxLeashDistance && !newCharacterStats.commandedToMove)
            {
                newCharacterStats.Character._NavigationController.SetTarget(_LeaderCharacter, _GroupLeashDistance - 1);
                newCharacterStats.commandedToMove = true;

				int index = _GroupMembers.IndexOf(character);
				_GroupMembers[index] = newCharacterStats;

                StartCoroutine(MoveGroupMemberToLeader(_GroupMembers[index], index));
			}
		}
	}

	private void ManageGroupDistance()
	{
        List<GroupMemberStruct> groupMembers = new List<GroupMemberStruct>(_GroupMembers);
		foreach (GroupMemberStruct character in groupMembers)
		{
            // chekc the distance
            GroupMemberStruct newCharacterStats = character;
			float distance = Vector3.Distance(_Character._Actor.transform.position, character.Character._Actor.transform.position);
            newCharacterStats.DistanceFromLeader = distance;
            
            int index = _GroupMembers.IndexOf(character);
            _GroupMembers[index] = newCharacterStats;
		}
	}

    private IEnumerator MoveGroupMemberToLeader(GroupMemberStruct groupMember, int memberIndexNum)
    {
        GroupMemberStruct copyGroupMember = groupMember;
        while (_GroupMembers.Count > 0 && _GroupMembers[memberIndexNum].commandedToMove)
        {
            if(_GroupMembers[memberIndexNum].DistanceFromLeader <= _GroupLeashDistance)
            {
                copyGroupMember.commandedToMove = false;
		        _GroupMembers[memberIndexNum] = copyGroupMember;
            }
            else
            {
				copyGroupMember.DistanceFromLeader = Vector3.Distance(_LeaderCharacter._Actor.transform.position, _Character._Actor.transform.position);
				_GroupMembers[memberIndexNum] = copyGroupMember;
				yield return null;
            }
		}
        if (copyGroupMember.Character._NavigationController._NavTarget == copyGroupMember.GroupMemberController._GroupLeader._Character._Actor.transform)
            copyGroupMember.Character._NavigationController.RemoveTarget();
		_GroupMembers[memberIndexNum] = copyGroupMember;
        groupMember.Character._StateMachine.ChangeState(groupMember.Character._StateMachine._StateLibrary._IdleState);
	}

    // end
}
