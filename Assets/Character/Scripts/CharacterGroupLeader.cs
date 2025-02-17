using System.Collections.Generic;
using UnityEngine;

public class CharacterGroupLeader : MonoBehaviour
{
    [field: Header("The group members"), SerializeField]
    private Character character;
	[field: Header("The group members"), SerializeField]
    public List<CharacterGroupMember> _GroupMembers { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
