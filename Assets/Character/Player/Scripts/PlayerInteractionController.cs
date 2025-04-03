using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    // we need to know if the player can interact with something right now
    // lets how an IInterablable
    public IInteractable activeInteractable { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetNewActiveInteractable(IInteractable interactable)
    {
        activeInteractable = interactable;
        print("setting new interactable");
    }
    public void RemoveActiveInteractable()
    {
        activeInteractable = null;
		print("unbsetting interactable");
	}

	// end
}
