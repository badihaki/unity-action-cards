using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    // we need to know if the player can interact with something right now
    // lets how an IInterablable
    public PlayerCharacter player;
    public IInteractable activeInteractable { get; private set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerCharacter>();
    }

	private void Update()
	{
        if (activeInteractable != null)
        {
            if (player._Controls._InteractInput)
            {
                player._Controls.UseInteract();
                player._StateMachine.ChangeState(player._StateMachine._CinemaWaitState);
			}
        }
	}

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

	// end
}
