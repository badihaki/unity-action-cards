using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractableDetector : MonoBehaviour
{
    private PlayerInteractionController _InteractableController;
    private PlayerCharacter player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _InteractableController = GetComponentInParent<PlayerInteractionController>();
        player = GetComponentInParent<PlayerCharacter>();
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.TryGetComponent(out IInteractable interactable))
        {
            _InteractableController.AddInteractableToList(interactable);
        }
       
	}

	private void OnTriggerExit(Collider other)
	{
        if (other.TryGetComponent(out IInteractable interactable))
        {
            _InteractableController.RemoveActiveInteractable(interactable);
        }
	}

}
