using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour, IInteractable
{
	[SerializeField]
	private Transform controllingEntity;
	public Transform GetControllingEntity()
	{
		return controllingEntity;
	}


	[SerializeField]
	private List<InteractionScriptableObj> interactions;
	public void Interact(Character interactingCharacter)
	{
		throw new System.NotImplementedException();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
