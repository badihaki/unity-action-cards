using UnityEngine;

public class CharacterGroupMember : MonoBehaviour
{
    [field: SerializeField]
    private NonPlayerCharacter npc;
    [field: SerializeField]
	public CharacterGroupLeader _GroupLeader { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npc = GetComponent<NonPlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
