using UnityEngine;

public class PlayerInteractableDetector : MonoBehaviour
{
    private PlayerInteractionController _InteractableController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _InteractableController = GetComponentInParent<PlayerInteractionController>();
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.TryGetComponent(out IInteractable interactable))
        {
            _InteractableController.SetNewActiveInteractable(interactable);
        }
       
	}

}
