using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractableDetector : MonoBehaviour
{
    private PlayerInteractionController _InteractableController;
    private List<IInteractable> queuedInteractionsList = new List<IInteractable>();
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
            if (queuedInteractionsList.Count <= 0)
                _InteractableController.SetNewActiveInteractable(interactable);
            else
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
        }
       
	}

	private void OnTriggerExit(Collider other)
	{
        if (other.TryGetComponent(out IInteractable interactable))
        {
            if (queuedInteractionsList.Contains(interactable))
                queuedInteractionsList.Remove(interactable);
            _InteractableController.RemoveActiveInteractable(interactable);
        }
	}

}
