using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    // we need to know if the player can interact with something right now
    // lets how an IInterablable
    public PlayerCharacter player;
	[SerializeField]
	private bool canInteract = true;
	[SerializeField]
	private bool isInteracting = false;

    public IInteractable activeInteractable { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerCharacter>();
		canInteract = true;
		isInteracting = false;
    }

	private void Update()
	{
		DetectInteract();
		if (isInteracting)
			DetectCancelInteract();
	}

	private void DetectInteract()
	{
		if (activeInteractable != null && canInteract)
		{
			if (player._Controls._InteractInput)
			{
				player._Controls.UseInteract();
				activeInteractable.Interact(player);
				player._StateMachine.ChangeState(player._StateMachine._CinemaWaitState);
				player._CameraController.UnlockCursorKBM();
				player._Controls.SetInputMap(1);
				canInteract = false;
				isInteracting = true;
			}
		}
	}

	private void DetectCancelInteract()
	{
		if(player._Controls._UiCancelInput)
		{
			activeInteractable.StopInteraction(player);
			StopPlayerInteraction();
		}
	}

	public void StopPlayerInteraction()
	{
		player._StateMachine.ChangeState(player._StateMachine._IdleState);
		player._CameraController.LockCursorKBM();
		player._Controls.SetInputMap(0);
		canInteract = true;
		isInteracting = false;
	}

	public void TrySetActiveInteractable(IInteractable interactable)
    {
		if (activeInteractable == null)
			activeInteractable = interactable;
		else
			SetClosestInteractableAsActive(interactable);
    }
    public void RemoveActiveInteractable(IInteractable interactable)
    {
        if (interactable == activeInteractable)
            activeInteractable = null;
		print("unbsetting interactable");
	}

	private void SetClosestInteractableAsActive(IInteractable interactable)
	{
		IInteractable selectedInteractable = interactable;
		float distanceFromNewInteractable = Vector3.Distance(player._PlayerActor.transform.position, interactable.GetControllingEntity().position);
		float distanceFromActiveInteractable = Vector3.Distance(player._PlayerActor.transform.position, activeInteractable.GetControllingEntity().position);
	}

	// end
}
