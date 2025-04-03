using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    // we need to know if the player can interact with something right now
    // lets how an IInterablable
    public PlayerCharacter player;
	[SerializeField]
    private bool canInteract = true;

	public List<IInteractable> queuedInteractionsList;
    public IInteractable activeInteractable { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerCharacter>();
		queuedInteractionsList = new List<IInteractable>();
		canInteract = true;
    }

	private void Update()
	{
		DetectInteract();
	}

	private void DetectInteract()
	{
		if (activeInteractable != null && canInteract)
		{
			if (player._Controls._InteractInput)
			{
				OpenMenu();
			}
		}
	}

	private void OpenMenu()
	{
		player._Controls.UseInteract();
		player._StateMachine.ChangeState(player._StateMachine._CinemaWaitState);
		player._Controls.SetInputMap(1);
		player._UIController.OpenInteractionMenu();
		canInteract = false;
	}

	public void CloseMenu()
	{
		player._Controls.SetInputMap(0);
		ResetCanInteract();
	}

	public void ResetCanInteract() => canInteract = true;

	public void SetNewActiveInteractable(IInteractable interactable)
    {
        activeInteractable = interactable;
        print("setting new interactable");
    }
    public void RemoveActiveInteractable(IInteractable interactable)
    {
        if (interactable == activeInteractable)
            activeInteractable = null;
		print("unbsetting interactable");
	}

	public void AddInteractableToList(IInteractable interactable)
	{
		if (queuedInteractionsList.Count <= 0)
			SetNewActiveInteractable(interactable);
		else
		{
			SetClosestInteractableAsActive(interactable);
		}
	}
	public void RemoveInteractableFromList(IInteractable interactable)
	{
		if (queuedInteractionsList.Contains(interactable))
			queuedInteractionsList.Remove(interactable);
		RemoveActiveInteractable(interactable);
	}

	private void SetClosestInteractableAsActive(IInteractable interactable)
	{
		IInteractable selectedInteractable = interactable;
		float distance = Vector3.Distance(player._PlayerActor.transform.position, interactable.GetControllingEntity().position);
		queuedInteractionsList.ForEach(compareInteractable =>
		{
			float compareInteractableDistance = Vector3.Distance(player._PlayerActor.transform.position, compareInteractable.GetControllingEntity().position);
			if (compareInteractableDistance < distance)
				selectedInteractable = compareInteractable;
		});
	}

	// end
}
